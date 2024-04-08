using Supplier.Business.Interfaces;
using System.Net.Http.Headers;

namespace Supplier.Business.Services.Handles
{
    public class HttpClientAuthorizationDelegatingHandler : DelegatingHandler
    {
        private readonly IUser _user;

        public HttpClientAuthorizationDelegatingHandler(IUser user)
        {
            _user = user;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorizationHeader = _user.ObterHttpContext().Request.Headers["Authorization"];

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                request.Headers.Add("Authorization", new List<string>() { authorizationHeader });

                if (_user.EstaAutenticado())
                {
                    var auxToken = authorizationHeader.FirstOrDefault().Split(" ");
                    var token = auxToken[1];

                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
            }
                        
            return base.SendAsync(request, cancellationToken);
        }
    }
}
