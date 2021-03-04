using System.Collections.Generic;
using CMTS.Core.Contracts;

namespace CMTS.Core
{
    public class ConferenceManager
    {
        private readonly ITalkGenerator _talkGenerator;
        public ConferenceManager(ITalkGenerator talkGenerator)
        {
            _talkGenerator = talkGenerator;
        }

        public List<List<Talks>> GenerateTrack(string fileName)
        {
            return _talkGenerator.GenerateTracks(fileName);
            
        }

    }
}
