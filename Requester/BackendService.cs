using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Shared.Models;
using Shared.Services;

namespace Requester
{
    public class BackendService
    {
        private const string NEW_CREATED_PAGES= "http://newsbotparser.azurewebsites.net/api/newpages";
        private const string NEW_EDITED_PAGES = "http://newsbotparser.azurewebsites.net/api/news";

        private const string RECENTLY_CREATED_PAGES_KEY = "RecentlyCreatedURL";
        private const string NEW_EDITED_PAGES_KEY = "LastUpdatesURL";

        private static HttpClient client;
        private static ConfigurationService configuration;

        public BackendService(ConfigurationService configurationService)
        {
            client = new HttpClient();
            configuration = configurationService;
        }

        public async Task<TelegramData> GetLastCreatedPageAsync()
        {
            string requestContent = string.Empty;
            TelegramData result = null;

            HttpResponseMessage response = await client.GetAsync(configuration.GetConfig(RECENTLY_CREATED_PAGES_KEY));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                requestContent = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(requestContent))
                    result = JsonConvert.DeserializeObject<TelegramData>(requestContent);
                else
                    Console.WriteLine("GetLastCreatedPage: Empty responce");
            }
            else
                Console.WriteLine($"GetLastUpdatedPage: Connection to API failed with {response.StatusCode} code");

            response.Dispose();

            return result;
        }

        public async Task<Embed> GetLastUpdatedPageAsync()
        {
            string requestContent = string.Empty; 
            Embed result = null;

            HttpResponseMessage response = await client.GetAsync(configuration.GetConfig(NEW_EDITED_PAGES_KEY));

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                requestContent = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(requestContent))
                    result = JsonConvert.DeserializeObject<Embed>(requestContent);
                else
                    Console.WriteLine("GetLastUpdatedPage: Empty responce");
            }
            else
                Console.WriteLine($"GetLastUpdatedPage: Connection to API failed with {response.StatusCode} code");

            response.Dispose();

            return result;
        }

        /// <summary>
        /// Send message to Discord bot
        /// </summary>
        /// <param name="info"></param>
        public async Task SendMessageAsync(Embed info)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(configuration.GetConfig(NEW_EDITED_PAGES_KEY), stringContent);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                Console.WriteLine("DiscordSendMessage: Message received");
            else
                Console.WriteLine($"DiscordSendMessage: Connection to API failed with {response.StatusCode} code");

            response.Dispose();
        }

        /// <summary>
        /// Send message to Telegram bot
        /// </summary>
        /// <param name="info"></param>
        public async Task SendMessageAsync(TelegramData info)
        {
            var stringContent = new StringContent(JsonConvert.SerializeObject(info), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(configuration.GetConfig(RECENTLY_CREATED_PAGES_KEY), stringContent);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                Console.WriteLine("TelegramSendMessage: Message received");
            else
                Console.WriteLine($"TelegramSendMessage: Connection to API failed with {response.StatusCode} code");

            response.Dispose();
        }
    }
}
