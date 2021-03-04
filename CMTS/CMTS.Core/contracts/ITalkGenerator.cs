using System.Collections.Generic;

namespace CMTS.Core.Contracts
{
    public interface ITalkGenerator
    {
        List<List<Talks>> GenerateTracks(string fileName);
    }
}