using System;
using System.Collections.Generic;

namespace AutoLepraTop.BL.Models
{
    public class ListDto<T>
    {
        public List<T> Comments { get; set; }
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public string LastUpdated { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}