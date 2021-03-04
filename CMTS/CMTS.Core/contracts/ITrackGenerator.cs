using System.Collections.Generic;

namespace CMTS.Core.Contracts
{
    public interface ITrackGenerator
    {
        List<Talks> ValidateAndCreateTalks(List<string> talkList);
        List<List<Talks>> GetScheduleConferenceTrack(List<Talks> talksList);
    }
}