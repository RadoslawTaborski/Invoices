using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace InvoicesService
{
    public static class ConfigurationManager
    {
        private static RootObject Config
        {
            get
            {
                if (_config == null)
                {
                    ReadConfig("./configuration.json");
                }

                return _config;
            }
        }

        private static RootObject _config;

        public static void ReadConfig(string pathToJson)
        {
            var json = File.ReadAllText(pathToJson);
            _config = JsonConvert.DeserializeObject<RootObject>(json);
        }

        public static string PathToDocuments()
        {
            return Config.Settings.System.PathToDocuments;
        }
    }
}
