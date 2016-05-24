using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFM.Data.Models
{
    public class Assignment
    {
        [Key]
        public int Id { get; set; }

        public DateTime Deadline { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        
        public virtual Unit Unit { get; set; }
    }
}
