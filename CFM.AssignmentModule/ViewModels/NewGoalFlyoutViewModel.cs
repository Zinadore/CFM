using System;
using Bulldog.FlyoutManager;
using CFM.Data.Models;
using CFM.Infrastructure.Events;
using CFM.Infrastructure.Repositories;
using Mehdime.Entity;
using Prism.Commands;
using Prism.Events;

namespace CFM.AssignmentModule.ViewModels
{
    public class NewGoalFlyoutViewModel: FlyoutBase
    {
        private readonly IDbContextScopeFactory _contextFactory;
        private readonly IGoalRepository _goalRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IEventAggregator _eventAggregator;

        public DelegateCommand SaveCommand { get; private set; } 

        public NewGoalFlyoutViewModel(IDbContextScopeFactory contextFactory, IGoalRepository goalRepository, IAssignmentRepository assignmentRepository,
                IEventAggregator eventAggregator)
        {
            _contextFactory = contextFactory;
            _goalRepository = goalRepository;
            _assignmentRepository = assignmentRepository;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(Save, CanSave)
                                                .ObservesProperty(() => Description);
        }



        #region Command Methods
        private bool CanSave()
        {
            return !String.IsNullOrWhiteSpace(Description);
        }

        private async void Save()
        {
            CurrentGoal.Description = Description;
            CurrentGoal.Deadline = Deadline;
            using (var db = _contextFactory.Create())
            {
                if (CurrentGoal.Id == 0)
                    _goalRepository.Add(CurrentGoal);
                else
                    _goalRepository.Update(CurrentGoal, CurrentGoal.Id);

                await db.SaveChangesAsync();
            }
            _eventAggregator.GetEvent<GoalAddedEvent>().Publish(CurrentGoal.Id);
            ClearData();
            Close();
        }

        private void ClearData()
        {
            Description = "";
        }
        #endregion

        #region Bindable Properties

        private string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private DateTime _deadline;
        public DateTime Deadline
        {
            get { return _deadline; }
            set { SetProperty(ref _deadline, value); }
        }

        private Goal _currentGoal;
        public Goal CurrentGoal
        {
            get { return _currentGoal; }
            set { SetProperty(ref _currentGoal, value); }
        }

        private Assignment _currentAssignment;
        public Assignment CurrentAssignment
        {
            get { return _currentAssignment; }
            set { SetProperty(ref _currentAssignment, value); }
        }
        #endregion

        #region FlyoutBase Implementations
        protected override async void OnOpening(FlyoutParameters flyoutParameters)
        {
            base.OnOpening(flyoutParameters);
            int assId = Convert.ToInt32(flyoutParameters["fassKey"]);
            int gId = Convert.ToInt32(flyoutParameters["goalId"]);
            using (_contextFactory.CreateReadOnly())
            {
                CurrentAssignment = await _assignmentRepository.GetAsync(assId);

                if (gId == 0)
                {
                    CurrentGoal = new Goal();
                    CurrentGoal.Assignment = CurrentAssignment;
                    CurrentGoal.Id = 0;
                    Deadline = CurrentAssignment.Deadline;
                    Header = "New Goal";
                }
                else
                {
                    CurrentGoal = await _goalRepository.GetAsync(gId);
                    Description = CurrentGoal.Description;
                    Deadline = CurrentGoal.Deadline;
                    Header = "Editing goal #" + CurrentGoal.Id;
                }

            }
        }
        #endregion
    }
}