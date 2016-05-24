using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data.Models;
using CFM.Infrastructure.Base;
using CFM.Infrastructure.Repositories;
using Mehdime.Entity;
using Prism.Regions;

namespace CFM.AssignmentModule.ViewModels
{
    public class AssignmentsViewModel: SearchableBindableBase<Assignment>, INavigationAware
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IDbContextScopeFactory _contextFactory;

        public AssignmentsViewModel(IAssignmentRepository assignmentRepository, IDbContextScopeFactory contextFactory)
        {
            _assignmentRepository = assignmentRepository;
            _contextFactory = contextFactory;
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
                AllAssignments = await _assignmentRepository.GetAllAsync();
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
