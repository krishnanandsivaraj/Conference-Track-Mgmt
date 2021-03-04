using System;

namespace CMTS.Core.contracts
{
    public interface INextScheduleTime
    {
        string GetNextScheduledTime(DateTime date, int timeDuration);
    }
}
