using System;

namespace AutoLepra.Parser
{
    public class Video : IComparable
    {
        public int Vote { get; set; }
        public string Comment { get; set; }
        public string Link { get; set; }
        public int TimeStamp{ get; set; }

        public int CompareTo(object obj)
        {
            return ((Video)obj).Vote - Vote;
        }
    }
}