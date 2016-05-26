using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulldog.FlyoutManager;
using CFM.AssignmentModule.Views;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Repositories;
using Mehdime.Entity;
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
        private int lastDeletedId;
        private int currentId;

        public AssignmentDetailsViewModel(IDbContextScopeFactory contextFactory, IAssignmentRepository assignmentRepository,
                IFlyoutManager flyoutManager, IApplicationCommands applicationCommands)
        {
            _contextFactory = contextFactory;
            _assignmentRepository = assignmentRepository;
            _flyoutManager = flyoutManager;
            _applicationCommands = applicationCommands;
        }
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

        private double _daysLeft;
        public double DaysLeft
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
            using (_contextFactory.CreateReadOnly())
            {
                CurrentAssignment = await _assignmentRepository.FindAsync(u => u.Id == currentId, includeProperties: i => i.Unit);
                if (CurrentAssignment == null)
                    _applicationCommands.NavigateCommand.Execute(typeof(AssignmentsView).FullName);
                //Loading = false;
                DaysLeft = (CurrentAssignment.Deadline - DateTime.Now).TotalDays;
                DeadlineText = (CurrentAssignment.Deadline > DateTime.Now) ? CurrentAssignment.Deadline + "\t (" + DaysLeft + " days left)" : CurrentAssignment.Deadline + "\t (past deadline)";
            }
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
