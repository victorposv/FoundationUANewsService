using Newtonsoft.Json;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class WebhookService
    {
        #region Members
        private HttpClient httpClient;
        private string webhookUrl;
        private WebhookItem webhookItem;
        #endregion

        #region Json
        [JsonProperty("content")]
        public string Content { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("avatar_url")]
        public string AvatarUrl { get; set; }
        // ReSharper disable once InconsistentNaming
        [JsonProperty("tts")]
        public bool IsTTS { get; set; }
        [JsonProperty("embeds")]
        public List<Embed> Embeds { get; set; } = new List<Embed>();
        #endregion

        #region C-tors
        public WebhookService(string webhookUrl)
        {
            webhookItem = new WebhookItem();
            httpClient = new HttpClient();
            this.webhookUrl = webhookUrl;
        }

        public WebhookService(ulong id, string token): this($"https://discordapp.com/api/webhooks/{id}/{token}")
        {
        }
        #endregion

        public async Task<HttpResponseMessage> Send(string content, string username = null, string avatarUrl = null, bool isTTS = false, List<Embed> embeds = null)
        {
            webhookItem.Content = content;
            webhookItem.Username = username;
            webhookItem.AvatarUrl = avatarUrl;
            webhookItem.IsTTS = isTTS;
            webhookItem.Embeds.Clear();

            if (embeds != null)
            {
                webhookItem.Embeds.AddRange(embeds);
            }

            var stringContent = new StringContent(JsonConvert.SerializeObject(webhookItem), Encoding.UTF8, "application/json");

            return await httpClient.PostAsync(webhookUrl, stringContent);
        }

        public async Task<HttpResponseMessage> Send(UpdatesPage latestPage)
        {
            return await Send(BuildEmbedCollection(latestPage));
        }

        public async Task<HttpResponseMessage> Send(List<Embed> embedInfo)
        {
            return await Send(string.Empty, null, null, false, embedInfo);
        }

        public List<Embed> BuildEmbedCollection(List<UpdatesPage> embedData)
        {
            List<Embed> output = new List<Embed>();
         
            foreach (var item in embedData as List<UpdatesPage>)
            {
                output.Add(BuildEmbed(item));
            }

            return output;
        }

        public List<Embed> BuildEmbedCollection(UpdatesPage embedData)
        {
            List<Embed> output = new List<Embed>();
            output.Add(BuildEmbed(embedData));
            return output;
        }

        public Embed BuildEmbed(UpdatesPage pageInfo)
        {
            EmbedAuthor author = new EmbedAuthor() { Name = pageInfo.Author.Name, Url = pageInfo.Author.Url };
            Embed embed = new Embed() { AuthorName = author.Name, AuthorUrl = author.Url, Title = pageInfo.Title, Url = pageInfo.PageUrl, TimeStamp = pageInfo.UpdateTime };
            return embed;
        }
    }
}
