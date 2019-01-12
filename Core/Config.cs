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
        public int maxTopicNameLength; //TODO: remove when mutli is implemented
    }

    class Config
    {        
        private const string configFolder = "ConfigCore";
        private const string configFile = "BotConfig.json";
        private const string channelIdFile = "VoiceChannelIds.json";
        private const string dataFolder = "Data";
        private const string serverDataFile = "ServerData.json";
       
        private readonly static string configPath = configFolder + '/' + configFile;
        private readonly static string channelIdPath = configFolder + '/' + channelIdFile;
        private readonly static string serverDataPath = dataFolder + '/' + serverDataFile;

        public static List<ulong> voiceChannelIds; //TODO: remove when mutli is implemented
        public static TeamMaker teamMakerInfo; //TODO: remove when mutli is implemented
        public static BotInfo bot;
        public static List<GuildServerData> serverData; // setup for multi server support, will use json to store it data.

        //Load the BotConfig. creates if it does not exist
        static Config() {
            //Create directory if does not exist
            if (!Directory.Exists(configFolder)) {
                Directory.CreateDirectory(configFolder);
            }
            //Create Data folder if it does not exist
            if (!Directory.Exists(dataFolder)) {
                Directory.CreateDirectory(dataFolder);
            }

            //Load or Create config file
            if (!File.Exists(configPath))
            {
                bot = new BotInfo();
                SaveConfigFile();
            }
            else {
                string json = File.ReadAllText(configPath);
                bot = JsonConvert.DeserializeObject<BotInfo>(json);

            }

            //Load or create channel ids file TODO: remove when mutli is implemented
            if (!File.Exists(channelIdPath))
            {
                voiceChannelIds = new List<ulong>();
                SaveChannelIds();
            }
            else {
                LoadChannelIds();
            }

            //Load or create server data  file
            if (!File.Exists(serverDataPath))
            {
                serverData = new List<GuildServerData>();
                SaveServerData();
            }
            else
            {
                LoadServerData();
            }

            teamMakerInfo = new TeamMaker();

        }

        public static void LoadChannelIds() { //TODO: remove when mutli is implemented
            string json = File.ReadAllText(channelIdPath);
            voiceChannelIds = JsonConvert.DeserializeObject<List<ulong>>(json);
        }
        public static void SaveChannelIds() //TODO: remove when mutli is implemented
        {            
            string json = JsonConvert.SerializeObject(voiceChannelIds, Formatting.Indented);
            File.WriteAllText(channelIdPath, json);
        }
        public static void SaveConfigFile() {
            string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }
        public static void LoadServerData()
        {
            string json = File.ReadAllText(serverDataPath);
            serverData = JsonConvert.DeserializeObject<List<GuildServerData>>(json);
        }
        public static void SaveServerData()
        {
            string json = JsonConvert.SerializeObject(serverData, Formatting.Indented);
            File.WriteAllText(serverDataPath, json);
        }
    }
}
