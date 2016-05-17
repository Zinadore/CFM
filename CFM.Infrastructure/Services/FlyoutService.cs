using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Bulldog.FlyoutManager;
using CFM.Infrastructure.Constants;
using CFM.Infrastructure.Interfaces;
using MahApps.Metro.Controls;
using Prism.Commands;
using Prism.Regions;

namespace CFM.Infrastructure.Services
{
    public class FlyoutService: IFlyoutService
    {
        private readonly IFlyoutManager _flyoutManager;
        public ICommand ShowFlyoutCommand { get; private set; }

        public FlyoutService(IApplicationCommands applicationCommands, IFlyoutManager flyoutManager)
        {
            _flyoutManager = flyoutManager;
            ShowFlyoutCommand = new DelegateCommand<string>(ShowFlyout, CanShowFlyout);
            applicationCommands.ShowFlyoutCommand.RegisterCommand(ShowFlyoutCommand);
        }

        public void ShowFlyout(string flyoutName)
        {
            _flyoutManager.OpenFlyout(flyoutName);
        }

        public bool CanShowFlyout(string flyoutName)
        {
            return true;
        }
    }
}
