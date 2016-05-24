using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace CFM.UnitModule.ViewModels
{
    class EditUnitFlyoutViewModel: FlyoutBase
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IDbContextScopeFactory _contextFactory;
        private readonly IEventAggregator _eventAggregator;

        public DelegateCommand AddProfessorCommand { get; private set; }
        public DelegateCommand RemoveProfessorCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }

        public EditUnitFlyoutViewModel(IUnitRepository unitRepository, IProfessorRepository professorRepository,
                IDbContextScopeFactory contextFactory, IEventAggregator eventAggregator)
        {
            _unitRepository = unitRepository;
            _professorRepository = professorRepository;
            _contextFactory = contextFactory;
            _eventAggregator = eventAggregator;
            Theme = FlyoutTheme.Dark;
            Position = FlyoutPosition.Bottom;
            AddProfessorCommand = new DelegateCommand(AddProfessor);
            RemoveProfessorCommand = new DelegateCommand(RemoveProfessor);
            SaveCommand = new DelegateCommand(Save, CanSave).ObservesProperty(() => Code)
                                                            .ObservesProperty(() => Title)
                                                            .ObservesProperty(() => Teachers);
            CancelCommand = new DelegateCommand(Close);
        }
    #region Bindable Properties
        private string _code;
        public string Code
        {
            get { return _code; }
            set { SetProperty(ref _code, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _flyoutTitle;
        public string FlyoutTitle
        {
            get { return _flyoutTitle; }
            set { SetProperty(ref _flyoutTitle, value); }
        }

        private Professor _selectedProfessor;
        public Professor SelectedProfessor
        {
            get { return _selectedProfessor; }
            set { SetProperty(ref _selectedProfessor, value); }
        }

        private Professor _selectedTeacher;
        public Professor SelectedTeacher
        {
            get { return _selectedTeacher; }
            set { SetProperty(ref _selectedTeacher, value); }
        }

        private ObservableCollection<Professor> _teachers;
        public ObservableCollection<Professor> Teachers
        {
            get { return _teachers; }
            set { SetProperty(ref _teachers, value); }
        }

        private ICollection<Professor> _allProfessors;
        public ICollection<Professor> AllProfessors
        {
            get { return _allProfessors; }
            set { SetProperty(ref _allProfessors, value); }
        }
        
        private Unit _currentUnit;
        private Unit CurrentUnit
        {
            get { return _currentUnit; }
            set { SetProperty(ref _currentUnit, value); }
        }
        #endregion

        #region Commands
        private async void Save()
        {
            CurrentUnit.Code = Code;
            CurrentUnit.Title = Title;
            CurrentUnit.Teachers = Teachers;
            using (var dbc = _contextFactory.Create())
            {
                _unitRepository.Update(CurrentUnit, CurrentUnit.Id);
                await dbc.SaveChangesAsync();
            }
            _eventAggregator.GetEvent<UnitEditedEvent>().Publish(CurrentUnit.Id);
            Close();
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Code) && !string.IsNullOrWhiteSpace(Title);
        }

        private void RemoveProfessor()
        {
            if (Teachers.Contains(SelectedTeacher) && SelectedTeacher != null)
            {
                Teachers.Remove(SelectedTeacher);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private void AddProfessor()
        {
            if (!Teachers.Contains(SelectedProfessor) && SelectedProfessor != null)
            {
                Teachers.Add(SelectedProfessor);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Flyout Implementations
        protected override async void OnOpening(FlyoutParameters flyoutParameters)
        {
            base.OnOpening(flyoutParameters);
            int id = Convert.ToInt32(flyoutParameters["unitId"]);
            using (_contextFactory.CreateReadOnly())
            {
                CurrentUnit = await _unitRepository.GetAsync(id, u => u.Teachers);
                AllProfessors = await _professorRepository.GetAllAsync();
                FlyoutTitle = "Editing " + CurrentUnit.Code + " - " + CurrentUnit.Title;
                Code = CurrentUnit.Code;
                Title = CurrentUnit.Title;
                Teachers = new ObservableCollection<Professor>(CurrentUnit.Teachers);
            }
        }

        protected override void OnClosing(FlyoutParameters flyoutParameters)
        {
            AllProfessors = null;
            Teachers = null;
            CurrentUnit = null;
            Code = "";
            Title = "";
            SelectedProfessor = SelectedTeacher = null;
            base.OnClosing(flyoutParameters);
        }

        #endregion
    }
}
