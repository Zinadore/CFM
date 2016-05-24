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

        public new Unit Update(Unit updated, int key)
        {
            if (updated == null)
                return null;
            var db = ContextLocator.Get<CfmDbContext>();
            // get Unit in its current state
            var existing = db.Units.Include(i => i.Teachers).First(u => u.Id == key);

            if (existing == null)
                return null;
            // work out Professors deleted in the updated Unit
            var deletedProfessors = existing.Teachers.Except(updated.Teachers).ToList();

            // remove the references to removed Professors
            deletedProfessors.ForEach(t => existing.Teachers.Remove(t));

            // work out Professors added in the updated Unit
            var addedProffessors = updated.Teachers.Except(existing.Teachers).ToList();

            // add the references to added Professors
            addedProffessors.ForEach(t =>
            {
                existing.Teachers.Add(t);
                // attach this shit
                db.Professors.Attach(t);
                // let the context know it is an existing entity
                db.Entry(t).State = EntityState.Modified;
            });

            db.Entry(existing).CurrentValues.SetValues(updated);

            return existing;
        }
        
    }
}
