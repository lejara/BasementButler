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
    [JsonObject(MemberSerialization.OptIn)]
    class GuildServerData
    {
        [JsonProperty]
        public ulong id_;
        [JsonProperty]
        public string serverName_;
        [JsonProperty]
        public int maxTopicNameLength;
        [JsonProperty]
        public List<ulong> voiceChannelIds_;

        public TeamMaker teamMakerInfo_;

        public GuildServerData() {
            teamMakerInfo_ = new TeamMaker();
        }
        public GuildServerData(SocketGuild g) {
            serverName_ = g.Name;
            id_ = g.Id;
            maxTopicNameLength = 8;
            teamMakerInfo_ = new TeamMaker();
            voiceChannelIds_ = new List<ulong>();
        }
    }
}
