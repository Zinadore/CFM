using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Constants;
using CFM.Infrastructure.Repositories;
using CFM.Infrastructure.Services;
using Mehdime.Entity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.FeedbackModule.ViewModels
{
    public class FeedbackDetailsViewModel: BindableBase, INavigationAware
    {
        private readonly IDbContextScopeFactory _contextFactory;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IApplicationCommands _applicationCommands;
        private readonly ScoringService _scoringService;

        private int currentId;

        public DelegateCommand<int?> FeedbackDetailsCommand { get; private set; } 

        public FeedbackDetailsViewModel(IDbContextScopeFactory contextFactory, IFeedbackRepository feedbackRepository,
                IApplicationCommands applicationCommands, ScoringService scoringService)
        {
            _contextFactory = contextFactory;
            _feedbackRepository = feedbackRepository;
            _applicationCommands = applicationCommands;
            _scoringService = scoringService;

            FeedbackDetailsCommand = new DelegateCommand<int?>(FeedbackDetails);
        }

        

        #region Bindable Properties

        private Feedback _currentFeedback;
        public Feedback CurrentFeedback
        {
            get { return _currentFeedback; }
            set { SetProperty(ref _currentFeedback, value); }
        }

        private ICollection<Feedback> _relevantFeedback;
        public ICollection<Feedback> RelevantFeedback
        {
            get { return _relevantFeedback; }
            set { SetProperty(ref _relevantFeedback, value); }
        }
        #endregion

        #region Command Methods
        private void FeedbackDetails(int? id)
        {
            var uriQuery = new NavigationParameters();
            uriQuery.Add("feedbackId", id.Value);
            _applicationCommands.NavigateCommand.Execute(ViewNames.FeedbackDetailsView + uriQuery);
        }
        #endregion

        #region INavigationAware Implementation
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            currentId = Convert.ToInt32(navigationContext.Parameters["feedbackId"]);
            CurrentFeedback = null;
            using (_contextFactory.CreateReadOnly())
            {
                CurrentFeedback = await _feedbackRepository.FindAsync(f => f.Id == currentId, i => i.Assignment, i => i.Assignment.Unit,
                    i => i.Assignment.Unit.Teachers);
                var temp = await _feedbackRepository.GetAllAsync(i => i.Assignment, i => i.Assignment.Unit,
                            i => i.Assignment.Unit.Teachers);
                _scoringService.ScoreList(CurrentFeedback, ref temp);
                RelevantFeedback = temp;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            CurrentFeedback = null;
        }
        #endregion
    }
}
