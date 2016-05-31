using System;

namespace CFM.Data.Models
{
    public class Goal
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Deadline { get; set; } 

        public virtual Assignment Assignment { get; set; }
    }
}