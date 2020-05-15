using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Models;
using Shared.Services;

namespace NewsBotBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewPagesController : ControllerBase
    {
        const string CONFIG_FILE_PATH = "Resources/Configs.xml";
        const string TELEGRAM_CHANNEL_NAME_KEY = "TelegramChatName";

        private ParseService parseService;
        private TelegramService telegramService;
        private ConfigurationService configurationService;

        private readonly ILogger logger;

        public NewPagesController(ILogger<NewPagesController> logger)
        {
            this.logger = logger;

            configurationService = new ConfigurationService(CONFIG_FILE_PATH);

            parseService = new ParseService();
            telegramService = new TelegramService(configurationService.GetConfig(TELEGRAM_CHANNEL_NAME_KEY));
        }
        // GET: api/NewPages
        [HttpGet]
        public TelegramData Get()
        {
            return telegramService.BuildTelegramDataObject(parseService.GetNewPages().Result);
        }

        // POST: api/NewPages
        [HttpPost]
        public void Post([FromBody] TelegramData value)
        {
            Console.WriteLine("Updating Telegram");

            telegramService.PostNewsAsync(value);
        }
    }
}
