using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using PlatformService.Dtos;

namespace PlatformService.AsyncDataSerices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        // http client factory 
        public HttpCommandDataClient(HttpClient httpClient, IConfiguration config )
        {
            _config = config;
            _httpClient = httpClient;
        }
        public async Task SendPlatformToCommand(PlatformReadDto plat)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync
                ($"{_config["CommandService"]}/api/c/platforms", httpContent);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Sync Post to commandService was ok");
            }
            else 
            {
                Console.WriteLine("failed");
            }
        }
    }
}