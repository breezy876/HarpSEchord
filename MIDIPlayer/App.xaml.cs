using Hscm.UI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Hscm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static MainWindow Window;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;

            // Initialize main window and view model
            Window = new MainWindow();
            ////var viewModel = new MainWindowViewModel();
            //mainWindow.DataContext = viewModel;

            Window.Show();
        }
    }
}
