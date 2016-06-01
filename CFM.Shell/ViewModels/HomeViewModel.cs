using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulldog.FlyoutManager;
using CFM.AssignmentModule.Views;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Constants;
using CFM.Infrastructure.Events;
using CFM.Infrastructure.Repositories;
using Mehdime.Entity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.Shell.ViewModels
{
    public class HomeViewModel: BindableBase, INavigationAware
    {
        private readonly IFlyoutManager _flyoutManager;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IDbContextScopeFactory _contextFactory;
        private readonly IApplicationCommands _applicationCommands;

        public DelegateCommand AssignmentCommand { get; private set; }
        public DelegateCommand FeedbackCommand { get; private set; }
        public DelegateCommand<int?> ShowAssignmentCommand { get; private set; }

        public HomeViewModel(IFlyoutManager flyoutManager, IAssignmentRepository assignmentRepository,
            IDbContextScopeFactory contextFactory, IApplicationCommands applicationCommands, IEventAggregator eventAggregator)
        {
            _flyoutManager = flyoutManager;
            _assignmentRepository = assignmentRepository;
            _contextFactory = contextFactory;
            _applicationCommands = applicationCommands;
            AssignmentCommand = new DelegateCommand(NewAssignment);
            FeedbackCommand = new DelegateCommand(NewFeedback);
            ShowAssignmentCommand = new DelegateCommand<int?>(ShowAssignment);
            eventAggregator.GetEvent<AssignmentAddedEvent>().Subscribe(async (a) => await RefreshData());
        }



        #region Command Methods
        private void NewAssignment()
        {
            _flyoutManager.OpenFlyout(FlyoutNames.NewAssignmentFlyout);
        }

        private void NewFeedback()
        {
            FlyoutParameters fp = new FlyoutParameters();
            fp["fassKey"] = SelectedAssignment.Id;
            fp["feedbackId"] = 0;
            _flyoutManager.OpenFlyout(FlyoutNames.FeedbackFlyout, fp);
        }

        private void ShowAssignment(int? id)
        {
            var uriQuery = new NavigationParameters();
            uriQuery.Add("assignmentId", id);
            _applicationCommands.NavigateCommand.Execute(typeof(AssignmentDetailsView).FullName + uriQuery);
        }

        private async Task RefreshData()
        {
            using (_contextFactory.CreateReadOnly())
            {
                AllAssignments = await _assignmentRepository.GetAllAsync(i => i.Unit);
                UpcomingAssignments = AllAssignments.OrderByDescending(a => a.Deadline).Take(6).ToList();
            }
        }
        #endregion

        #region Bindable Properties

        private ICollection<Assignment> _upcomingAssignments;
        public ICollection<Assignment> UpcomingAssignments
        {
            get { return _upcomingAssignments; }
            set { SetProperty(ref _upcomingAssignments, value); }
        }

        private ICollection<Assignment> _allAssignments;
        public ICollection<Assignment> AllAssignments
        {
            get { return _allAssignments; }
            set { SetProperty(ref _allAssignments, value); }
        }

        private Assignment _selectedAssignment;
        public Assignment SelectedAssignment
        {
            get { return _selectedAssignment; }
            set { SetProperty(ref _selectedAssignment, value); }
        }
        #endregion

        #region INavigationAware Implementation
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
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
