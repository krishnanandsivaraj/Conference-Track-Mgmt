using System.Collections.Generic;

namespace CMTS.Core.contracts
{
    public interface ITotalTalksTime
    {
        public int GetTotalTalksTime(List<Talks> talksList);
    }
}
