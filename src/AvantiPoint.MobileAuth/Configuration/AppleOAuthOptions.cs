using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AvantiPoint.MobileAuth.Configuration;

internal class AppleOAuthOptions
{
    public string? ServiceId { get; set; }

    public string? KeyId { get; set; }

    public string? TeamId { get; set; }

    public string? PrivateKey { get; set; }

    public void Configure(AuthenticationBuilder builder, WebApplicationBuilder appBuilder)
    {
        if (string.IsNullOrEmpty(ServiceId) || string.IsNullOrEmpty(KeyId) || string.IsNullOrEmpty(TeamId))
            return;

        else
            builder.AddApple(o =>
            {
                o.ClientId = ServiceId;
                o.KeyId = KeyId;
                o.TeamId = TeamId;

                if (!string.IsNullOrEmpty(PrivateKey))
                    o.PrivateKey = (k, c) => Task.FromResult(PrivateKey.AsMemory());
                else
                    o.UsePrivateKey(keyId =>
                        appBuilder.Environment.ContentRootFileProvider.GetFileInfo(Path.Combine("App_Data", $"AuthKey_{keyId}.p8")));
                o.SaveTokens = true;
            });
    }
}
