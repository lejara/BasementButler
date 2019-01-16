using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using DiscordButlerBot.Commands.CommandCompoments;
using Discord.WebSocket;
using Discord;

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
        public int maxTopicNameLength_;
        [JsonProperty]
        public bool removeFirstWord_;
        [JsonProperty]
        public List<ulong> voiceChannelIds_;
        
        public string firstWordTitle_;
        public TeamMaker teamMakerInfo_;
        private List<IGuildChannel> avalChannels;

        public GuildServerData() {
            firstWordTitle_ = "";
            teamMakerInfo_ = new TeamMaker();            
        }
        public GuildServerData(SocketGuild g) {
            serverName_ = g.Name;
            id_ = g.Id;
            maxTopicNameLength_ = 16;
            removeFirstWord_ = false;
            firstWordTitle_ = "";
            teamMakerInfo_ = new TeamMaker();
            voiceChannelIds_ = new List<ulong>();
        }

        //Returns a list of empty channels in the guild, including the current channel the user is in.
        public List<IGuildChannel> GetGuildAvailableChannels(IReadOnlyCollection<SocketGuildChannel> guildChannels, SocketGuildChannel currentChannel)
        {
            avalChannels = new List<IGuildChannel>
            {
                currentChannel
            };

            foreach (var gChannel in guildChannels)
            {
                if (gChannel.Users.Count == 0 && voiceChannelIds_.Contains(gChannel.Id))
                {
                    avalChannels.Add(gChannel);
                }
            }
            return avalChannels;
        }
        //Returns a number of empty channels in the guild, including the current channel the user is in.
        public int GetGuildAvailableChannelsCount(IReadOnlyCollection<SocketGuildChannel> guildChannels, SocketGuildChannel currentChannel)
        {
            int counter = 1;

            foreach (var gChannel in guildChannels)
            {
                if (gChannel.Users.Count == 0 && voiceChannelIds_.Contains(gChannel.Id))
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}
