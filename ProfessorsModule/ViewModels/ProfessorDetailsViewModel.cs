using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Constants;
using CFM.Infrastructure.Events;
using CFM.Infrastructure.Interfaces;
using CFM.Infrastructure.Repositories;
using CFM.ProfessorModule.Views;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.ProfessorModule.ViewModels
{
    class ProfessorDetailsViewModel: BindableBase
    {
        private readonly IProfessorRepository _repository;
        private readonly IApplicationCommands _applicationCommands;
        private readonly IEventAggregator _eventAggregator;
        private readonly IDialogService _dialogService;

        public DelegateCommand UpdateCommand { get; private set; }
        public DelegateCommand DeleteCommand { get; private set; } 

        public ProfessorDetailsViewModel(IProfessorRepository repository, IApplicationCommands applicationCommands
            , IEventAggregator eventAggregator, IDialogService dialogService)
        {
            _repository = repository;
            _applicationCommands = applicationCommands;
            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            UpdateCommand = new DelegateCommand(Update, CanUpdate);
            DeleteCommand = new DelegateCommand(Delete);
        }

        private Professor _professor;

        public Professor Professor
        {
            get { return _professor; }
            set { SetProperty(ref _professor, value); }
        }

        private bool _editMode;

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                SetProperty(ref _editMode, value);
                if (!value)
                {
                    UpdateCommand.Execute();
                }
            }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                SetProperty(ref _firstName, value); 
                UpdateCommand.RaiseCanExecuteChanged();
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                SetProperty(ref _lastName, value); 
                UpdateCommand.RaiseCanExecuteChanged();
            }
        }

        public bool NormalMode => !EditMode;

        public async void LoadData(object id)
        {
            if (id == null)
            {
                Professor = new Professor();
            }
            else
            {
                await Task.Run(() =>
                {
                    var profId = Convert.ToInt32(id);
                    Professor = _repository.Get(profId);
                });
                if (Professor == null)
                {
                    _applicationCommands.NavigateCommand.Execute(typeof(ProfessorsView));
                    return;
                }
                FirstName = Professor.FirstName;
                LastName = Professor.LastName;
            }
        }

        private async void Update()
        {
            Professor.FirstName = FirstName;
            Professor.LastName = LastName;
            await Task.Run(() =>
            {
                _repository.Update(Professor, Professor.Id);
            });
            _eventAggregator.GetEvent<ProfessorUpdatedEvent>().Publish(Professor);
            EditMode = false;
        }

        private bool CanUpdate()
        {
            try
            {
                return FirstName != Professor.FirstName || LastName != Professor.LastName;
            }
            catch 
            {
                
            }
            return false;
        }

        private async void Delete()
        {
            var controler = await _dialogService.ShowMessageAsnyc("", "Are you sure you want to delete this entry?",MessageDialogStyle.AffirmativeAndNegative);
            if (controler == MessageDialogResult.Affirmative)
            {
                await Task.Run(() =>
                {
                    _repository.Delete(Professor);
                });
                _applicationCommands.NavigateCommand.Execute(typeof(ProfessorsView).FullName);
            }
        }
    }
}
