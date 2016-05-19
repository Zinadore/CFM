using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using CFM.Data;
using CFM.Data.Models;
using CFM.Infrastructure.Base;
using Mehdime.Entity;

namespace CFM.Infrastructure.Repositories
{
    public class UnitRepository: BaseService<Unit>, IUnitRepository
    {
        public UnitRepository(IAmbientDbContextLocator ambientContextLocator)
            : base(ambientContextLocator)
        {
        }
        public new Unit Add(Unit newUnit)
        {
            var db = ContextLocator.Get<CfmDbContext>();
            foreach (Professor p in newUnit.Teachers)
            {
                db.Entry(p).State = EntityState.Unchanged;
                db.Professors.Attach(p);
            }
            return base.Add(newUnit);
        }

        public new void Update(Unit updated, int key)
        {
            var db = ContextLocator.Get<CfmDbContext>();
            foreach (Professor p in updated.Teachers)
            
           db.Entry(p).State = EntityState.Unchanged;
 
            base.Update(updated, key);
        }
    }
}
