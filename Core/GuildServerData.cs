using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using DiscordButlerBot.Commands.CommandCompoments;
using Discord.WebSocket;

namespace DiscordButlerBot.Core
{
    class GuildServerData
    {
        
        public string serverName_;
        public int maxTopicNameLength;
        public SocketGuild discordServer_;
        public static List<ulong> voiceChannelIds_;
        public static TeamMaker teamMakerInfo_;

        public GuildServerData(ref SocketGuild g) {
            discordServer_ = g;
            serverName_ = g.Name;
        }
    }
}
