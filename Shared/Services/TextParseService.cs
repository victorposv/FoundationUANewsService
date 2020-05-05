using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Services
{
    public static class TextParseService
    {
        private const string BASE_URL = "http://scp-ukrainian.wikidot.com/";

        public static string ParseTitle(string title)
        {
            string quoteSign = "&quot;";
            string quouteReplacement = "\"";

            return title.Replace(quoteSign, quouteReplacement);
        }

        public static string CleanGeneralAuthorName(string name)
        {
            name = name.Replace("&nbsp;", "");
            name = name.Replace("змінено ", "");
            name = name.Replace(" в", "");

            return name;
        }

        public static DateTime GetCorrectDateTime(string dateTime)
        {
            // TODO: setup current time in propper way
            return DateTime.Parse(dateTime);
        }

        public static string BuildUrl(string urlPart)
        {
            return string.Format("{0}{1}", BASE_URL, urlPart);
        }
    }
}
