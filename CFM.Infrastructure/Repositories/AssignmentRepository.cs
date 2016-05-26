using System.Data.Entity;
using CFM.Data;
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

        public new Assignment Add(Assignment newEntity)
        {
            var db = ContextLocator.Get<CfmDbContext>();

            db.Units.Attach(newEntity.Unit);
            db.Entry(newEntity.Unit).State = EntityState.Unchanged;

            return base.Add(newEntity);
        }
    }
}