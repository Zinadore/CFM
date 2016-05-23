using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulldog.FlyoutManager;
using CFM.Infrastructure.Base;
using CFM.Infrastructure.Constants;
using CFM.UnitModule.Navigation;
using CFM.UnitModule.Views;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;

namespace CFM.UnitModule
{
    public class UnitModule: PrismModuleBase
    {
        private readonly IFlyoutManager _flyoutManager;

        public UnitModule(IRegionManager regionManager, IUnityContainer unityContainer, IFlyoutManager flyoutManager) 
            :base(regionManager, unityContainer)
        {
            _flyoutManager = flyoutManager;
            RegionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(UnitButton));
            RegisterViews();
            RegisterViewModels();
        }

        public override void Initialize()
        {
            base.Initialize();
            _flyoutManager.RegisterFlyout<NewUnitFlyoutView>(FlyoutNames.NewUnitFlyout, RegionNames.FlyoutRegion);
            _flyoutManager.RegisterFlyout<EditUnitFlyoutView>(FlyoutNames.EditUnitFlyout, RegionNames.FlyoutRegion);
        }

        protected override void RegisterViews()
        {
            base.RegisterViews();
            UnityContainer.RegisterTypeForNavigation<UnitsView>(typeof(UnitsView).FullName);
            UnityContainer.RegisterTypeForNavigation<UnitDetailsView>(typeof(UnitDetailsView).FullName);
        }

        protected override void RegisterViewModels()
        {
            base.RegisterViewModels();
        }
    }
}
