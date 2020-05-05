using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Models;
using Microsoft.Extensions.Logging;

namespace NewsBotBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        const string CONFIG_FILE_PATH = "Resources/Configs.xml";
        const string WEBHOOK_TOKEN_KEY = "DiscordWebhookUrl";

        private ParseService parseService;
        private WebhookService webhookService;
        private ConfigurationService configurationService;

        private readonly ILogger logger;

        public NewsController(ILogger<NewsController> logger)
        {
            this.logger = logger;

            configurationService = new ConfigurationService(CONFIG_FILE_PATH);

            parseService = new ParseService();
            webhookService = new WebhookService(configurationService.GetConfig(WEBHOOK_TOKEN_KEY));
        }


        // GET: api/News
        [HttpGet]
        public Embed Get()
        {
            Console.WriteLine("Sending latest updated page");
            return webhookService.BuildEmbed(parseService.GetLatestEdited());
        }

        // POST: api/News
        [HttpPost]
        public async void Post([FromBody] Embed value)
        {
            List<Embed> embedCollection = new List<Embed>();
            embedCollection.Add(value);
            
            Console.WriteLine("Updating Discord");

            await webhookService.Send(embedCollection);
        }
    }
}
