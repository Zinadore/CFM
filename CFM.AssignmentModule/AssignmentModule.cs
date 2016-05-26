using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bulldog.FlyoutManager;
using CFM.AssignmentModule.Navigation;
using CFM.AssignmentModule.Views;
using CFM.Infrastructure.Base;
using CFM.Infrastructure.Constants;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Prism.Unity;

namespace CFM.AssignmentModule
{
    public class AssignmentModule: PrismModuleBase
    {
        private readonly IFlyoutManager _flyoutManager;

        public AssignmentModule(IRegionManager regionManager, IUnityContainer unityContainer, IFlyoutManager flyoutManager) 
            : base(regionManager, unityContainer)
        {
            _flyoutManager = flyoutManager;
            regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(AssignmentButton));
        }

        public override void Initialize()
        {
            base.Initialize();
            RegisterViews();
            _flyoutManager.RegisterFlyout<NewAssignmentFlyoutView>(FlyoutNames.NewAssignmentFlyout, RegionNames.FlyoutRegion);
        }

        protected override void RegisterViews()
        {
            base.RegisterViews();
            UnityContainer.RegisterTypeForNavigation<AssignmentsView>(typeof(AssignmentsView).FullName);
            UnityContainer.RegisterTypeForNavigation<AssignmentDetailsView>(typeof(AssignmentDetailsView).FullName);
        }
    }
}
