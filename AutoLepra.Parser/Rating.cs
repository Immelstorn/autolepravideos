using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLepra.Parser
{
    public class User
    {
        public object city { get; set; }
        public object ignored { get; set; }
        public object bans { get; set; }
        public object subscribed { get; set; }
        public object rating { get; set; }
        public int deleted { get; set; }
        public string gender { get; set; }
        public object subscribers_count { get; set; }
        public object few_words { get; set; }
        public object wiki_groups { get; set; }
        public int karma { get; set; }
        public object country { get; set; }
        public object ban { get; set; }
        public object attributes { get; set; }
        public string login { get; set; }
        public int active { get; set; }
        public int id { get; set; }
    }

    public class Con
    {
        public int vote { get; set; }
        public User user { get; set; }
    }

    public class Rating
    {
        public string status { get; set; }
        public int rating { get; set; }
        public IList<Con> cons { get; set; }
        public int total_count { get; set; }
        public object pros { get; set; }
        public int cons_count { get; set; }
        public int pros_count { get; set; }
        public object offset { get; set; }
    }
}
