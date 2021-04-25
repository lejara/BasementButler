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
            RT(async () =>
            {
                List<VCTopicData> removed = new List<VCTopicData>();
                foreach (var topicData in topicTracking)
                {
                    string vcName = topicData.voiceChannel.Name;
                    //DateTime.Now - topicData.dateTime).TotalHours
                    if ((DateTime.Now - topicData.dateTime).TotalHours > 5) {

                        if (RemoveTopicBracket(ref vcName))
                        {
                            Console.WriteLine("Removing Expired Topic....  " + topicData.voiceChannel.Name);
                            await topicData.voiceChannel.ModifyAsync(x =>
                            {
                                x.Name = vcName;
                            });
                            removed.Add(topicData);
                        }
                    }                                        
                }
                foreach (var topicData in removed) {
                    topicTracking.Remove(topicData);
                }
                removed.Clear();

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
