using AuthJWT.Providers;
using AuthJWT.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AuthJWT.Controllers
{
    [Produces("application/json")]
    [Route("api/token")]
    public class TokenController : Controller
    {
        private OAuthProvider OAuthProvider { get; }

        public TokenController(OAuthProvider oAuthProvider)
        {
            OAuthProvider = oAuthProvider;
        }

        public ResponseData Token([FromBody] JWTRequest parameters)
        {
            if (parameters == null)
            {
                return new ResponseData
                {
                    Message = "null of parameters"
                };
            }

            switch (parameters.grant_type)
            {
                case "password":
                {
                    return OAuthProvider.DoPassword(parameters);
                }
                case "refresh_token":
                {
                    return OAuthProvider.DoRefreshToken(parameters);
                }
                default:
                    return new ResponseData
                    {
                        Message = "bad request"
                    };
            }
        }
    }
}
