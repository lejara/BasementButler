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
        public string GetName()
        {
            var user = Context.User as IGuildUser;
            string username = Context.User.Username;
            if (user.Nickname != "" && user.Nickname != null)
            {
                username = user.Nickname;
            }
            return username;
        }
    }
}
