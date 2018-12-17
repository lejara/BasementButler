using DiscordButlerBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;

namespace DiscordButlerBot.Commands
{
    public class Misc : ModuleBase<SocketCommandContext>
    {

        [Command("hi")]
        public async Task Speak()
        {
            var user = Context.User as IGuildUser;
            string username = Context.User.Username;
            if (user.Nickname != "" && user.Nickname != null){
                await Context.Channel.SendMessageAsync(String.Format("Greetings master {0}.", user.Nickname));
            }
            else {
                await Context.Channel.SendMessageAsync(String.Format("Greetings master {0}.:", username));
            }
                
        }
        [Command("listvoice")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task ListVoice()
        {
            var callingUser = Context.User as IGuildUser;
            var guild = Context.Client.Guilds.FirstOrDefault(g => g.Id == Config.bot.serverID);
            var voiceChannelUserIn = guild.Channels.FirstOrDefault(c => c.Id == callingUser.VoiceChannel.Id);

            string listingReplyMsg = "Master " + callingUser.Username + ", the people in voice " + voiceChannelUserIn.Name + " are:\n";

            var users = voiceChannelUserIn.Users;
            foreach (var user in users)
            {

                var gUser = user as IGuildUser;

                listingReplyMsg += "-" + user.Username;
                if (user.Nickname != "" && user.Nickname != null)
                {
                    listingReplyMsg += " ( " + user.Nickname + " )" + "\n";
                }
                else
                {
                    listingReplyMsg += "\n";
                }
            }
            await Context.Channel.SendMessageAsync(listingReplyMsg);

        }
    }
}
