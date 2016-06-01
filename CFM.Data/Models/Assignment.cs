using System;
using System.Collections.Generic;

namespace CFM.Data.Models
{
    public class Assignment
    {
        public int Id { get; set; }

        public DateTime Deadline { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        
        public virtual Unit Unit { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; } 
        public virtual ICollection<Goal> Goals { get; set; }
    }
}
