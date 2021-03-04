using System.Collections.Generic;

namespace CMTS.Core.Contracts
{
    public interface ITalkValidator
    {
        List<string> GenerateTalkListFrom(string fileName);
        List<List<Talks>> ScheduleConference(List<string> talkList);

    }
}
