using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CFM.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var programDataDir = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\CITY Feedback Manager";
            System.IO.Directory.CreateDirectory(programDataDir);
            AppDomain.CurrentDomain.SetData("DataDirectory", programDataDir);
            Bootstrapper bs = new Bootstrapper();
            bs.Run();
        }
    }
}
