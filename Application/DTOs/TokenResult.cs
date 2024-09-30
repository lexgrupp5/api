using Application.Models;

namespace Application.DTOs;

public record TokenResult(string Access, RefreshCookieParameter Cookie);
