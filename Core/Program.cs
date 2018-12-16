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
        public DiscordSocketClient client_;
        public CommandHandler handler_;

        static void Main(string[] args)
        => new Program().StartMainSync().GetAwaiter().GetResult();

        public async Task StartMainSync() {

            //Init
            if (Config.bot.token != "" && Config.bot.token != null) {

                //Init REST socket
                client_ = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
                client_.Log += Client_Logging;
                
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

        //Write to the console any log event
        private async Task Client_Logging(LogMessage msg)
        {
            Console.WriteLine("Client Socket: " + msg.Message);
            
        }
    }
}
