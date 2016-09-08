using System.Collections.Generic;

namespace AutoLepra.Parser
{
    internal class VideoComparer : IEqualityComparer<Video>
    {
        public bool Equals(Video x, Video y)
        {
            return x.Link == y.Link;
        }

        public int GetHashCode(Video obj)
        {
            return obj.Link.GetHashCode();
        }
    }
}