using Microsoft.AspNetCore.Http;

namespace Application.Models;

public record RefreshCookieParameter(
    string Token,
    CookieOptions Options,
    string Key = "RefreshToken"
);
