namespace CMTS.Core
{
    public class Talks
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public int TimeDuration { get; set; }
        public bool Scheduled { get; set; } = false;
        public string ScheduledTime { get; set; }
    }
}