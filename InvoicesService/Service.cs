using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using InvoicesService.Helpers.Serializers;
using InvoicesService.Models;

namespace InvoicesService
{
    public static class Service
    {
        public static Models.Settings Settings;

        static Service()
        {
            Settings= new Models.Settings();
            LoadSettings();
        }

        public static void LoadSettings()
        {
            var path = Directory.GetCurrentDirectory() + "\\settings";
            if (File.Exists(path))
            {
                Settings = BinarySerializer<Models.Settings>.Deserialize(path);
            }
        }

        public static void SaveSettings()
        {
            var path = Directory.GetCurrentDirectory() + "\\settings";
            BinarySerializer<Models.Settings>.Serialize(path, Settings);
        }
    }
}
