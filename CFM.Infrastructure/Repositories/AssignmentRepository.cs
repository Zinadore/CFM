using CFM.Data.Models;
using CFM.Infrastructure.Base;
using Mehdime.Entity;

namespace CFM.Infrastructure.Repositories
{
    public class AssignmentRepository: BaseService<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(IAmbientDbContextLocator contextLocator) 
            : base(contextLocator)
        {
        }
    }
}