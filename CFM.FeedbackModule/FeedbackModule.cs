using Bulldog.FlyoutManager;
using CFM.FeedbackModule.Views;
using CFM.Infrastructure.Base;
using CFM.Infrastructure.Constants;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Prism.Unity;

namespace CFM.FeedbackModule
{
    public class FeedbackModule: PrismModuleBase
    {
        private readonly IFlyoutManager _flyoutManager;

        public FeedbackModule(IRegionManager regionManager, IUnityContainer unityContainer, IFlyoutManager flyoutManager) 
            : base(regionManager, unityContainer)
        {
            _flyoutManager = flyoutManager;
        }

        public override void Initialize()
        {
            base.Initialize();
            RegisterViews();
            _flyoutManager.RegisterFlyout<FeedbackFlyoutView>(FlyoutNames.FeedbackFlyout, RegionNames.FlyoutRegion);
        }

        protected override void RegisterViews()
        {
            base.RegisterViews();
            UnityContainer.RegisterTypeForNavigation<FeedbackDetailsView>(ViewNames.FeedbackDetailsView);
        }
    }
}
