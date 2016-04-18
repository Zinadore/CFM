using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data;
using CFM.Data.Models;
using CFM.Infrastructure.Base;

namespace CFM.Infrastructure.Repositories
{
    public class UnitRepository: BaseService<Unit>, IUnitRepository
    {
        public UnitRepository(CfmDbContext context) : base(context)
        {
        }
    }
}
