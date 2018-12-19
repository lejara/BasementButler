﻿using DiscordButlerBot.Core;
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
                    await Context.Channel.SendMessageAsync("I have added " + voiceChannelUserIn.Name + " voice channel into my list");
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
        //------------------------------------------------------------------
        [Command("commands")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task Commands() {
            string msg = "These are your commands master " + GetName() + ":\n" +
                "- !hi - say hi.\n" +
                "- !listvoice - tells you who is in your voice channel.\n" +
                "- !getdrink (name) - Will give you the drink of your liking *wink*\n" +
                "- !maketeams # - orgainzes a team in vc, and can escort teams to different voice channels.\n" +
                "- !setvctopic (name) - adds a topic to the voice channel your currently in.\n" +
                "- !clearvctopic - removes the topic to the voice channel your currently in.";
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
            await Context.Channel.SendMessageAsync("BAKA! *slaps you*");
        }
        [Command("setvctopic")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task SetVCTopic([Remainder] string newTopic = "") {
            
            var callingUser = Context.User as IGuildUser;
            var voiceChannelUserIn = callingUser.VoiceChannel;
            if (voiceChannelUserIn != null)
            {
                if (newTopic != null || newTopic == "")
                {

                    string vcName = voiceChannelUserIn.Name;
                    int index = vcName.IndexOf("(");
                    if (index != -1)
                    {
                        vcName = vcName.Remove(index - 1);
                    }
                    vcName += String.Format(" ({0})", newTopic);
                    await callingUser.VoiceChannel.ModifyAsync(x =>
                    {
                        x.Name = vcName;
                    });
                    await Context.Channel.SendMessageAsync(String.Format("Master {0}, topic is now set to {1}", GetName(), newTopic));

                }
                else
                {
                    await ClearVCTopic();
                }
            }
            else {

            }
        }
        [Command("clearvctopic")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task ClearVCTopic(){

            var callingUser = Context.User as IGuildUser;
            var voiceChannelUserIn = callingUser.VoiceChannel;
            string vcName = voiceChannelUserIn.Name;
            int index = vcName.IndexOf("(");
            if (index != -1)
            {
                vcName = vcName.Remove(index - 1);
                await callingUser.VoiceChannel.ModifyAsync(x =>
                {
                    x.Name = vcName;
                });
                await Context.Channel.SendMessageAsync("VC topic was removed Master " + GetName());
            }
            else {
                await Context.Channel.SendMessageAsync("Master, no topic was set here ");
            }
        }
        [Command("listvoice")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task ListVoice()
        {
            var callingUser = Context.User as IGuildUser;
            var guild = Context.Client.Guilds.FirstOrDefault(g => g.Id == Config.bot.serverID);
            var voiceChannelUserIn = guild.Channels.FirstOrDefault(c => c.Id == callingUser.VoiceChannel.Id);

            string listingReplyMsg = "Master " + GetName() + ", the people in voice " + voiceChannelUserIn.Name + " are:\n";

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
