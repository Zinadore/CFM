using Bulldog.FlyoutManager;
using CFM.Infrastructure.Base;
using CFM.Infrastructure.Constants;
using CFM.ProfessorModule.ViewModels;
using CFM.ProfessorModule.Views;
using Microsoft.Practices.Unity;
using Prism.Regions;
using Prism.Unity;
using ProfessorModule.Navigation;

namespace CFM.ProfessorModule
{
    public class ProfessorModule: PrismModuleBase
    {
        private readonly IFlyoutManager _flyoutManager;

        public ProfessorModule(IRegionManager regionManager, IUnityContainer unityContainer, IFlyoutManager flyoutManager) 
            : base(regionManager, unityContainer)
        {
            _flyoutManager = flyoutManager;
            regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(ProfessorButton));
            RegisterViews();
            RegisterViewModels();
        }

        public override void Initialize()
        {
            base.Initialize();
            _flyoutManager.RegisterFlyout<NewProfessorFlyoutView>(FlyoutNames.NewProfessorFlyout, RegionNames.FlyoutRegion);
        }

        protected override void RegisterViews()
        {
            base.RegisterViews();
            UnityContainer.RegisterTypeForNavigation<ProfessorsView>(typeof(ProfessorsView).FullName);
            UnityContainer.RegisterTypeForNavigation<ProfessorDetailsView>(typeof(ProfessorDetailsView).FullName);
        }

        protected override void RegisterViewModels()
        {
            base.RegisterViewModels();
        }
    }
}
