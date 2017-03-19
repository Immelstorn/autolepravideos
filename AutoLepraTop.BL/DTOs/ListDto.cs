using System.Collections.Generic;

namespace AutoLepraTop.BL.Models
{
    public class ListDto<T>
    {
        public List<T> Comments { get; set; }
        public int Page { get; set; }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
    }
}