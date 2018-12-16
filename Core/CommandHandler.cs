using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace DiscordButlerBot.Core
{
    class CommandHandler {

        DiscordSocketClient client_;
        CommandService service_;
        //position when the prefix end and the command begins
        int argPos = 0;

        public async Task InitAsyncTask(DiscordSocketClient client) {

            client_ = client;
            service_ = new CommandService();
            //load the modules
            await service_.AddModulesAsync(Assembly.GetEntryAssembly());
            
            client_.MessageReceived += HandleCommandAsync;

        }

        //Handling the recieving of a message 
        private async Task HandleCommandAsync(SocketMessage s)
        {
            var userMsg = s as SocketUserMessage;

            if (userMsg != null) {
                Console.WriteLine("Messaged Recevied");
                //info about the client
                var context = new SocketCommandContext(client_, userMsg);

                //If bot is mentioned
                if (userMsg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos) || userMsg.HasMentionPrefix(client_.CurrentUser, ref argPos))
                {

                    var result = await service_.ExecuteAsync(context, argPos);
                    if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    {
                        Console.WriteLine("Problem HandleCommandAsync: " + result.ErrorReason);
                    }
                    else
                    {
                        Console.WriteLine("Mentioned Received");
                    }
                }
            }
            
        }
    }
}
