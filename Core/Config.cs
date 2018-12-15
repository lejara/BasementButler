using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace DiscordButlerBot.Core
{
    public struct BotInfo {
        public string token;
        public string cmdPrefix;
    }

    class Config
    {
        private const string configFolder = "ConfigCore";
        private const string configFile = "BotConfig.json";       
        private readonly static string configPath = configFolder + '/' + configFile;

        public static BotInfo bot;

        //Load the BotConfig. creates if it does not exist
        static Config() {

            //Create directory if does not exist
            if (!Directory.Exists(configFolder)) {
                Directory.CreateDirectory(configFolder);
            }
            //Load or Create config file
            if (!File.Exists(configPath))
            {
                bot = new BotInfo();
                string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
                File.WriteAllText(configPath, json);
            }
            else {
                //if it does exist
                string json = File.ReadAllText(configPath);
                bot = JsonConvert.DeserializeObject<BotInfo>(json);

            }

        }

    }
}
