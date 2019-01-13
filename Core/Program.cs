using DiscordButlerBot.Core;
using Discord.Commands;
using Discord.WebSocket;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace DiscordButlerBot
{    
    class Program
    {
        public static double versionMajor = 2;
        public static double versionMinor = 0; 
        public static double versionPatch = 0; 

        public DiscordSocketClient client_;
        public CommandHandler handler_;

        static void Main(string[] args)
        => new Program().StartMainSync().GetAwaiter().GetResult();

        public async Task StartMainSync() {

            //Init
            if (Config.bot.token != "" && Config.bot.token != null) {
                Console.WriteLine(String.Format("DiscordButlerBot Ver: {0}.{1}.{2} ", versionMajor, versionMinor, versionPatch));
                //Init REST socket
                client_ = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
                client_.Log += Client_Logging;
                client_.GuildAvailable += InitJoinedGuild;
                client_.JoinedGuild += InitJoinedGuild;

                // Login/Start
                await client_.LoginAsync(TokenType.Bot, Config.bot.token);
                await client_.StartAsync();

                //Init Command Handler
                handler_ = new CommandHandler();
                await handler_.InitAsyncTask(client_);                
                await Task.Delay(-1);
            }
            return;
            
        }
        private async Task InitJoinedGuild(SocketGuild g)
        {
            if (!Config.serverData.ContainsKey(g.Id))
            {
                Config.serverData.Add(g.Id, new GuildServerData(g));
                Config.SaveServerData();
                Console.Write(String.Format("Added Guild server data set: {0}, {1}\n", g.Id, g.Name));
            }
            else {
                Console.Write(String.Format("Guild server data set exist: {0}, {1}\n", g.Id, g.Name));
            }                                    
        }

        //Write to the console any log event
        private async Task Client_Logging(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            
        }
    }
}
