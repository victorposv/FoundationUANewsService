using Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace Shared.Services
{
    public class TelegramService
    {
        private const string TELEGRAM_BOT_TOKEN = "1032744107:AAGidmwQzRH-kHGLZDemCLLnZutk6rDM5pE";
        private const string LOGO_PATH = "Resources/logo.jpg";

        private TelegramBotClient client;

        private string channelName;

        public TelegramService(string channelName)
        {
            client = new TelegramBotClient(TELEGRAM_BOT_TOKEN);
            this.channelName = channelName;
        }

        public async Task PostNewsAsync(TelegramData newPage)
        {
            StringBuilder builder = new StringBuilder();
            InputOnlineFile inputOnlineFile;

            builder.AppendLine("Новий об'єкт на сайті!");
            builder.AppendLine($"Автор: {newPage.AuthorName}");
            builder.AppendLine($"Назва: {newPage.Title}");
            builder.AppendLine(newPage.Url);

            if (System.IO.File.Exists(LOGO_PATH))
            {
                using (FileStream fs = System.IO.File.OpenRead(LOGO_PATH))
                {
                    inputOnlineFile = new InputOnlineFile(fs, "logo.jpg");

                    await client.SendPhotoAsync(channelName, inputOnlineFile, builder.ToString());
                }
            }
            else
            {
                await client.SendTextMessageAsync(channelName, builder.ToString());
            }
        }

        public TelegramData BuildTelegramDataObject(RecentlyCreatedPage newPage)
        {
            return new TelegramData()
            {
                Title = newPage.Title,
                AuthorName = newPage.Author.Name,
                TimeStamp = newPage.CreationTime,
                Url = newPage.PageUrl
            };
        }
    }
}
