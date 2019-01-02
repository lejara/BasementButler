using DiscordButlerBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;


namespace DiscordButlerBot.Commands.CommandCompoments
{
    public class Team 
    {
        int teamID_;
        string teamName_;
        List<IGuildUser> users_;

        public Team(int id, string name) {
            teamID_ = id;
            teamName_ = name;
            users_ = new List<IGuildUser>();
        }

        public void AddUser(IGuildUser user) {
            users_.Add(user);
        }

        //Move each user to the given voice channel id
        public async Task MoveUsers(ulong channelID) {           
            foreach (var user in users_) {
                await user.ModifyAsync(x =>
                {
                    x.ChannelId = channelID;
                });
            }
        }

        //Returns a string of memebers formatted
        public string GetStringMembersFormatted() {
            string msg = String.Format("**{0}** : \n", teamName_);
            foreach (var user in users_) {
                msg += "\t -" + user.Username;
                if (user.Nickname != "" && user.Nickname != null)
                {
                    msg += " ( " + user.Nickname + " )" + "\n";
                }
                else
                {
                    msg += "\n";
                }
            }
            return msg;
        }

        //Returns a embed object of a list of memebers in this team
        public EmbedBuilder GetEmbedMembersFormatted()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle(teamName_);
            embed.WithColor(teamID_*10, teamID_ * 10, teamID_ * 10);
            string list = "";
            foreach (var user in users_)
            {
                list += "\t-" + user.Username;
                if (user.Nickname != "" && user.Nickname != null)
                {
                    list += " ( " + user.Nickname + " )" + "\n";
                }
                else
                {
                    list += "\n";
                }
            }
            return embed;
        }
    }
}
