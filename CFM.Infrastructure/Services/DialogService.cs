using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CFM.Infrastructure.Constants;
using CFM.Infrastructure.Interfaces;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Unity;

namespace CFM.Infrastructure.Services
{
    public class DialogService: IDialogService
    {
        public MetroWindow MainWindow { get; private set; }
        public DialogService(IUnityContainer container)
        {
            MainWindow = container.Resolve<Window>(WindowNames.MainWindowName) as MetroWindow;
        }
        public async Task<MessageDialogResult> ShowMessageAsnyc(string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative,
            MetroDialogSettings settings = null)
        {
            this.MainWindow.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;

            return await this.MainWindow.ShowMessageAsync(title, message, style, this.MainWindow.MetroDialogOptions);
        }
    }
}
