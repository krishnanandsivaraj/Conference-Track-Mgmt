using System.Collections.Generic;
using CMTS.Core.Contracts;

namespace CMTS.Core
{
    public class TalkGenerator : ITalkGenerator
    {
        private readonly ITalkValidator _talkValidator;
        public TalkGenerator(ITalkValidator talkValidator)
        {
            _talkValidator = talkValidator;
        }

        public List<List<Talks>> GenerateTracks(string fileName)
        {
            var talksList = _talkValidator.GenerateTalkListFrom(fileName);
            return _talkValidator.ScheduleConference(talksList);
        }
    }
}