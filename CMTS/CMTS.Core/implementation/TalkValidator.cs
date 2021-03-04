using System;
using System.Collections.Generic;
using CMTS.Core.Contracts;

namespace CMTS.Core.implementation
{
    public class TalkValidator : ITalkValidator
    {
        private readonly ITrackGenerator _talkGenerator;
        public TalkValidator(ITrackGenerator talkGenerator)
        {
            _talkGenerator = talkGenerator;
        }
        public List<string> GenerateTalkListFrom(string fileName)
        {
            var talkList = new List<string>();
            try
            {
                var lines = System.IO.File.ReadAllLines(fileName);
                talkList.AddRange(lines);
            }
            catch
            {//Catch exception if any

            }

            return talkList;
        }

        public List<List<Talks>> ScheduleConference(List<string> talkList)
        {
            var talksList = _talkGenerator.ValidateAndCreateTalks(talkList);
            return _talkGenerator.GetScheduleConferenceTrack(talksList);
        }
    }
}
