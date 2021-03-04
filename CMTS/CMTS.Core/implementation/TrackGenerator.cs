using System;
using System.Collections.Generic;
using System.Linq;
using CMTS.Core.contracts;
using CMTS.Core.Contracts;

namespace CMTS.Core.implementation
{
    public class TrackGenerator : ITrackGenerator
    {
        private readonly IUtils _utils;
        public TrackGenerator(IUtils utils)
        {
            _utils = utils;

        }
        public List<Talks> ValidateAndCreateTalks(List<string> talkList)
        {
            if (talkList == null)
                throw new InvalidTalkException("Empty Talk List");

            var validTalksList = new List<Talks>();
            const string minSuffix = "min";
            const string lightningSuffix = "lightning";

            foreach (string talk in talkList)
            {
                int lastSpaceIndex = talk.LastIndexOf(" ", StringComparison.Ordinal);
                string name = "";
                if (lastSpaceIndex == -1)
                    throw new InvalidTalkException("Invalid talk, " + talk + ". Talk time must be specify.");

                {
                    name = talk.Substring(0, lastSpaceIndex);
                }

                string timeStr = talk.Substring(lastSpaceIndex + 1);
                if ("".Equals(name.Trim()))
                    throw new InvalidTalkException("Invalid talk name, " + talk);

                if (!timeStr.EndsWith(minSuffix) && !timeStr.EndsWith(lightningSuffix))
                    throw new InvalidTalkException("Invalid talk time, " + talk + ". Time must be in min or in lightning");

                var time = 0;
                try
                {
                    if (timeStr.EndsWith(minSuffix))
                    {
                        time = Convert.ToInt32(timeStr.Substring(0, timeStr.IndexOf(minSuffix, StringComparison.Ordinal)));
                    }
                    else if (timeStr.EndsWith(lightningSuffix))
                    {
                        String lightningTime = timeStr.Substring(0, length: timeStr.IndexOf(lightningSuffix, StringComparison.Ordinal));
                        if (string.IsNullOrEmpty(lightningTime))
                            time = 5;
                        else
                            time = Convert.ToInt32(lightningTime) * 5;
                    }
                }
                catch (FormatException)
                {
                    throw new InvalidTalkException("Unable to parse time " + timeStr + " for talk " + talk);
                }

                validTalksList.Add(new Talks() { Title = talk, Name = name, TimeDuration = time });
            }

            return validTalksList;
        }

        public List<List<Talks>> GetScheduleConferenceTrack(List<Talks> talksList)
        {
            int perDayMinTime = 6 * 60;
            int totalTalksTime = _utils.GetTotalTalksTime(talksList);
            int totalPossibleDays = totalTalksTime / perDayMinTime;

            List<Talks> list = new List<Talks>();
            list.AddRange(talksList);
            List<Talks> talksListForOperation = list.OrderByDescending(x => x.TimeDuration).ToList();

            List<List<Talks>> combForMornSessions = _utils.FindPossibleCombSession(talksListForOperation, totalPossibleDays, true);

            talksListForOperation = combForMornSessions.Aggregate(talksListForOperation, (current, talkList) => current.Except(talkList).ToList());

            List<List<Talks>> combForEveSessions = _utils.FindPossibleCombSession(talksListForOperation, totalPossibleDays, false);

            talksListForOperation = combForEveSessions.Aggregate(talksListForOperation, (current, talkList) => current.Except(talkList).ToList());

            int maxSessionTimeLimit = 240;
            List<Talks> scheduledTalkList = new List<Talks>();
            if (talksListForOperation.Count != 0)
            {
                foreach (var talkList in combForEveSessions)
                {
                    var totalTime = _utils.GetTotalTalksTime(talkList);

                    foreach (Talks talk in talksListForOperation)
                    {
                        int talkTime = talk.TimeDuration;

                        if (talkTime + totalTime <= maxSessionTimeLimit)
                        {
                            talkList.Add(talk);
                            talk.Scheduled = true;
                            scheduledTalkList.Add(talk);
                        }
                    }

                    talksListForOperation = talksListForOperation.Except(scheduledTalkList).ToList();
                    if (talksListForOperation.Count == 0)
                        break;
                }
            }

            if (talksListForOperation.Count != 0)
            {
                throw new Exception("Unable to schedule all task for conferencing.");
            }

            return GetScheduledTalksList(combForMornSessions, combForEveSessions);
        }

        public List<List<Talks>> GetScheduledTalksList(List<List<Talks>> combForMornSessions, List<List<Talks>> combForEveSessions)
        {
            List<List<Talks>> scheduledTalksList = new List<List<Talks>>();
            int totalPossibleDays = combForMornSessions.Count;



            for (int dayCount = 0; dayCount < totalPossibleDays; dayCount++)
            {
                var talkList = new List<Talks>();

                DateTime date = new DateTime();

                date = date.Date.AddHours(9);
                date = date.AddMinutes(0);
                date = date.AddSeconds(0);

                int trackCount = dayCount + 1;
                String scheduledTime = $"{date:hh:mm tt}";

                Console.WriteLine("Track " + trackCount + ":");

                List<Talks> mornSessionTalkList = combForMornSessions[dayCount];
                foreach (Talks talk in mornSessionTalkList)
                {
                    talk.ScheduledTime = scheduledTime;
                    talkList.Add(talk);
                    Console.WriteLine(scheduledTime + $" {talk.Title}");
                    TimeSpan time = new TimeSpan(0, 0, talk.TimeDuration, 0);
                    scheduledTime = _utils.GetNextScheduledTime(date, talk.TimeDuration);
                    date = date.Add(time);

                }

                int lunchTimeDuration = 60;
                Talks lunchTalk = new Talks() { Name = "Lunch", Title = "lunch", TimeDuration = 60 };
                lunchTalk.ScheduledTime = scheduledTime;
                talkList.Add(lunchTalk);
                Console.WriteLine(scheduledTime + " Lunch");

                scheduledTime = _utils.GetNextScheduledTime(date, lunchTimeDuration);
                TimeSpan time3 = new TimeSpan(0, 0, 60, 0);
                date = date.Add(time3);
                List<Talks> eveSessionTalkList = combForEveSessions[dayCount];
                foreach (Talks talk in eveSessionTalkList)
                {

                    talk.ScheduledTime = scheduledTime;
                    talkList.Add(talk);
                    Console.WriteLine(scheduledTime + $" {talk.Title}");
                    scheduledTime = _utils.GetNextScheduledTime(date, talk.TimeDuration);
                    TimeSpan time = new TimeSpan(0, 0, talk.TimeDuration, 0);
                    date = date.Add(time);
                }

                Talks networkingTalk = new Talks() { Name = "Networking Event", Title = "Networking Event", TimeDuration = 60 };
                networkingTalk.ScheduledTime = scheduledTime;
                talkList.Add(networkingTalk);
                Console.WriteLine(scheduledTime + " Networking Event\n");
                scheduledTalksList.Add(talkList);
            }

            return scheduledTalksList;
        }
    }

    public class InvalidTalkException : Exception
    {
        public InvalidTalkException(string emptyTalkList) : base(emptyTalkList)
        {
            
        }
    }
}
