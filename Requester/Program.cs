using System;
using System.Threading;
using System.Threading.Tasks;

using Shared.Services;

namespace Requester
{
    class Program
    {
        private const string CONFIG_FILE_PATH = "Resources/Configs.xml";
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            ConfigurationService configurationService = new ConfigurationService(CONFIG_FILE_PATH);
            BackendService backendService = new BackendService(configurationService);

            Console.WriteLine("Requester online");

            var lastUpdatedPage = backendService.GetLastUpdatedPage().Result;
            //var lastCreatedPage = BackendService.GetLastCreatedPage().Result;

            while (true)
            {
                var updated = backendService.GetLastUpdatedPage().Result;
                var created = backendService.GetLastCreatedPage().Result;

                if (lastUpdatedPage != null || updated != null)
                {
                    if (lastUpdatedPage.TimeStamp.Value.CompareTo(updated.TimeStamp.Value) < 0 ||
                        string.CompareOrdinal(lastUpdatedPage.Title, updated.Title) != 0)
                    {
                        lastUpdatedPage = updated;

                        Console.WriteLine("New page has been updated. Sending request.");

                        backendService.SendMessage(lastUpdatedPage);
                    }
                }

                //if (lastCreatedPage != null || created != null)
                //{
                //    if (lastCreatedPage.TimeStamp.Value.CompareTo(created.TimeStamp.Value) < 0 ||
                //        string.CompareOrdinal(lastCreatedPage.Title, created.Title) != 0)
                //    {
                //        lastCreatedPage = created;

                //        Console.WriteLine("New page has been updated. Sending request.");

                //       // backendService.SendMessage(lastCreatedPage);
                //    }
                //}

                Thread.Sleep(10000);
            }
        }
    }
}
