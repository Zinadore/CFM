using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulldog.FlyoutManager;
using CFM.AssignmentModule.Views;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Constants;
using CFM.Infrastructure.Events;
using CFM.Infrastructure.Interfaces;
using CFM.Infrastructure.Repositories;
using MahApps.Metro.Controls.Dialogs;
using Mehdime.Entity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.AssignmentModule.ViewModels
{
    public class AssignmentDetailsViewModel: BindableBase, INavigationAware
    {
        private readonly IDbContextScopeFactory _contextFactory;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IFlyoutManager _flyoutManager;
        private readonly IApplicationCommands _applicationCommands;
        private readonly IDialogService _dialogService;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IGoalRepository _goalRepository;
        private int lastDeletedId;
        private int currentId;

        public DelegateCommand EditCommand { get; private set; } 
        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand<int?> NewFeedback { get; private set; }
        public DelegateCommand<int?> FeedbackDetailsCommand { get; private set; }
        public DelegateCommand<int?> DeleteFeedbackCommand { get; private set; }
        public DelegateCommand<int?> NewGoalCommand { get; private set; }
        public DelegateCommand<int?> DeleteGoalCommand { get; private set; }


        public AssignmentDetailsViewModel(IDbContextScopeFactory contextFactory, IAssignmentRepository assignmentRepository,
                IFlyoutManager flyoutManager, IApplicationCommands applicationCommands, IDialogService dialogService,
                IFeedbackRepository feedbackRepository, IEventAggregator eventAggregator, IGoalRepository goalRepository)
        {
            _contextFactory = contextFactory;
            _assignmentRepository = assignmentRepository;
            _flyoutManager = flyoutManager;
            _applicationCommands = applicationCommands;
            _dialogService = dialogService;
            _feedbackRepository = feedbackRepository;
            _goalRepository = goalRepository;
            EditCommand = new DelegateCommand(Edit);
            DeleteCommand = new DelegateCommand(Delete);
            NewFeedback = new DelegateCommand<int?>(OpenFeedback);
            FeedbackDetailsCommand = new DelegateCommand<int?>(FeedbackDetails);
            DeleteFeedbackCommand = new DelegateCommand<int?>(DeleteFeedback);
            NewGoalCommand = new DelegateCommand<int?>(OpenGoal);
            DeleteGoalCommand = new DelegateCommand<int?>(DeleteGoal);
            eventAggregator.GetEvent<FeedbackAddedEvent>().Subscribe(async (i) => await RefreshData());
        }

        #region Command Methods
        private async void DeleteFeedback(int? obj)
        {
            using (var db = _contextFactory.Create())
            {
                _feedbackRepository.Delete(obj.Value);
                await db.SaveChangesAsync();
            }
            await RefreshData();
        }

        private async void DeleteGoal(int? obj)
        {
            using (var db = _contextFactory.Create())
            {
                _goalRepository.Delete(obj.Value);
                await db.SaveChangesAsync();
            }
            await RefreshData();
        }

        private void FeedbackDetails(int? id)
        {
            var uriQuery = new NavigationParameters();
            uriQuery.Add("feedbackId", id);
            _applicationCommands.NavigateCommand.Execute(ViewNames.FeedbackDetailsView + uriQuery);
        }

        private void OpenFeedback(int? id)
        {
            FlyoutParameters fp = new FlyoutParameters();
            fp["fassKey"] = CurrentAssignment.Id;
            fp["feedbackId"] = id.Value;
            _flyoutManager.OpenFlyout(FlyoutNames.FeedbackFlyout, fp);
        }

        private void OpenGoal(int? id)
        {
            FlyoutParameters fp = new FlyoutParameters();
            fp["fassKey"] = CurrentAssignment.Id;
            fp["goalId"] = id.Value;
            _flyoutManager.OpenFlyout(FlyoutNames.GoalFlyout, fp);
        }

        private void Edit()
        {

        }

        private async void Delete()
        {
            var controler = await _dialogService.ShowMessageAsnyc("", "Are you sure you want to delete this entry?", MessageDialogStyle.AffirmativeAndNegative);
            if (controler == MessageDialogResult.Affirmative)
            {
                using (var dbc = _contextFactory.Create())
                {
                    _assignmentRepository.Delete(currentId);
                    await dbc.SaveChangesAsync();
                }
                lastDeletedId = currentId;
                _applicationCommands.NavigateCommand.Execute(typeof(AssignmentsView).FullName);
            }
        }

        private async Task RefreshData()
        {
            using (_contextFactory.CreateReadOnly())
            {
                CurrentAssignment = await _assignmentRepository.FindAsync(u => u.Id == currentId, i => i.Unit, i => i.Feedbacks, i => i.Goals);
                if (CurrentAssignment == null)
                    _applicationCommands.NavigateCommand.Execute(typeof(AssignmentsView).FullName);
                //Loading = false;
                DaysLeft = (CurrentAssignment.Deadline - DateTime.Now).Days;
                DeadlineText = (CurrentAssignment.Deadline > DateTime.Now) ?
                    CurrentAssignment.Deadline.ToShortDateString() + "\t (" + DaysLeft + " days left)"
                    : CurrentAssignment.Deadline.ToShortDateString() + "\t (past deadline)";
            }
        }
        #endregion

        #region Bindable Properties

        private Assignment _currentAssignment;
        public Assignment CurrentAssignment
        {
            get { return _currentAssignment; }
            set { SetProperty(ref _currentAssignment, value); }
        }

        private string _deadlineText;
        public string DeadlineText
        {
            get { return _deadlineText; }
            set { SetProperty(ref _deadlineText, value); }
        }

        private int _daysLeft;
        public int DaysLeft
        {
            get { return _daysLeft; }
            set { SetProperty(ref _daysLeft, value); }
        }

        //public string DeadlineText => (CurrentAssignment.Deadline > DateTime.Now)? CurrentAssignment.Deadline + "\t (" + DaysLeft + " days left)": CurrentAssignment.Deadline + "\t (past deadline)";

        //public double DaysLeft => (CurrentAssignment.Deadline - DateTime.Now).TotalDays;
        #endregion

        #region INavigationAware Implementation
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            //Loading = true;
            currentId = Convert.ToInt32(navigationContext.Parameters["assignmentId"]);
            if (currentId == lastDeletedId)
            {
                _applicationCommands.NavigateCommand.Execute(typeof(AssignmentsView).FullName);
            }
            //TestUnit = await _context.Units.Include(unit => unit.Teachers).FirstAsync(u => u.Id == currentId);
            await RefreshData();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
        #endregion

    }
}
