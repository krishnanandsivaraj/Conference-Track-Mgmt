using System;
using System.Collections.Generic;
using System.Linq;
using CMTS.Core.implementation;
using Xunit;

namespace CMTS.Core.Tests
{
    [Trait("Category", "TrackGenerator")]

    public class TrackGeneratorTests
    {
        private readonly TrackGenerator _trackGenerator;
        public TrackGeneratorTests()
        {
                _trackGenerator=new TrackGenerator(new Utils.Utils());
        }
        [Fact]
        public void Throws_Error_On_No_Talk()
        {
            List<Talks> talks=new List<Talks>() {new Talks(){ }};
            
            var ex = Assert.Throws<Exception>(()=> this._trackGenerator.GetScheduleConferenceTrack(talks));
            Assert.Equal("Unable to schedule all task for conferencing.",ex.Message);
        }

        [Fact]
        public void Returns_Value_On_Talks()
        {
            List<Talks> talks = new List<Talks>()
            {
                new Talks() { TimeDuration = 60, Name = "test", Title = "test"},
                new Talks() { TimeDuration = 60, Name = "test2", Title = "test2"},
                new Talks() { TimeDuration = 60, Name = "test3", Title = "test3"},
                new Talks() { TimeDuration = 60, Name = "test4", Title = "test4"},
                new Talks() { TimeDuration = 60, Name = "test5", Title = "test5"},
                new Talks() { TimeDuration = 60, Name = "test6", Title = "test6"},
                new Talks() { TimeDuration = 60, Name = "test7", Title = "test7"},
                new Talks() { TimeDuration = 60, Name = "test8", Title = "test8"}
            };
            var result = this._trackGenerator.GetScheduleConferenceTrack(talks);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Validate_Talks_When_Correct_Format()
        {
            List<string> talks = new List<string>()
            {
               "test 80min",
               "test2 60min",
               "test3 40min",
               "test4 45min"
            };
            var result = this._trackGenerator.ValidateAndCreateTalks(talks);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Validate_Talks_When_Wrong_Format()
        {
            List<string> talks = new List<string>()
            {
                "test80min",
                "test2 60min",
            };
            var ex =Assert.Throws<InvalidTalkException>(() => _trackGenerator.ValidateAndCreateTalks(talks));
            Assert.Equal("Invalid talk, test80min. Talk time must be specify.",ex.Message);
        }

        [Fact]
        public void Schedule_Talks_When_Correct_Format()
        {
            List<List<Talks>> talkList = new List<List<Talks>>()
            {
                new List<Talks>()
                {
                    new Talks() {TimeDuration = 60, Name = "test", Title = "test"},
                    new Talks() {TimeDuration = 60, Name = "test2", Title = "test2"},
                    new Talks() {TimeDuration = 60, Name = "test3", Title = "test3"},
                    new Talks() {TimeDuration = 60, Name = "test4", Title = "test4"},
                    new Talks() {TimeDuration = 60, Name = "test5", Title = "test5"},
                    new Talks() {TimeDuration = 60, Name = "test6", Title = "test6"},
                    new Talks() {TimeDuration = 60, Name = "test7", Title = "test7"},
                    new Talks() {TimeDuration = 60, Name = "test8", Title = "test8"}
                }
            };


            var result = this._trackGenerator.GetScheduledTalksList(talkList, talkList);
            Assert.NotEmpty(result);

        }
    }
}
