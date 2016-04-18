using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;

namespace CFM.Infrastructure
{
    public static class ApplicationCommands
    {
        public static CompositeCommand ShowFlyoutCommand = new CompositeCommand();
        public static CompositeCommand NavigateCommand = new CompositeCommand();
        public static CompositeCommand GoBackCommand = new CompositeCommand();
    }

    public interface IApplicationCommands
    {
        CompositeCommand ShowFlyoutCommand { get; }
        CompositeCommand NavigateCommand { get; }
        CompositeCommand GoBackCommand { get; }
    }

    public class ApplicationCommandsProxy : IApplicationCommands
    {
        public CompositeCommand ShowFlyoutCommand
        {
            get { return ApplicationCommands.ShowFlyoutCommand; }
        }

        public CompositeCommand NavigateCommand
        {
            get { return ApplicationCommands.NavigateCommand; }
        }

        public CompositeCommand GoBackCommand
        {
            get { return ApplicationCommands.GoBackCommand; }
        }
    }
}
