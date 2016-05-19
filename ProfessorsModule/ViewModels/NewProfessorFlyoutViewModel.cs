using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Bulldog.FlyoutManager;
using CFM.Data.Models;
using CFM.Infrastructure.Events;
using CFM.Infrastructure.Repositories;
using Mehdime.Entity;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.ProfessorModule.ViewModels
{
    public class NewProfessorFlyoutViewModel: FlyoutBase
    {
        private readonly IProfessorRepository _repository;
        private readonly IDbContextScopeFactory _scopeFactory;
        private readonly IEventAggregator _eventAggregator;

        public DelegateCommand SaveCommand { get; private set; }

        public NewProfessorFlyoutViewModel(IProfessorRepository repository, IDbContextScopeFactory scopeFactory,
                                        IEventAggregator eventAggregator)
        {
            _repository = repository;
            _scopeFactory = scopeFactory;
            _eventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(SaveProfessor, CanSaveProfessor);
            Theme = FlyoutTheme.Dark;
            Position = FlyoutPosition.Left;
        }
        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                SetProperty(ref _firstName, value); 
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set
            {
                SetProperty(ref _lastName, value); 
                SaveCommand.RaiseCanExecuteChanged();
            }
        }
        
        private async void SaveProfessor()
        {
            var newProf = new Professor { FirstName = FirstName, LastName = LastName};
            using (var dbc = _scopeFactory.Create())
            {
                _repository.Add(newProf);
                await dbc.SaveChangesAsync();
            }
                _eventAggregator.GetEvent<ProfessorAddedEvent>().Publish(newProf);
            Close();
        }

        private bool CanSaveProfessor()
        {
            return !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName);
        }

        protected override void OnOpening(FlyoutParameters flyoutParameters)
        {
            base.OnOpening(flyoutParameters);
            FirstName = LastName = "";
        }

        protected override void OnClosing(FlyoutParameters flyoutParameters)
        {
            base.OnClosing(flyoutParameters);
            FirstName = LastName = "";
        }
    }
}
