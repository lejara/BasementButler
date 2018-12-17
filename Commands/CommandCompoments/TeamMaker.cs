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
    public enum TeamMakingStages { empty, listing, made, move }
    public class TeamMaker
    {
        public int numberOFTeams_;

        private static Random rng = new Random();   

        public TeamMakingStages currentStage_ = TeamMakingStages.empty;

        public List<IGuildUser> guildUsersInVoice_;
        public List<Team> teams_;

        public TeamMaker() {
            currentStage_ = TeamMakingStages.empty;
            guildUsersInVoice_ = null;
            numberOFTeams_ = 0;

        }

        //Populate the guildUserInVoice_, everytime this method is called a new list will be created.        
        public void PopulateUsersInVoice(IReadOnlyCollection<Discord.WebSocket.SocketGuildUser> usersList)
        {
            //Reset the lists
            guildUsersInVoice_= new List<IGuildUser>();
            teams_ = new List<Team>();

            foreach (var users in usersList)
            {
                guildUsersInVoice_.Add(users);
            }
        }

        //Makes team slots
        private void CreateTeams() {
            if (numberOFTeams_ != 0)
            {
                teams_ = new List<Team>();
                for (int ctr = 0; ctr < numberOFTeams_; ctr++)
                {
                    teams_.Add(new Team(ctr + 1, String.Format("Team {0}", ctr + 1)));
                }
            }

        }

        //Creates and Assign Teams. TODO: only assign the team
        public void AssignTeams() {
            if (numberOFTeams_ != 0) {
                CreateTeams();
                int counter = 0;
                //add users to teams
                foreach (var user in guildUsersInVoice_) {
                    counter = ((counter + 1) % teams_.Count);
                    teams_[counter].AddUser(user);
                }
                          
            }            
        }

        //Returns a string of all users  in the voice list formatted
        public string ListUsersInVoice()
        {
            string msg = "";
            int ctr = 0;
            foreach (var user in guildUsersInVoice_)
            {

                msg += ++ctr + ". " + user.Username;
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

        //Shuffles the guildUsersInVoice_ 
        public void ShuffleUsersInVoice()
        {
            List<IGuildUser> list = guildUsersInVoice_;
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
