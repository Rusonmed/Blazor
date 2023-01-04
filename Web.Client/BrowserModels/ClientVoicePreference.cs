﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.BrowserModels;

public record class ClientVoicePreference(
    [property: JsonPropertyName("voice")] string Voice,
    [property: JsonPropertyName("voiceSpeed")] double VoiceSpeed);
