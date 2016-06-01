using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Repositories;
using Mehdime.Entity;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.FeedbackModule.ViewModels
{
    public class FeedbackDetailsViewModel: BindableBase, INavigationAware
    {
        private readonly IDbContextScopeFactory _contextFactory;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IApplicationCommands _applicationCommands;

        private int currentId;

        public FeedbackDetailsViewModel(IDbContextScopeFactory contextFactory, IFeedbackRepository feedbackRepository,
                IApplicationCommands applicationCommands)
        {
            _contextFactory = contextFactory;
            _feedbackRepository = feedbackRepository;
            _applicationCommands = applicationCommands;
        }

        #region Bindable Properties

        private Feedback _currentFeedback;
        public Feedback CurrentFeedback
        {
            get { return _currentFeedback; }
            set { SetProperty(ref _currentFeedback, value); }
        }
        #endregion

        #region INavigationAware Implementation
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            currentId = Convert.ToInt32(navigationContext.Parameters["feedbackId"]);
            using (_contextFactory.CreateReadOnly())
            {
                CurrentFeedback = await _feedbackRepository.GetAsync(currentId, i => i.Assignment, i => i.Assignment.Unit,
                    i => i.Assignment.Unit.Teachers);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            CurrentFeedback = new Feedback();
        }
        #endregion
    }
}
