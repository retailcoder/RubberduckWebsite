using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Rubberduck.Client
{
    public abstract class ApiClientBase
    {
        private readonly ILogger _logger;
        private readonly string _baseUrl;
        private readonly TimeSpan _timeout;

        private static readonly ProductInfoHeaderValue UserAgent =
            new("Rubberduck.Client", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());


        protected ApiClientBase(ILogger<ApiClientBase> logger, IConfiguration configuration)
        {
            _logger = logger;
            _baseUrl = configuration.GetSection("API")["BaseUrl"];
            if (int.TryParse(configuration.GetSection("API")["TimeoutSeconds"], out var timeoutSeconds))
            {
                _timeout = TimeSpan.FromSeconds(timeoutSeconds);
            }
        }

        protected string BaseUrl => _baseUrl;

        protected virtual HttpClient GetClient()
        {
            var client = new HttpClient { Timeout = _timeout };
            client.DefaultRequestHeaders.UserAgent.Add(UserAgent);
            return client;
        }

        protected virtual async Task<TResult> GetResponse<TResult>(string route)
        {
            var uri = new Uri($"{_baseUrl}{route}");
            try
            {
                using (var client = GetClient())
                using (var response = await client.GetAsync(uri))
                {
                    response.EnsureSuccessStatusCode();
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        return (TResult)await JsonSerializer.DeserializeAsync(stream, typeof(TResult));
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "This exception will be wrapped into a new ApiException.");
                throw new ApiException(exception);
            }
        }

        protected virtual async Task<T> Post<T>(string route, T args) => await Post<T, T>(route, args);

        protected virtual async Task<TResult> Post<TArgs, TResult>(string route, TArgs args)
        {
            var uri = new Uri($"{_baseUrl}{route}");
            string json;
            try
            {
                json = JsonSerializer.Serialize(args);
            }
            catch (Exception exception)
            {
                throw new ArgumentException("The specified arguments could not be serialized.", exception);
            }

            try
            {
                using (var client = GetClient())
                using (var response = await client.PostAsync(uri, new StringContent(json)))
                {
                    response.EnsureSuccessStatusCode();
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        return (TResult)await JsonSerializer.DeserializeAsync(stream, typeof(TResult));
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "This exception will be wrapped into a new ApiException.");
                throw new ApiException(exception);
            }
        }
    }
}
