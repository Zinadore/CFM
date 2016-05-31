using CFM.Data;
using CFM.Data.Models;
using CFM.Infrastructure.Base;
using Mehdime.Entity;

namespace CFM.Infrastructure.Repositories
{
    public class GoalRepository: BaseService<Goal>, IGoalRepository
    {
        public GoalRepository(IAmbientDbContextLocator contextLocator) 
            : base(contextLocator)
        {
        }

        public new Goal Add(Goal newGoal)
        {
            ContextLocator.Get<CfmDbContext>().Assignments.Attach(newGoal.Assignment);
            return base.Add(newGoal);
        }
    }
}