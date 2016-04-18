using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        
        public UnitModule(IRegionManager regionManager, IUnityContainer unityContainer) 
            :base(regionManager, unityContainer)
        {
            RegionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(UnitButton));
            RegionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(NewUnitFlyout));
            RegisterViews();
            RegisterViewModels();
        }

        protected override void RegisterViews()
        {
            base.RegisterViews();
            UnityContainer.RegisterTypeForNavigation<UnitsView>(typeof(UnitsView).FullName);
        }

        protected override void RegisterViewModels()
        {
            base.RegisterViewModels();
            UnityContainer.RegisterType<NewUnitFlyout>();
        }
    }
}
