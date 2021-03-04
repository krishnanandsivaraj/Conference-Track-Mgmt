using System;
using System.Collections.Generic;
using CMTS.Core.contracts;
using CMTS.Core.Contracts;
using CMTS.Core.implementation;
using Moq;
using Xunit;

namespace CMTS.Core.Tests
{

    public class TalkGeneratorTests
    {
        private readonly ITalkGenerator _talkGenerator;
        public TalkGeneratorTests()
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

            var utilMock = new Mock<IUtils>();
            utilMock
                .Setup(x => x.FindPossibleCombSession(It.IsAny<List<Talks>>(), It.IsAny<int>(), It.IsAny<Boolean>()))
                .Returns(talkList);
            utilMock.Setup(x => x.GetNextScheduledTime(It.IsAny<DateTime>(), It.IsAny<int>())).Returns("9 AM");
            utilMock.Setup(x => x.GetScheduledTalksList(It.IsAny<List<List<Talks>>>(), It.IsAny<List<List<Talks>>>())).Returns(talkList);
            utilMock.Setup(x => x.GetTotalTalksTime(It.IsAny<List<Talks>>())).Returns(180);

            this._talkGenerator = new TalkGenerator(new TalkValidator(new TrackGenerator(utilMock.Object)));
        }

        [Fact]
        public void Can_TalkValidatorGenerateTalk_Tests()
        {
            var result = _talkGenerator.GenerateTracks("");
            Assert.NotEmpty(result);
        }
    }
}