using CFM.Data;
using CFM.Data.Models;
using CFM.Infrastructure.Base;
using CFM.Infrastructure.Interfaces;
using Mehdime.Entity;

namespace CFM.Infrastructure.Repositories
{
    public class FeedbackRepository: BaseService<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(IAmbientDbContextLocator contextLocator) 
            : base(contextLocator)
        {
        }

        public new Feedback Add(Feedback f)
        {
            ContextLocator.Get<CfmDbContext>().Assignments.Attach(f.Assignment);
            return base.Add(f);
        }

        //public new Feedback Update(Feedback updated, int key)
        //{
            
        //}
    }
}