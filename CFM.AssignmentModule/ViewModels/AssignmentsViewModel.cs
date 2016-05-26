using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.AssignmentModule.Views;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Base;
using CFM.Infrastructure.Repositories;
using Mehdime.Entity;
using Prism.Commands;
using Prism.Regions;

namespace CFM.AssignmentModule.ViewModels
{
    public class AssignmentsViewModel: SearchableBindableBase<Assignment>, INavigationAware
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IDbContextScopeFactory _contextFactory;
        private readonly IApplicationCommands _applicationCommands;

        public DelegateCommand<int?> DetailsCommand { get; private set; } 

        public AssignmentsViewModel(IAssignmentRepository assignmentRepository, IDbContextScopeFactory contextFactory,
               IApplicationCommands applicationCommands)
        {
            _assignmentRepository = assignmentRepository;
            _contextFactory = contextFactory;
            _applicationCommands = applicationCommands;
            DetailsCommand = new DelegateCommand<int?>(ShowDetails);
        }

        private void ShowDetails(int? id)
        {
            var uriQuery = new NavigationParameters();
            uriQuery.Add("assignmentId", id);
            Filter = "";//Clear the search filter
            _applicationCommands.NavigateCommand.Execute(typeof(AssignmentDetailsView).FullName + uriQuery);
        }

        protected override void ApplyFilter(string filter)
        {
            FilteredCollection = string.IsNullOrWhiteSpace(filter) ? AllAssignments : AllAssignments.Where((u) =>
                u.Title.ToLower().Contains(filter.ToLower())
            ).OrderByDescending(a => a.Deadline).ToList();
        }

        #region Bindable Properties

        private ICollection<Assignment> _allAssignments;
        public ICollection<Assignment> AllAssignments
        {
            get { return _allAssignments; }
            set { SetProperty(ref _allAssignments, value); }
        }
        #endregion


        #region INavigationAware Implementation
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Filter = "";
            using (_contextFactory.CreateReadOnly())
            {
                AllAssignments = FilteredCollection = await _assignmentRepository.GetAllAsync(a => a.Unit);
            }
                
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Filter = "";
        }
        #endregion
    }
}
