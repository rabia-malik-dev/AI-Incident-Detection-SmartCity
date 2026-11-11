using Microsoft.EntityFrameworkCore;
using SmartCity.Api.Contracts;
using SmartCity.Api.Data;
using SmartCity.Api.Entities;

namespace SmartCity.Api.Services;

public class AuthService(IncidentDbContext db, JwtTokenService tokens)
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(u => u.Email == email, ct))
            throw new InvalidOperationException("Email already registered.");

        if (request.Role is not ("Admin" or "TrafficOfficer"))
            throw new InvalidOperationException("Role must be Admin or TrafficOfficer.");

        var user = new User
        {
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            DisplayName = request.DisplayName.Trim(),
            Role = request.Role,
            CreatedAtUtc = DateTime.UtcNow,
        };
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        var (token, expires) = tokens.CreateToken(user);
        return new AuthResponse(token, user.Email, user.DisplayName, user.Role, expires);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken ct)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        var (token, expires) = tokens.CreateToken(user);
        return new AuthResponse(token, user.Email, user.DisplayName, user.Role, expires);
    }
}
