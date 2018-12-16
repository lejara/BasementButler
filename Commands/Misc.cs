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
        //TODO: use a json implem for setting ids
        const ulong generalTwoID = 523667456133300268;
        const ulong serverID = 523627638527492135;

        [Command("speak")]
        public async Task Speak() {
            string username = Context.User.Username;
            await Context.Channel.SendMessageAsync("Hello master " + username + ".");
        }
        [Command("copy")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task Copy([Remainder] string message)
        {
            string username = Context.User.Username;
            await Context.Channel.SendMessageAsync(username + " said, " + message);
        }
        [Command("list")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task List()
        {
            var callingUser = Context.User as IGuildUser;
            var guild = Context.Client.Guilds.FirstOrDefault(g => g.Id == serverID);
            var voiceChannelUserIn = guild.Channels.FirstOrDefault(c => c.Id == callingUser.VoiceChannel.Id);

            string listingReplyMsg = "Master " + callingUser.Username + ", the people in voice " + voiceChannelUserIn.Name + " are:\n";

            var users = voiceChannelUserIn.Users;
            foreach (var user in users) {

                var gUser = user as IGuildUser;

                listingReplyMsg += "-" + user.Username;
                if (user.Nickname != "" && user.Nickname != null)
                {
                    listingReplyMsg += " ( " + user.Nickname + " )" + "\n";
                }
                else {
                    listingReplyMsg += "\n";
                }

                //Move a user(testing)
                await gUser.ModifyAsync(x =>
                {
                    x.ChannelId = generalTwoID;
                });

            }
            await Context.Channel.SendMessageAsync(listingReplyMsg);


        }
    }
}
