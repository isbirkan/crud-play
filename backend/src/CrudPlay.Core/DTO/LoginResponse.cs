﻿using System.Text.Json.Serialization;

namespace CrudPlay.Core.DTO;

public class LoginResponse
{
    [JsonPropertyName("token_type")]
    public string? TokenType { get; set; }

    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}
