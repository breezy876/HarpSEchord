using Hscm.UI.Notifications.Tracks;
using Hscm.UI.ViewModels;
using Hscm.UI.ViewModels.Settings;
using GalaSoft.MvvmLight.Messaging;
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

namespace Hscm.UI
{
    /// <summary>
    /// Interaction logic for InstrumentPanel.xaml
    /// </summary>
    public partial class InstrumentPanel : UserControl
    {
        public InstrumentPanel()
        {
            InitializeComponent();


        }

        private InstrumentPanelViewModel viewModel;


        public void Initialize(InstrumentPanelViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.DataContext = this.viewModel;


        }

    }
}
