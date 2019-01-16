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
    public abstract class CommandBase : ModuleBase<SocketCommandContext>
    {
        //Gets the user's full name or nickname
        protected string GetName()
        {
            var user = Context.User as IGuildUser;
            string username = Context.User.Username;
            if (user.Nickname != "" && user.Nickname != null)
            {
                username = user.Nickname;
            }
            return username;
        }

        //Returns a changed ref of a string, returns flase if brackets did not exist
        protected bool RemoveTopicBracket(ref string name)
        {
            bool hasRemoved = false;
            int index = name.IndexOf("(");
            if (index != -1)
            {
                name = name.Remove(index - 1);
                hasRemoved = true;
            }
            return hasRemoved;
        }

        protected string RemoveChannelNameFirstWord(string s, ulong serverId) {
            Config.serverData[serverId].firstWordTitle_ = s.Substring(0, s.IndexOf(' ') + 1);
            return s.Substring(s.IndexOf(' ') + 1);
        }
    }
}
