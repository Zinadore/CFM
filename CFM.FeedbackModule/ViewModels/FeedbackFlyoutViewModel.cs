using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulldog.FlyoutManager;
using CFM.Data.Models;
using CFM.Infrastructure.Events;
using CFM.Infrastructure.Repositories;
using Mehdime.Entity;
using Prism.Commands;
using Prism.Events;

namespace CFM.FeedbackModule.ViewModels
{
    public class FeedbackFlyoutViewModel: FlyoutBase
    {
        private readonly IDbContextScopeFactory _contextFactory;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IEventAggregator _eventAggregator;

        public DelegateCommand SaveCommand { get; private set; }

        public FeedbackFlyoutViewModel(IDbContextScopeFactory contextFactory, IAssignmentRepository assignmentRepository,
               IFeedbackRepository feedbackRepository, IEventAggregator EventAggregator)
        {
            _contextFactory = contextFactory;
            _assignmentRepository = assignmentRepository;
            _feedbackRepository = feedbackRepository;
            _eventAggregator = EventAggregator;
            SaveCommand = new DelegateCommand(Save,CanSave)
                                             .ObservesProperty(() => Description);
        }

        #region Command Methods
        private async void Save()
        {
            CurrentFeedback.Description = Description;
            using (var db = _contextFactory.Create())
            {
                if (CurrentFeedback.Id == 0)
                    _feedbackRepository.Add(CurrentFeedback);
                else
                    _feedbackRepository.Update(CurrentFeedback, CurrentFeedback.Id);

                await db.SaveChangesAsync();
            }
            _eventAggregator.GetEvent<FeedbackAddedEvent>().Publish(CurrentFeedback.Id);
            Close();
        }

        private bool CanSave()
        {
            return !String.IsNullOrWhiteSpace(Description);
        }
        #endregion

        #region Bindable Properties

        private Assignment _currentAssignment;
        public Assignment CurrentAssignment
        {
            get { return _currentAssignment; }
            set { SetProperty(ref _currentAssignment, value); }
        }

        private Feedback _currentFeedback;
        public Feedback CurrentFeedback
        {
            get { return _currentFeedback; }
            set { SetProperty(ref _currentFeedback, value); }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }
        
        #endregion


        #region FlyoutBase Implementations
        protected override async void OnOpening(FlyoutParameters flyoutParameters)
        {
            base.OnOpening(flyoutParameters);
            int assId = Convert.ToInt32(flyoutParameters["fassKey"]);
            int fId = Convert.ToInt32(flyoutParameters["feedbackId"]);
            using (_contextFactory.CreateReadOnly())
            {
                CurrentAssignment = await _assignmentRepository.GetAsync(assId);

                if (fId == 0)
                {
                    CurrentFeedback = new Feedback();
                    CurrentFeedback.Assignment = CurrentAssignment;
                    CurrentFeedback.Id = 0;
                    Header = "New Feedback";
                }
                else
                {
                    CurrentFeedback = await _feedbackRepository.GetAsync(fId);
                    Description = CurrentFeedback.Description;
                    Header = "Editing feedback #" + CurrentFeedback.Id;
                }
                    
            }
        }
        #endregion
    }
}
