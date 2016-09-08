using System.Collections.Generic;

namespace AutoLepraTop.Models
{
    public class MainModel
    {
        public List<Video> Videos { get; set; }
        public int PageNumber { get; set; }
        public int PagesCount { get; set; }
    }
}