﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.PwnedApi;

static class WebApplicationExtensions
{
    /// <summary>
    /// Maps "pwned breach data" endpoints and "pwned passwords" endpoints, with Minimal APIs.
    /// </summary>
    /// <param name="app">The current <see cref="WebApplication"/> instance to map on.</param>
    /// <returns>The given <paramref name="app"/> as a fluent API.</returns>
    /// <exception cref="ArgumentNullException">When <paramref name="app"/> is <c>null</c>.</exception>
    internal static WebApplication MapPwnedEndpoints(this WebApplication app)
    {
        ArgumentNullException.ThrowIfNull(app);

        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapBreachEndpoints();
        app.MapPwnedPasswordsEndpoints();

        return app;
    }

    internal static WebApplication MapBreachEndpoints(this WebApplication app)
    {
        // Map "have i been pwned" breaches.
        app.MapGet("api/pwned/breaches/{email}", GetBreachHeadersForAccountAsync);
        app.MapGet("api/pwned/breach/{name}", GetBreachAsync);

        return app;
    }

    internal static WebApplication MapPwnedPasswordsEndpoints(this WebApplication app)
    {
        // Map "have i been pwned" passwords.
        app.MapGet("api/pwned/passwords/{password}", GetPwnedPasswordAsync);

        return app;
    }

    [Authorize, RequiredScope("User.ApiAccess"), EnableCors]
    internal static async Task<IResult> GetBreachHeadersForAccountAsync(
        [FromRoute] string email,
        [FromServices] IPwnedServices pwnedServices)
    {
        var breaches = await pwnedServices.GetBreachHeadersAsync(email);
        return Results.Json(breaches, DefaultJsonSerialization.Options);
    }

    [Authorize, RequiredScope("User.ApiAccess"), EnableCors]
    internal static async Task<IResult> GetBreachAsync(
        [FromRoute] string name,
        [FromServices] IPwnedServices pwnedServices)
    {
        var breach = await pwnedServices.GetBreachDetailsAsync(name);
        return Results.Json(breach, DefaultJsonSerialization.Options);
    }

    [Authorize, RequiredScope("User.ApiAccess"), EnableCors]
    internal static async Task<IResult> GetPwnedPasswordAsync(
        [FromRoute] string password,
        [FromServices] IPwnedPasswordsClient pwnedPasswordsClient)
    {
        var pwnedPassword = await pwnedPasswordsClient.GetPwnedPasswordAsync(password);
        return Results.Json(pwnedPassword, DefaultJsonSerialization.Options);
    }
}
