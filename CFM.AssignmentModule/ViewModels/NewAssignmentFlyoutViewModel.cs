using System;
using System.Collections;
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
using Prism.Regions;

namespace CFM.AssignmentModule.ViewModels
{
    public class NewAssignmentFlyoutViewModel: FlyoutBase, INavigationAware
    {
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IUnitRepository _unitRepository;
        private readonly IDbContextScopeFactory _contextFactory;
        private readonly IEventAggregator _eventAggregator;

        public DelegateCommand SaveCommand { get; private set; } 

        public NewAssignmentFlyoutViewModel(IAssignmentRepository assignmentRepository, IUnitRepository unitRepository, IDbContextScopeFactory contextFactory,
                IEventAggregator eventAggregator)
        {
            _assignmentRepository = assignmentRepository;
            _unitRepository = unitRepository;
            _contextFactory = contextFactory;
            _eventAggregator = eventAggregator;
            Position = FlyoutPosition.Left;
            Theme = FlyoutTheme.Dark;
            Deadline = DateTime.Now;
            SelectedUnit = null;
            SaveCommand = new DelegateCommand(Save, CanSave)
                                             .ObservesProperty(() => Title)
                                             .ObservesProperty(() => SelectedUnit)
                                             .ObservesProperty(() => SelectedType)
                                             .ObservesProperty(() => Deadline);
        }

        private async void Save()
        {
            var newAssignment = new Assignment
            {
                Title = Title,
                Unit = SelectedUnit,
                Type = SelectedType,
                Deadline = Deadline
            };
            using (var db = _contextFactory.Create())
            {
                newAssignment = _assignmentRepository.Add(newAssignment);
                await db.SaveChangesAsync();
            }
            _eventAggregator.GetEvent<AssignmentAddedEvent>().Publish(newAssignment.Id);
            Close();
        }

        private bool CanSave()
        {
            return !String.IsNullOrWhiteSpace(Title) &&
                   !String.IsNullOrWhiteSpace(SelectedType) &&
                   SelectedUnit != null &&
                   Deadline != DateTime.Now;
        }

        #region Bindable Properties

        private ICollection<Unit> _availableUnits;
        public ICollection<Unit> AvailableUnits
        {
            get { return _availableUnits; }
            set { SetProperty(ref _availableUnits, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _selectedType;
        public string SelectedType
        {
            get { return _selectedType; }
            set { SetProperty(ref _selectedType, value); }
        }

        private DateTime _deadline;
        public DateTime Deadline
        {
            get { return _deadline; }
            set { SetProperty(ref _deadline, value); }
        }

        private Unit _selectedUnit;
        public Unit SelectedUnit
        {
            get { return _selectedUnit; }
            set { SetProperty(ref _selectedUnit, value); }
        }
        #endregion




        #region INavigtionAware Implementation
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            AvailableUnits = null;
        }
        #endregion

        #region FlyoutBase Implementation

        protected override async void OnOpening(FlyoutParameters flyoutParameters)
        {
            base.OnOpening(flyoutParameters);
            using (_contextFactory.CreateReadOnly())
            {
                AvailableUnits = await _unitRepository.GetAllAsync();
            }
        }

        protected override void OnClosing(FlyoutParameters flyoutParameters)
        {
            base.OnClosing(flyoutParameters);
        }

        #endregion
    }
}
