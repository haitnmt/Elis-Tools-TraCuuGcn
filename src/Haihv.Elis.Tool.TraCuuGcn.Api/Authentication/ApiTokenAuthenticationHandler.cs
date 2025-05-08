using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Haihv.Elis.Tool.TraCuuGcn.Api.Authentication;

public class ApiTokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "ApiToken";
    private readonly IConfiguration _configuration;

    [Obsolete("Obsolete")]
    public ApiTokenAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration)
        : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Lấy API token từ header (ví dụ: X-Api-Key)
        if (!Request.Headers.TryGetValue("X-Api-Key", out var apiKey))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        // Lấy giá trị token hợp lệ từ cấu hình (hoặc có thể kiểm tra DB)
        var validToken = _configuration["ApiToken:Key"];
        if (string.IsNullOrWhiteSpace(validToken) || apiKey != validToken)
        {
            return Task.FromResult(AuthenticateResult.Fail("API Token không hợp lệ"));
        }

        var claims = new[] { new Claim(ClaimTypes.Name, "ApiTokenUser") };
        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
