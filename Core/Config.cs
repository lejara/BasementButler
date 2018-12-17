using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using DiscordButlerBot.Commands.CommandCompoments;

namespace DiscordButlerBot.Core
{
    public struct BotInfo {
        public string token;
        public string cmdPrefix;
        public ulong serverID;
    }

    class Config
    {
        private const string configFolder = "ConfigCore";
        private const string configFile = "BotConfig.json";
        private const string channelIdFile = "VoiceChannelIds.json";
        private readonly static string configPath = configFolder + '/' + configFile;
        private readonly static string channelIdPath = configFolder + '/' + channelIdFile;

        public static List<ulong> voiceChannelIds;

        public static TeamMaker teamMakerInfo;

        public static BotInfo bot;

        //Load the BotConfig. creates if it does not exist
        static Config() {

            teamMakerInfo = new TeamMaker();

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
                string json = File.ReadAllText(configPath);
                bot = JsonConvert.DeserializeObject<BotInfo>(json);

            }

            //Load or create channel ids file
            if (!Directory.Exists(channelIdPath))
            {
                voiceChannelIds = new List<ulong>();
                string json = JsonConvert.SerializeObject(voiceChannelIds, Formatting.Indented);
                File.WriteAllText(channelIdPath, json);
            }
            else {
                string json = File.ReadAllText(channelIdPath);
                voiceChannelIds = JsonConvert.DeserializeObject<List<ulong>>(json);
            }

        }

    }
}
