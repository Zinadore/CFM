using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CFM.Infrastructure.Constants;
using CFM.Infrastructure.Interfaces;
using CFM.UnitModule.ViewModels;
using MahApps.Metro.Controls;
using Microsoft.Practices.Unity;

namespace CFM.UnitModule.Views
{
    /// <summary>
    /// Interaction logic for NewUnitFlyout.xaml
    /// </summary>
    public partial class NewUnitFlyout : Flyout, IFlyoutView
    {
        public NewUnitFlyout(IUnityContainer container)
        {
            InitializeComponent();
            this.DataContext = container.Resolve<NewUnitFlyoutModel>();
        }

        public string FlyoutName => FlyoutNames.NewUnitFlyout;
    }
}
