using DiscordButlerBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;

namespace DiscordButlerBot.Commands
{
    public class MiscCommands : CommandBase
    {

        //Owner Only commands----------------------------------------------------------
        [Command("welcome")]
        [RequireOwner]
        public async Task Welcome() {

            await Context.Channel.SendMessageAsync("**Greetings, glad to be of service !~~~ ** ");

            foreach (var role in Context.Guild.Roles) {
                if (!(role.Name.Contains("everyone") || role.Name.Contains("Basement")))
                {
                    await Context.Channel.SendMessageAsync(role.Mention);
                }

            }
            
        }
        [Command("setTopicLength")]
        [RequireOwner]
        public async Task SetTopicLength(string num = "") {
            int newLength = 0;
            var guildUser = Context.User as IGuildUser;
            if (Int32.TryParse(num, out newLength))
            {
                Config.serverData[guildUser.GuildId].maxTopicNameLength = newLength;
                Config.SaveServerData();
                await Context.Channel.SendMessageAsync(String.Format("I have set the topic length to {0} master.", Config.serverData[guildUser.GuildId].maxTopicNameLength));
            }
            else {
                await Context.Channel.SendMessageAsync("Could not set topic new length Parse failed");
            }

        }

        [Command("addThisVChannel")]
        [RequireOwner]
        public async Task AddChannel() {
            var guildUser = Context.User as IGuildUser;
            var voiceChannelUserIn = guildUser.VoiceChannel;
            if (voiceChannelUserIn != null)
            {
                if (Config.serverData[guildUser.GuildId].voiceChannelIds_.Contains(voiceChannelUserIn.Id))
                {
                    await Context.Channel.SendMessageAsync("This channel is already in my list master");
                }
                else {
                    Config.serverData[guildUser.GuildId].voiceChannelIds_.Add(voiceChannelUserIn.Id);
                    await Context.Channel.SendMessageAsync("I have added " + voiceChannelUserIn.Name + " voice channel into my list");
                    Config.SaveServerData();                    
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
            var guildUser = Context.User as IGuildUser;
            var voiceChannelUserIn = guildUser.VoiceChannel;
            if (voiceChannelUserIn != null)
            {
                if (Config.serverData[guildUser.GuildId].voiceChannelIds_.Contains(voiceChannelUserIn.Id))
                {
                    Config.serverData[guildUser.GuildId].voiceChannelIds_.Remove(voiceChannelUserIn.Id);
                    await Context.Channel.SendMessageAsync("I have removed " + voiceChannelUserIn.Name + " from my list");
                    Config.SaveServerData();
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
        //------------------------------------------------------------------


        [Command("commands")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task Commands() {
            string msg = 
                "\n These are your commands master " + Context.User.Mention + " : \n" +
                "```" +
                "!hi - Say hi.\n\n" +                
                "!getdrink (name) - Will give you the drink of your liking *wink*\n\n" +
                "!maketeams # - Orgainzes a team from voice chat, and can escort teams to different voice channels.\n\n" +
                "!vctopic (topic) - Adds a topic to the voice channel your currently in.\n\n" +
                "!rmvctopic - Removes the topic to the voice channel your currently in.\n\n" +
                "!listvoice - list all users in your current voice channel." +
                "```";
            await Context.Channel.SendMessageAsync(msg);
        }
        [Command("hi")]
        public async Task Speak()
        {
            var user = Context.User as IGuildUser;
            string username = Context.User.Username;

            await Context.Channel.SendMessageAsync(String.Format("Greetings master {0}.", GetName() + " <3"));

        }
        [Command("getDrink")]
        public async Task AskDrink([Remainder]string drink) {
            if (drink != "" && drink != null) {
                await Context.Channel.SendMessageAsync(String.Format("*Hands you a {0}*", drink));
            }
            else {
                await Context.Channel.SendMessageAsync(String.Format("Master {0}, what drink would you like? (!getdrink (name)) ", GetName()));
            }
            
        }
        [Command("lewd")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task BadTimes()
        {
            var user = Context.User as IGuildUser;

            bool hasNickName = user.Nickname == null ? false : true;
            bool hasName = false;
            string username = user.Username.ToLower();

            if (hasNickName) {
                string nickname = user.Nickname.ToLower();
                hasName = nickname.Contains("nemu");
            }
                      
            if (!(username.Contains("nemu") || hasName))
            {
                await Context.Channel.SendMessageAsync("BAKA! *slaps you* ");
            }
            else {
                await Context.Channel.SendMessageAsync(String.Format("AAAAHH~!! YES SENPAIIII <3~~~ {0}", Context.User.Mention));
            }
            
        }
        [Command("vctopic")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task SetVCTopic([Remainder] string newTopic = "") {
            
            var guildUser = Context.User as IGuildUser;
            var voiceChannelUserIn = guildUser.VoiceChannel;
            if (voiceChannelUserIn != null)
            {
                if (newTopic != null && newTopic != "")
                {
                    if (newTopic.Length <= Config.serverData[guildUser.GuildId].maxTopicNameLength)
                    {
                        string vcName = voiceChannelUserIn.Name;
                        string oldvcName = RemoveTopicBracket(ref vcName) == false ? voiceChannelUserIn.Name : vcName;

                        vcName += String.Format(" ({0})", newTopic);
                        await guildUser.VoiceChannel.ModifyAsync(x =>
                        {
                            x.Name = vcName;
                        });
                        await Context.Channel.SendMessageAsync(String.Format("Master {0}, topic is now set to \"{1}\" in {2}", guildUser.Mention, newTopic, oldvcName));
                    }
                    else {
                        await Context.Channel.SendMessageAsync(String.Format("Sorry master {0}, the topic name is too long it needs to be less than {1} characters.", guildUser.Mention, Config.serverData[guildUser.GuildId].maxTopicNameLength));
                    }
                }
                else
                {
                    await ClearVCTopic();
                }
            }
            else {
                await Context.Channel.SendMessageAsync(String.Format("Master {0}, you need to be in a voice channel to set a topic!", guildUser.Mention));
            }
        }
        [Command("rmvctopic")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task ClearVCTopic(){

            var callingUser = Context.User as IGuildUser;
            var voiceChannelUserIn = callingUser.VoiceChannel;
            string vcName = voiceChannelUserIn.Name;
            if (voiceChannelUserIn != null)
            {
                if (RemoveTopicBracket(ref vcName))
                {
                    await callingUser.VoiceChannel.ModifyAsync(x =>
                    {
                        x.Name = vcName;
                    });
                    await Context.Channel.SendMessageAsync( String.Format("Master {0}, topic was removed in {1}", callingUser.Mention, vcName));
                }
                else
                {
                    await Context.Channel.SendMessageAsync("Master, no topic was set in " + vcName);
                }
            }
            else {
                await Context.Channel.SendMessageAsync(String.Format("Master {0}, you need to be in a voice channel to clear a topic", callingUser.Mention));
            }
        }
        //Returns a changed ref of a string, returns flase if brackets did not exist
        bool RemoveTopicBracket(ref string name) {
            bool hasRemoved = false;
            int index = name.IndexOf("(");
            if (index != -1)
            {
                name = name.Remove(index - 1);
                hasRemoved = true;
            }
            return hasRemoved;
        }
        [Command("listvoice")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task ListVoice()
        {
            var guildUser = Context.User as SocketGuildUser;
            var voiceChannelUserIn = guildUser.VoiceChannel;

            if (guildUser.VoiceChannel != null)
            {
                string listingReplyMsg = "";

                var users = voiceChannelUserIn.Users;
                string vcName = voiceChannelUserIn.Name;
                RemoveTopicBracket(ref vcName);
                EmbedBuilder em = new EmbedBuilder();
                em.WithTitle("Members in Voice " + vcName);
                em.WithColor(new Color(10, 10, 10));
                em.WithFooter("");
                foreach (var user in users)
                {

                    listingReplyMsg += "-" + user.Mention;
                    listingReplyMsg += "\n";
                }
                em.WithDescription(listingReplyMsg);
                await Context.Channel.SendMessageAsync("Master " + guildUser.Mention + ", here is your list: \n", false, em);
            }
            else {
                await Context.Channel.SendMessageAsync(String.Format("Sorry master {0}, you need to be in a voice channel to use this command.", Context.User.Mention));
            }

        }
    }
}
