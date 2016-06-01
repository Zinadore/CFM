using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFM.Infrastructure;
using CFM.Infrastructure.Constants;
using CFM.Shell.Navigation;
using CFM.Shell.Views;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace CFM.Shell.ViewModels
{
    public class MainWindowViewModel: BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<object> NavigateCommand { get; private set; } 
        public DelegateCommand GoBackCommand { get; private set; } 

        public MainWindowViewModel(IRegionManager regionManager, IApplicationCommands applicationCommands)
        {
            _regionManager = regionManager;
            NavigateCommand = new DelegateCommand<object>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack, CanGoBack);
            applicationCommands.NavigateCommand.RegisterCommand(NavigateCommand);
            applicationCommands.GoBackCommand.RegisterCommand(GoBackCommand);
            _regionManager.RequestNavigate(RegionNames.MainRegion, typeof(HomeView).FullName);
            _regionManager.RegisterViewWithRegion(RegionNames.NavigationRegion, typeof(HomeButton));
        }

        private bool CanGoBack()
        {
            try
            {
                var region = _regionManager.Regions[RegionNames.MainRegion];
                return region.NavigationService.Journal.CanGoBack;
            }
            catch (Exception)
            {
                
            }
            return false;
        }

        private void GoBack()
        {
            _regionManager.Regions[RegionNames.MainRegion].NavigationService.Journal.GoBack();
        }

        private void Navigate(object path)
        {
            if (path != null)
            {
                _regionManager.RequestNavigate(RegionNames.MainRegion, path.ToString());
                GoBackCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
