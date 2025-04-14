using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services
{
    /// <summary>
    /// Handles authentication endpoints.
    /// </summary>
    public class AuthEndpointCookieHandler : DelegatingHandler
    {
        /// <summary>
        /// Main method to override for the handler.
        /// </summary>
        /// <param name="request">The original request.</param>
        /// <param name="cancellationToken">The token to handle cancellations.</param>
        /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // include cookies!
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
