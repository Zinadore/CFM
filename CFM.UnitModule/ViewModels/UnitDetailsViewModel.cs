using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Interfaces;
using CFM.Infrastructure.Repositories;
using CFM.UnitModule.Views;
using MahApps.Metro.Controls.Dialogs;
using Mehdime.Entity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.UnitModule.ViewModels
{
    class UnitDetailsViewModel: BindableBase, INavigationAware
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IDbContextScopeFactory _contextFactory;
        private readonly IApplicationCommands _applicationCommands;
        private readonly IDialogService _dialogService;
        private int lastDeletedId;
        private int currentId;

        public DelegateCommand DeleteCommand { get; private set; }

        public UnitDetailsViewModel(IUnitRepository unitRepository, IDbContextScopeFactory contextFactory,
                                    IApplicationCommands applicationCommands, IDialogService dialogService)
        {
            _unitRepository = unitRepository;
            _contextFactory = contextFactory;
            _applicationCommands = applicationCommands;
            _dialogService = dialogService;
            lastDeletedId = currentId -1;
            DeleteCommand = new DelegateCommand(Delete);
        }

        /*
         * Bindable Properties
         */
        #region Bindable Properties
        private NotifyTaskCompletion<Unit> _currentUnit;

        public NotifyTaskCompletion<Unit> CurrentUnit
        {
            get { return _currentUnit; }
            set
            {
                SetProperty(ref _currentUnit, value);
            }
        }
        #endregion

        /*
         * Command Methods
         */
        #region Command Methods

        private async void Delete()
        {
            var controler = await _dialogService.ShowMessageAsnyc("", "Are you sure you want to delete this entry?", MessageDialogStyle.AffirmativeAndNegative);
            if (controler == MessageDialogResult.Affirmative)
            {
                using (var dbc = _contextFactory.Create())
                {
                    _unitRepository.Delete(currentId);
                    await dbc.SaveChangesAsync();
                }
                lastDeletedId = currentId;
                _applicationCommands.NavigateCommand.Execute(typeof(UnitsView).FullName);
            }
        }
        #endregion

        /*
         * INavigationAware Implementations
         */
        #region INavigationAware 
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            currentId = Convert.ToInt32(navigationContext.Parameters["unitId"]);
            if (currentId == lastDeletedId)
            {
                _applicationCommands.NavigateCommand.Execute(typeof(UnitsView).FullName);
            }
            using (_contextFactory.CreateReadOnly())
            {
                CurrentUnit = new NotifyTaskCompletion<Unit>(_unitRepository.GetAsync(currentId));
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (lastDeletedId != -1)
            {
                int id = Convert.ToInt32(navigationContext.Parameters["unitId"]);
                return lastDeletedId == id;
            }
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            CurrentUnit = null;
        }
        #endregion
    }
}
