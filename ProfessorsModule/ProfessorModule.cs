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
        public ProfessorModule(IRegionManager regionManager, IUnityContainer unityContainer) 
            : base(regionManager, unityContainer)
        {
            regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(ProfessorButton));
            regionManager.RegisterViewWithRegion(RegionNames.FlyoutRegion, typeof(NewProfessorFlyout));
            RegisterViews();
            RegisterViewModels();
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
            //UnityContainer.RegisterType<ProfessorsViewModel>();
            UnityContainer.RegisterType<NewProfessorFlyoutModel>();
            //UnityContainer.RegisterType<ProfessorDetailsViewModel>();
        }
    }
}
