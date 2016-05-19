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
using Mehdime.Entity;

namespace CFM.Infrastructure.Repositories
{
    public class ProfessorRepository: BaseService<Professor>, IProfessorRepository
    {
        public ProfessorRepository(IAmbientDbContextLocator contextLocator)
            : base(contextLocator)
        {
                
        }
    }
}
