using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace UTB.School.Web
{
    public class TokenHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(httpContextAccessor.HttpContext, "HttpContext not available");

            string? accessToken = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
