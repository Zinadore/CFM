using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulldog.FlyoutManager;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Events;
using CFM.Infrastructure.Repositories;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace CFM.UnitModule.ViewModels
{
    public class NewUnitFlyoutViewModel: FlyoutBase
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IProfessorRepository _professorRepository;
        private readonly IEventAggregator _eventAggregator;

        public DelegateCommand AddProfessorCommand { get; private set; }
        public DelegateCommand RemoveProfessorCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public NewUnitFlyoutViewModel(IUnitRepository unitRepository, IProfessorRepository professorRepository,
                                    IEventAggregator eventAggregator)
        {
            _unitRepository = unitRepository;
            _professorRepository = professorRepository;
            _eventAggregator = eventAggregator;
            Teachers = new ObservableCollection<Professor>();
            AddProfessorCommand = new DelegateCommand(AddProfessor);
            RemoveProfessorCommand = new DelegateCommand(RemoveProfessor);
            SaveCommand = new DelegateCommand(Save, CanSave).ObservesProperty(() => Code)
                                                            .ObservesProperty(() => Title)
                                                            .ObservesProperty(()=>Teachers);
            //_eventAggregator.GetEvent<ProfessorAddedEvent>().Subscribe((p) => LoadData());
            //_eventAggregator.GetEvent<ProfessorUpdatedEvent>().Subscribe((p) => LoadData());
        }

        private async void Save()
        {
            var newUnit = new Unit {Code = Code, Title = Title, Teachers = Teachers.ToList()};
            await Task.Run(async () =>
            {
                await _unitRepository.AddAsync(newUnit);
            });
            _eventAggregator.GetEvent<UnitAddedEvent>().Publish(newUnit);
            Code = Title = "";
            Teachers = new ObservableCollection<Professor>(); // Need to fire the OnPropertyChanged event
            IsOpen = false;
        }

        private bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Code) && !string.IsNullOrWhiteSpace(Title) && Teachers.Count > 0;
        }

        private void RemoveProfessor()
        {
            if (Teachers.Contains(SelectedTeacher))
            {
                Teachers.Remove(SelectedTeacher);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private void AddProfessor()
        {
            if (!Teachers.Contains(SelectedProfessor))
            {
                Teachers.Add(SelectedProfessor);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public async void LoadData()
        {
            await Task.Run(() =>
            {
                Professors = _professorRepository.GetAll();
            });
        }

        protected override void OnOpening(FlyoutParameters flyoutParameters)
        {
            base.OnOpening(flyoutParameters);
            LoadData();
        }

        #region Properties
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

        private ICollection<Professor> _professors;

        public ICollection<Professor> Professors
        {
            get { return _professors; }
            set { SetProperty(ref _professors, value); }
        }

        private ObservableCollection<Professor> _teachers;

        public ObservableCollection<Professor> Teachers
        {
            get { return _teachers; }
            set { SetProperty(ref _teachers, value); }
        }
    #endregion
    }
}
