using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFM.Data.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public Assignment Assignment { get; set; }

        [NotMapped]
        public virtual double RelevanceScore { get; set; }
    }
}
