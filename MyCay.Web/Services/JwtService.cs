using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyCay.Web.Services;

public interface IJwtService
{
    string GenerateToken(int userId, string email, string role, string name);
    ClaimsPrincipal? ValidateToken(string token);
    (int userId, string email, string role)? ParseToken(string token);
}

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expirationHours;

    public JwtService(IConfiguration config)
    {
        _config = config;
        _secretKey = config["Jwt:SecretKey"] ?? "MyCaySasin_DefaultKey_2024_32Chars!";
        _issuer = config["Jwt:Issuer"] ?? "MyCaySasin";
        _audience = config["Jwt:Audience"] ?? "MyCaySasinApp";
        _expirationHours = int.Parse(config["Jwt:ExpirationHours"] ?? "24");
    }

    public string GenerateToken(int userId, string email, string role, string name)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.Name, name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_expirationHours),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token)) return null;

        // Remove "Bearer " prefix if present
        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token[7..];

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return principal;
        }
        catch
        {
            return null;
        }
    }

    public (int userId, string email, string role)? ParseToken(string token)
    {
        var principal = ValidateToken(token);
        if (principal == null) return null;

        var userIdClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        var email = principal.FindFirst(JwtRegisteredClaimNames.Email)?.Value ?? "";
        var role = principal.FindFirst(ClaimTypes.Role)?.Value ?? "customer";

        if (int.TryParse(userIdClaim, out var userId))
            return (userId, email, role);

        return null;
    }
}
