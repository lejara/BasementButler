using DiscordButlerBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using System.Diagnostics; // Process
using System.IO; // StreamWriter

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

        protected string Run_Python(string scriptpy, string arg = "")
        {
            string output;

            Process p = new Process(); // create process (i.e., the python program
            p.StartInfo.FileName = "python.exe";
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.UseShellExecute = false; // make sure we can read the output from stdout
            p.StartInfo.Arguments = scriptpy + " \"" + arg + "\" "; // start the python program with one parameters
            p.Start(); // start the process (the python program)
            StreamReader s = p.StandardOutput;
            output = s.ReadToEnd();
            //string[] r = output.Split(new char[] { ' ' }); // get the parameter
            Console.WriteLine(output);
            p.WaitForExit();



            return output;
        }


    }
}
