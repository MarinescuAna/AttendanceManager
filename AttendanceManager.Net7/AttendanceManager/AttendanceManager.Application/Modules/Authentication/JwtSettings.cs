﻿namespace AttendanceManager.Application.Models.Authentication
{
    public sealed class JwtSettings
    {
        public required string Key { get; init; }
        public required string Issuer { get; init; }
        public required string Audience { get; init; }
        public required double RefreshTokenExpirationDays { get; init; }
        public required double AccessTokenExpirationMinutes { get; init; }
    }
}
