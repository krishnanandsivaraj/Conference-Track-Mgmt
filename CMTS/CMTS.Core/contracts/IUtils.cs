using System;
using System.Collections.Generic;
using System.Text;

namespace CMTS.Core.contracts
{
    public interface IUtils : ITotalTalksTime, INextScheduleTime, IScheduledTalkList
    {
        int GetTotalTalksTime(List<Talks> talksList);
        string GetNextScheduledTime(DateTime date, int timeDuration);

        List<List<Talks>> GetScheduledTalksList(List<List<Talks>> combForMornSessions,
            List<List<Talks>> combForEveSessions);

        List<List<Talks>> FindPossibleCombSession(List<Talks> talksListForOperation, int totalPossibleDays, bool p2);
    }
}
