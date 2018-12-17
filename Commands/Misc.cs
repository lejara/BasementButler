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


        [Command("welcome")]
        [RequireOwner]
        public async Task Welcome() {

            await Context.Channel.SendMessageAsync("**Greetings, glad to be of service**");
            
        }
        [Command("addThisVChannel")]
        [RequireOwner]
        public async Task AddChannel() {
            var callingUser = Context.User as IGuildUser;
            var voiceChannelUserIn = callingUser.VoiceChannel;
            if (voiceChannelUserIn != null)
            {
                if (Config.voiceChannelIds.Contains(voiceChannelUserIn.Id))
                {
                    await Context.Channel.SendMessageAsync("This channel is already in my list master");                    
                }
                else {
                    Config.voiceChannelIds.Add(voiceChannelUserIn.Id);
                    await Context.Channel.SendMessageAsync("I have added "+ voiceChannelUserIn.Name + " voice channel into my list");
                    Config.SaveChannelIds();
                }                
            }
            else {
                await Context.Channel.SendMessageAsync("Sorry master you need to be in a voice channel for this work");
            }
        }
        [Command("removeThisVChannel")]
        [RequireOwner]
        public async Task RemoveChannel()
        {
            var callingUser = Context.User as IGuildUser;
            var voiceChannelUserIn = callingUser.VoiceChannel;
            if (voiceChannelUserIn != null)
            {
                if (Config.voiceChannelIds.Contains(voiceChannelUserIn.Id))
                {
                    Config.voiceChannelIds.Remove(voiceChannelUserIn.Id);
                    await Context.Channel.SendMessageAsync("I have removed " + voiceChannelUserIn.Name + " from my list");
                    Config.SaveChannelIds();
                }
                else
                {                    
                    await Context.Channel.SendMessageAsync("Master the voice your in is already not in my list");                    
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync("Sorry master you need to be in a voice channel for this work");
            }
        }

        [Command("hi")]
        public async Task Speak()
        {
            var user = Context.User as IGuildUser;
            string username = Context.User.Username;
            if (user.Nickname != "" && user.Nickname != null)
            {
                await Context.Channel.SendMessageAsync(String.Format("Greetings master {0}.", user.Nickname));
            }
            else
            {
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
