using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFM.Infrastructure.Interfaces
{
    public interface ISearchable<T> where T: class
    {
        string Filter { get; set; }
        ICollection<T> FilteredCollection { get; set; }
    }
}
