using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bulldog.FlyoutManager;
using CFM.Data;
using CFM.Data.Models;
using CFM.Infrastructure;
using CFM.Infrastructure.Constants;
using CFM.Infrastructure.Events;
using CFM.Infrastructure.Interfaces;
using CFM.Infrastructure.Repositories;
using CFM.UnitModule.Views;
using MahApps.Metro.Controls.Dialogs;
using Mehdime.Entity;
using Prism.Commands;
using Prism.Events;
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
        private readonly IFlyoutManager _flyoutManager;
        private int lastDeletedId;
        private int currentId;

        public DelegateCommand DeleteCommand { get; private set; }
        public DelegateCommand EditCommand { get; private set; }

        public UnitDetailsViewModel(IUnitRepository unitRepository, IDbContextScopeFactory contextFactory,
                                    IApplicationCommands applicationCommands, IDialogService dialogService,
                                    IFlyoutManager flyoutManager, IEventAggregator eventAggregator)
        {
            _unitRepository = unitRepository;
            _contextFactory = contextFactory;
            _applicationCommands = applicationCommands;
            _dialogService = dialogService;
            _flyoutManager = flyoutManager;
            lastDeletedId = currentId -1;
            eventAggregator.GetEvent<UnitEditedEvent>().Subscribe(RefreshUnit);
            DeleteCommand = new DelegateCommand(Delete);
            EditCommand = new DelegateCommand(Edit);
        }

        private async void RefreshUnit(int? obj)
        {
            if (currentId == obj.Value)
                using (_contextFactory.CreateReadOnly())
                {
                    CurrentUnit = await _unitRepository.GetAsync(currentId, includeProperties: i => i.Teachers);
                }
        }

        /*
         * Bindable Properties
         */
        #region Bindable Properties
        private Unit _currentUnit;

        public Unit CurrentUnit
        {
            get { return _currentUnit; }
            set
            {
                SetProperty(ref _currentUnit, value);
            }
        }

        private bool _loading;

        public bool Loading
        {
            get { return _loading; }
            set { SetProperty(ref _loading, value); }
        }

        public bool DoneLoading => !Loading;

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

        private void Edit()
        {
            var parameters = new FlyoutParameters();
            parameters["unitId"] = currentId;
            _flyoutManager.OpenFlyout(FlyoutNames.EditUnitFlyout,parameters);
        }
        #endregion

        /*
         * INavigationAware Implementations
         */
        #region INavigationAware 
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            Loading = true;
            currentId = Convert.ToInt32(navigationContext.Parameters["unitId"]);
            if (currentId == lastDeletedId)
            {
                _applicationCommands.NavigateCommand.Execute(typeof(UnitsView).FullName);
            }
            //TestUnit = await _context.Units.Include(unit => unit.Teachers).FirstAsync(u => u.Id == currentId);
            using (_contextFactory.CreateReadOnly())
            {
                CurrentUnit = await _unitRepository.FindAsync(u => u.Id == currentId, includeProperties: i => i.Teachers);
                if(CurrentUnit == null)
                    _applicationCommands.NavigateCommand.Execute(typeof(UnitsView).FullName);
                Loading = false;
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
