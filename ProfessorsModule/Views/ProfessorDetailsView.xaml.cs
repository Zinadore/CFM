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
using CFM.ProfessorModule.ViewModels;
using Microsoft.Practices.Unity;
using Prism.Regions;

namespace CFM.ProfessorModule.Views
{
    /// <summary>
    /// Interaction logic for ProfessorDetails.xaml
    /// </summary>
    public partial class ProfessorDetailsView : UserControl, IRegionMemberLifetime, INavigationAware
    {
        public ProfessorDetailsView(IUnityContainer container)
        {
            InitializeComponent();
            //this.DataContext = container.Resolve<ProfessorDetailsViewModel>();
        }

        public bool KeepAlive => false;
        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var vm = DataContext as ProfessorDetailsViewModel;
            vm.LoadData(navigationContext.Parameters["profId"]);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
