using DiscordButlerBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DiscordButlerBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test: " + Config.bot.token + "prefix: " + Config.bot.cmdPrefix);
            Console.ReadLine();
            
        }
    }
}
