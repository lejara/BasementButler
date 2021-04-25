using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace DiscordButlerBot.Core
{
    public struct VCTopicData {

        public VCTopicData(DateTime d, Discord.IVoiceChannel vc) {
            dateTime = d;
            voiceChannel = vc;
        }

        public DateTime dateTime;
        public Discord.IVoiceChannel voiceChannel;
    }

    public class FuncHelpers
    {
        static public CancellationToken TrackingTopicToken;
        static private List<VCTopicData> topicTracking;


        static public void Init() {
            Console.WriteLine("Initing FuncHelper....");
            TrackingTopicToken = new CancellationToken();
            topicTracking = new List<VCTopicData>();
            TrackTopicExpiry();
        }

        static public void AddTopicTracking(VCTopicData d) {
            topicTracking.Add(d);
        }

        static private void TrackTopicExpiry()
        {
            RT(() =>
            {
                foreach (var topicData in topicTracking)
                {
                    string vcName = topicData.voiceChannel.Name;
                    //DateTime.Now - topicData.dateTime).TotalHours
                    if ((DateTime.Now - topicData.dateTime).TotalSeconds > 7) {

                        if (RemoveTopicBracket(ref vcName))
                        {
                            //if (Config.serverData[callingUser.GuildId].removeFirstWord_)
                            //{
                            //    vcName = String.Format("{0}{1}", Config.serverData[callingUser.GuildId].firstWordTitle_, vcName);
                            //    Config.serverData[callingUser.GuildId].firstWordTitle_ = "";
                            //}
                            Console.WriteLine("Removing Expired Topic....  " + topicData.voiceChannel.Name);
                            topicData.voiceChannel.ModifyAsync(x =>
                            {
                                x.Name = vcName;
                            });

                        }
                    }                                        
                }
            }, 2, TrackingTopicToken);
        }

        //Returns a changed ref of a string, returns flase if brackets did not exist
        static public bool RemoveTopicBracket(ref string name)
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

        static private void RT(Action action, int seconds, CancellationToken token)
        {
            if (action == null)
                return;
            Task.Run(async () => {
                while (!token.IsCancellationRequested)
                {
                    action();
                    await Task.Delay(TimeSpan.FromSeconds(seconds), token);
                }
            }, token);
        }


    }
}
