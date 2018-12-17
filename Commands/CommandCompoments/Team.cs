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
            string msg = teamName_ + " : \n";
            foreach (var user in users_) {
                msg += "\t-" + user.Username;
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
    }
}
