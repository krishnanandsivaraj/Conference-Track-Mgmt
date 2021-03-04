using System.Collections.Generic;

namespace CMTS.Core.contracts
{
    public interface IScheduledTalkList
    {
        public List<List<Talks>> GetScheduledTalksList(List<List<Talks>> combForMornSessions,
            List<List<Talks>> combForEveSessions);
    }
}
