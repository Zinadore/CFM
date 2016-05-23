using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CFM.Data.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }


        public virtual ICollection<Professor> Teachers { get; set; }

    }
}
