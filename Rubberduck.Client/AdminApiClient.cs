using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Rubberduck.Client.Abstract;
using Rubberduck.Model.Entities;

namespace Rubberduck.Client
{

    public class AdminApiClient : PublicApiClient, IAdminApiClient
    {
        public AdminApiClient(ILogger<AdminApiClient> logger, IConfiguration configuration) 
            : base(logger, configuration)
        {

        }

        //private static string GenerateJwtToken(ClaimsIdentity user, int expireMinutes = 20)
        //{
        //    var claimToken = user.Claims.SingleOrDefault(e => e.Type == "access_token")?.Value;
        //    var claimTokenBytes = Encoding.UTF8.GetBytes(claimToken);

        //    var now = DateTime.UtcNow;
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = user,

        //        Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),
                
        //        SigningCredentials = new SigningCredentials(
        //            new SymmetricSecurityKey(claimTokenBytes),
        //            SecurityAlgorithms.HmacSha256Signature)
        //    };

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var stoken = tokenHandler.CreateToken(tokenDescriptor);
        //    var result = tokenHandler.WriteToken(stoken);

        //    return result;
        //}

        protected override HttpClient GetClient(string contentType = ContentTypeApplicationJson)
        {
            //if (_token is null)
            //{
            //    throw new UnauthorizedAccessException();
            //}

            var client = base.GetClient();
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JTW", _token);
            if (contentType != null)
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));
            }
            return client;
        }

        public async Task<Feature> SaveFeatureAsync(Feature dto)
        {
            var endpoint = "Admin/SaveFeature";

            try
            {
                return await Post(endpoint, dto);
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<Feature> DeleteFeatureAsync(Feature dto)
        {
            var endpoint = "Admin/DeleteFeature";

            try
            {
                return await Post(endpoint, dto);
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<FeatureItem> SaveFeatureItemAsync(FeatureItem dto)
        {
            var endpoint = "Admin/SaveFeatureItem";

            try
            {
                return await Post(endpoint, dto);
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<FeatureItem> DeleteFeatureItemAsync(FeatureItem dto)
        {
            var endpoint = "Admin/DeleteFeatureItem";

            try
            {
                return await Post(endpoint, dto);
            }
            catch (ApiException)
            {
                return null;
            }
        }

        public async Task<bool> UpdateTagMetadataAsync()
        {
            var endpoint = "Admin/Update";

            var uri = new Uri($"{BaseUrl}{endpoint}");
            try
            {
                using (var client = GetClient())
                {
                    client.Timeout = base.PostRequestTimeout;
                    using (var response = await client.PostAsync(uri, null))
                    {
                        response.EnsureSuccessStatusCode();
                        return true;
                    }
                }
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsUpdatingAsync()
        {
            var endpoint = "Admin/IsUpdating";

            var uri = new Uri($"{BaseUrl}{endpoint}");
            try
            {
                using (var client = GetClient())
                {
                    client.Timeout = base.GetRequestTimeout;
                    using (var response = await client.GetAsync(uri))
                    {
                        response.EnsureSuccessStatusCode();
                        var value = await response.Content.ReadAsStringAsync();
                        return bool.Parse(value);
                    }
                }
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch
            {
                return false;
            }
        }
    }
}
