using System.Windows.Controls;
using CFM.ProfessorModule.ViewModels;
using Microsoft.Practices.Unity;

namespace CFM.ProfessorModule.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class ProfessorsView : UserControl
    {
        public ProfessorsView(IUnityContainer container)
        {
            InitializeComponent();
           //DataContext = container.Resolve<ProfessorsViewModel>();
        }
    }
}
