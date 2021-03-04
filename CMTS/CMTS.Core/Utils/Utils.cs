using System;
using System.Collections.Generic;
using CMTS.Core.contracts;

namespace CMTS.Core.Utils
{
    public class Utils :IUtils
    {

        public int GetTotalTalksTime(List<Talks> talksList)
        {
            if (talksList == null || talksList.Count == 0)
                return 0;

            int totalTime = 0;
            foreach (Talks talk in talksList)
            {
                totalTime += talk.TimeDuration;
            }
            return totalTime;
        }

        public string GetNextScheduledTime(DateTime date, int timeDuration)
        {

            TimeSpan time = new TimeSpan(0, 0, timeDuration, 0);

            date = date.Add(time);
            var str = $"{date:hh:mm tt}";
            return str;
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
                    talk.ScheduledTime=scheduledTime;
                    talkList.Add(talk);
                    Console.WriteLine(scheduledTime + $" {talk.Title}");
                    TimeSpan time = new TimeSpan(0, 0, talk.TimeDuration, 0);
                    scheduledTime = GetNextScheduledTime(date, talk.TimeDuration);
                    date = date.Add(time);

                }

                int lunchTimeDuration = 60;
                Talks lunchTalk = new Talks() { Name = "Lunch", Title = "Lunch", TimeDuration = 60};
                lunchTalk.ScheduledTime=scheduledTime;
                talkList.Add(lunchTalk);
                Console.WriteLine(scheduledTime + " Lunch");

                scheduledTime = GetNextScheduledTime(date, lunchTimeDuration);
                TimeSpan time3 = new TimeSpan(0, 0, 60, 0);
                date = date.Add(time3);
                List<Talks> eveSessionTalkList = combForEveSessions[dayCount];
                foreach (Talks talk in eveSessionTalkList)
                {

                    talk.ScheduledTime = scheduledTime;
                    talkList.Add(talk);
                    Console.WriteLine(scheduledTime + $" {talk.Title}");
                    scheduledTime = GetNextScheduledTime(date, talk.TimeDuration);
                    TimeSpan time = new TimeSpan(0, 0, talk.TimeDuration, 0);
                    date = date.Add(time);


                }

                Talks networkingTalk = new Talks(){ Name = "Networking", Title = "Networking", TimeDuration = 60};
                networkingTalk.ScheduledTime = scheduledTime;
                talkList.Add(networkingTalk);
                Console.WriteLine(scheduledTime + " Networking Event\n");
                scheduledTalksList.Add(talkList);
            }

            return scheduledTalksList;
        }

        public List<List<Talks>> FindPossibleCombSession(List<Talks> talksListForOperation, int totalPossibleDays, bool morningSession)
        {
            int minSessionTimeLimit = 180;
            int maxSessionTimeLimit = 240;

            if (morningSession)
                maxSessionTimeLimit = minSessionTimeLimit;

            int talkListSize = talksListForOperation.Count;
            List<List<Talks>> possibleCombinationsOfTalks = new List<List<Talks>>();
            int possibleCombinationCount = 0;

            for (int count = 0; count < talkListSize; count++)
            {
                int startPoint = count;
                int totalTime = 0;
                List<Talks> possibleCombinationList = new List<Talks>();

                while (startPoint != talkListSize)
                {
                    int currentCount = startPoint;
                    startPoint++;
                    Talks currentTalk = talksListForOperation[currentCount];
                    if (currentTalk.Scheduled)
                        continue;
                    int talkTime = currentTalk.TimeDuration;

                    if (talkTime > maxSessionTimeLimit || talkTime + totalTime > maxSessionTimeLimit)
                    {
                        continue;
                    }

                    possibleCombinationList.Add(currentTalk);
                    totalTime += talkTime;

                    if (morningSession)
                    {
                        if (totalTime == maxSessionTimeLimit)
                            break;
                    }
                    else if (totalTime >= minSessionTimeLimit)
                        break;
                }

                bool validSession = false;
                if (morningSession)
                    validSession = (totalTime == maxSessionTimeLimit);
                else
                    validSession = (totalTime >= minSessionTimeLimit && totalTime <= maxSessionTimeLimit);

                if (validSession)
                {
                    possibleCombinationsOfTalks.Add(possibleCombinationList);
                    foreach (Talks talk in possibleCombinationList)
                    {
                        talk.Scheduled =true;
                    }
                    possibleCombinationCount++;
                    if (possibleCombinationCount == totalPossibleDays)
                        break;
                }
            }

            return possibleCombinationsOfTalks;
        }
    }
}
