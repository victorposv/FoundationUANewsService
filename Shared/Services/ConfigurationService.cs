using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Shared.Services
{
    public class ConfigurationService
    {
        private const string KEY_ATTRIBUTE = "key";
        private const string VALUE_ATTRIBUTE = "value";
        private const string KEY_NODE = "add";

        private Dictionary<string, string> configs;
        private readonly string configFilePath;

        public ConfigurationService(string configFilePath)
        {
            configs = new Dictionary<string, string>();
            this.configFilePath = configFilePath;
            ReadConfigs();
        }

        private void ReadConfigs()
        {
            XmlReader reader = XmlReader.Create(configFilePath);

            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        // process a controller element
                        // now get each View
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                if (reader.Name.ToLower() == KEY_NODE)
                                {
                                    string key = string.Empty;
                                    string value = string.Empty;

                                    key = reader.GetAttribute(KEY_ATTRIBUTE);
                                    value = reader.GetAttribute(VALUE_ATTRIBUTE);

                                    configs.Add(key, value);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ReadConfigs: An exception occured during reading configs");
                Console.WriteLine(e.Message);
            }
            finally
            {
                reader.Dispose();
            }
        }

        public string GetConfig(string key)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(key))
                configs.TryGetValue(key, out result);
            else
                Console.WriteLine("GetConfig: No such configuration was found");
            return result;
        }
    }
}
