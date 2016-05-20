using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Data.Models;
using CFM.Infrastructure.Repositories;
using Mehdime.Entity;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.UnitModule.ViewModels
{
    class UnitDetailsViewModel: BindableBase, INavigationAware
    {
        private readonly IUnitRepository _unitRepository;
        private readonly IDbContextScopeFactory _contextFactory;
        private int lastDeletedId;
        private int currentId;

        public UnitDetailsViewModel(IUnitRepository unitRepository, IDbContextScopeFactory contextFactory)
        {
            _unitRepository = unitRepository;
            _contextFactory = contextFactory;
            lastDeletedId = currentId -1;
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
        #endregion

        /*
         * INavigationAware Implementations
         */
        #region INavigationAware 
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            currentId = Convert.ToInt32(navigationContext.Parameters["unitId"]);
            using (_contextFactory.CreateReadOnly())
            {
                CurrentUnit = await _unitRepository.GetAsync(currentId);
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
            CurrentUnit = new Unit { Code = "", Title = "", Teachers = new List<Professor>()};
        }
        #endregion
    }
}
