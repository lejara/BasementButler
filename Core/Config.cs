using System;
using System.Collections.Generic;
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
        private const string channelIdFile = "VoiceChannelIds.json";
        private const string dataFolder = "Data";
        private const string serverDataFile = "ServerData.json";
       
        private readonly static string configPath = configFolder + '/' + configFile;
        private readonly static string channelIdPath = configFolder + '/' + channelIdFile;
        private readonly static string serverDataPath = dataFolder + '/' + serverDataFile;

        public static Dictionary<ulong, GuildServerData> serverData;
        public static BotInfo bot;           

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

            //Load or create server data  file
            if (!File.Exists(serverDataPath))
            {
                serverData = new Dictionary<ulong, GuildServerData>();
                SaveServerData();
            }
            else
            {
                LoadServerData();
            }
        }
        public static void SaveConfigFile() {
            string json = JsonConvert.SerializeObject(bot, Formatting.Indented);
            File.WriteAllText(configPath, json);
        }
        public static void LoadServerData()
        {
            string json = File.ReadAllText(serverDataPath);
            serverData = JsonConvert.DeserializeObject<Dictionary<ulong, GuildServerData>>(json);
        }
        public static void SaveServerData()
        {            
            string json = JsonConvert.SerializeObject(serverData, Formatting.Indented);
            File.WriteAllText(serverDataPath, json);
            Console.WriteLine("Saved ServerData");
        }
    }
}

