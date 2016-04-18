using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CFM.Data;
using CFM.Data.Models;
using CFM.Infrastructure.Base;
using CFM.Infrastructure.Interfaces;

namespace CFM.Infrastructure.Repositories
{
    public class ProfessorRepository: BaseService<Professor>, IProfessorRepository
    {
        public ProfessorRepository(CfmDbContext context)
            : base(context)
        {
                
        }
    }
}
