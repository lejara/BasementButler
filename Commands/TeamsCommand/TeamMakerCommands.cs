using DiscordButlerBot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord;
using Discord.WebSocket;
using DiscordButlerBot.Commands.CommandCompoments;

namespace DiscordButlerBot.Commands
{
    public class TeamMakerCommands : CommandBase
    {

        [Command("MakeTeams")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task MakeTeams(string numOfTeams = ""){
            int numberOFTeams = 0;
            if (Int32.TryParse(numOfTeams, out numberOFTeams))
            {
                var guildUser = Context.User as SocketGuildUser;
                var voiceChannelUserIn = guildUser.VoiceChannel;
                
                //Set number of teams
                Config.serverData[guildUser.Guild.Id].teamMakerInfo_.numberOFTeams_ = numberOFTeams;
                
                //Set Default Channel
                Config.serverData[guildUser.Guild.Id].teamMakerInfo_.defualtVoiceChannel_ = voiceChannelUserIn.Id;

                //Populate users
                Config.serverData[guildUser.Guild.Id].teamMakerInfo_.PopulateUsersInVoice(voiceChannelUserIn.Users);

                //check if there is enough users for the number of teams
                if (Config.serverData[guildUser.Guild.Id].teamMakerInfo_.guildUsersInVoice_.Count >= numberOFTeams)
                {

                    //Output and list
                    string msg = "Master, here is your list of users that will be place into teams: \n";
                    EmbedBuilder embed = Config.serverData[guildUser.Guild.Id].teamMakerInfo_.ListUsersInVoiceEmbed();
                    embed.WithTitle("Members to be in teams");
                    embed.WithFooter("Would you like to \"!MakeRandom\" the teams, or \"!exclude #\" a user?");
                    Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ = TeamMakingStages.listing;

                    await Context.Channel.SendMessageAsync(msg, false, embed);
                }
                else {
                    await Context.Channel.SendMessageAsync("There are far too few users for the number of teams you like master!");
                }
                         
            }
            else {
                await Context.Channel.SendMessageAsync(String.Format("Sorry master {0}, i need to know the number of teams first. (!MakeTeams #)", Context.User.Mention));
            }            
        }

        [Command("exclude")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task ExcludeUser(string idx="") {

            var guildUser = Context.User as SocketGuildUser;

            if (Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ == TeamMakingStages.listing)
            {
                int index = 0;
                if (Int32.TryParse(idx, out index))
                {
                    index--;

                    var removingUser = Config.serverData[guildUser.Guild.Id].teamMakerInfo_.guildUsersInVoice_.ElementAt(index);

                    if (removingUser == null)
                    {
                        await Context.Channel.SendMessageAsync(String.Format("Sorry master {0}, could not find the user based on the number you gave.", Context.User.Mention ));
                    }
                    else
                    {
                        if (Config.serverData[guildUser.Guild.Id].teamMakerInfo_.guildUsersInVoice_.Count - 1 >= Config.serverData[guildUser.Guild.Id].teamMakerInfo_.numberOFTeams_)
                        {
                            Config.serverData[guildUser.Guild.Id].teamMakerInfo_.guildUsersInVoice_.Remove(removingUser);

                            string name = removingUser.Mention;
                            string msg = String.Format("I have removed \"{0}\" from the list. The new list is now: \n", name);
                            EmbedBuilder embed = Config.serverData[guildUser.Guild.Id].teamMakerInfo_.ListUsersInVoiceEmbed();
                            embed.WithFooter("Would you like to \"!MakeRandom\" the teams, or again \"!exclude #\" a user?");
                            await Context.Channel.SendMessageAsync(msg, false, embed);
                        }
                        else {
                            await Context.Channel.SendMessageAsync(String.Format("Master {0}, there are now too few users to be in the teams you like now. Remake (!MakeTeams #).", Context.User.Mention));
                            Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ = TeamMakingStages.empty;
                        }
                    }
                }
                else
                {
                    await Context.Channel.SendMessageAsync(String.Format("Sorry master {0}, you need to give me a valid number based on my list (!exclude #)", Context.User.Mention));
                }
            }
        }
        [Command("makerandom")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task MakeRandom() {

            var guildUser = Context.User as SocketGuildUser;

            if (Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ == TeamMakingStages.listing || 
                Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ == TeamMakingStages.move ||
                Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ == TeamMakingStages.NeedForceMove ||
                Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ == TeamMakingStages.made) {

                Config.serverData[guildUser.Guild.Id].teamMakerInfo_.ShuffleUsersInVoice();
                Config.serverData[guildUser.Guild.Id].teamMakerInfo_.AssignTeams();

                string msg = "";
                EmbedBuilder embed = new EmbedBuilder();
                embed.WithColor(200, 0, 0);
                embed.WithThumbnailUrl("https://cdn0.iconfinder.com/data/icons/sea-nautical-pirate-maritime/35/9-512.png");
                foreach (var team in Config.serverData[guildUser.Guild.Id].teamMakerInfo_.teams_) {
                    msg += team.GetStringMembersFormatted();

                }
                embed.WithDescription(msg);

                var guild = Context.Client.Guilds.FirstOrDefault(g => g.Id == guildUser.Guild.Id);
                var voiceChannelUserIn = guildUser.VoiceChannel;
            
                //check if theres enough voice channels for each teams
                if (Config.serverData[guildUser.Guild.Id].GetGuildAvailableChannelsCount(guild.Channels, voiceChannelUserIn) >= Config.serverData[guildUser.Guild.Id].teamMakerInfo_.teams_.Count) {
                    embed.WithFooter("Would you like to \"!moveteams\" to different voice channels?");
                    Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ = TeamMakingStages.move;
                }
                else if (Config.serverData[guildUser.Guild.Id].voiceChannelIds_.Count >= Config.serverData[guildUser.Guild.Id].teamMakerInfo_.teams_.Count)
                {
                    embed.WithFooter("Not enough empty channels, you can still force the move sir \"!moveteams force\". ");
                    Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ = TeamMakingStages.move;
                }
                else {
                    Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ = TeamMakingStages.made;
                    embed.WithFooter("Cannot move teams, not enough voice channels for each teams");
                }
                
                await Context.Channel.SendMessageAsync("These are the teams: \n", false, embed);
            }
        }
        
        [Command("moveteams")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task MoveTeams(string parm = "") {

            var guildUser = Context.User as SocketGuildUser;

            if (Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ == TeamMakingStages.move) {

                int counter = 0;
                if (parm.ToLower().Contains("force"))
                {
                    //Move teams to different channels
                    foreach (var team in Config.serverData[guildUser.Guild.Id].teamMakerInfo_.teams_)
                    {
                        await team.MoveUsers(Config.serverData[guildUser.Guild.Id].voiceChannelIds_[counter]);
                        counter++;
                    }
                    await Context.Channel.SendMessageAsync("Teams are now in their channels. You can \"!groupback\" to bring everyone together.");
                }
                else {
                    //check the availability of voice channels again
                    var guild = Context.Client.Guilds.FirstOrDefault(g => g.Id == guildUser.Guild.Id);
                    var voiceChannelUserIn = guildUser.VoiceChannel;

                    List<IGuildChannel> avalChannels = Config.serverData[guildUser.Guild.Id].GetGuildAvailableChannels(guild.Channels, voiceChannelUserIn);

                    if (avalChannels.Count >= Config.serverData[guildUser.Guild.Id].teamMakerInfo_.teams_.Count)
                    {
                        //Move teams to different channels
                        foreach (var team in Config.serverData[guildUser.Guild.Id].teamMakerInfo_.teams_)
                        {
                            await team.MoveUsers(avalChannels[counter].Id);
                            counter++;
                        }
                        await Context.Channel.SendMessageAsync("Teams are now in their channels. You can \"!groupback\" to bring everyone together.");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync(String.Format("Sorry master {0}, there are not enough empty channels anymore! You can \"!moveteams force\" or try again.", Context.User.Mention));
                    }
                }
            }
        }

        [Command("groupback")]
        [RequireUserPermission(Discord.GuildPermission.MoveMembers)]
        public async Task MoveBackTeams(){

            var guildUser = Context.User as SocketGuildUser;

            if (Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ == TeamMakingStages.move || 
                Config.serverData[guildUser.Guild.Id].teamMakerInfo_.currentStage_ == TeamMakingStages.NeedForceMove)
            {
                foreach (var team in Config.serverData[guildUser.Guild.Id].teamMakerInfo_.teams_) {
                    await team.MoveUsers(Config.serverData[guildUser.Guild.Id].teamMakerInfo_.defualtVoiceChannel_);
                }
                await Context.Channel.SendMessageAsync("I have moved everyone back.");
            }
        }
    }
}
