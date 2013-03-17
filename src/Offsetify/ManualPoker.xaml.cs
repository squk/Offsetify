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
using System.Windows.Shapes;
using XDevkit;

namespace Offsetify
{
    /// <summary>
    /// Interaction logic for ManualPoker.xaml
    /// </summary>
    public partial class ManualPoker : Elysium.Controls.Window
    {
        private RealTimeEditing rte;

        public ManualPoker()
        {
            InitializeComponent();
            rte = new RealTimeEditing();
            XboxManager xmb = new XboxManager();
            xdkName.Text = xmb.DefaultConsole;
        }

        private void connectToXDKButton_Click(object sender, RoutedEventArgs e)
        {
            rte = new RealTimeEditing(xdkName.Text);
            bool successfulConnection = rte.Connect();
            if (successfulConnection)
            {
                connectToXDKButton.Content = "Success";
                connectToXDKButton.Background = Brushes.YellowGreen;
                connectToXDKButton.BorderBrush = Brushes.YellowGreen;
            }
            else
            {
                connectToXDKButton.Content = "Failure";
                connectToXDKButton.Background = Brushes.Red;
                connectToXDKButton.BorderBrush = Brushes.Red;
            }
        }

        private void customPokeButton_Click(object sender, RoutedEventArgs e)
        {
            rte.PokeXbox(Convert.ToUInt32(offsetBox.Text, 0x10), (typeBox.SelectedItem as ComboBoxItem).Content.ToString(), valueBox.Text);
        }

        private void customPeekButton_Click(object sender, RoutedEventArgs e)
        {
            valueBox.Text = rte.PeekXbox(Convert.ToUInt32(offsetBox.Text, 0x10), (typeBox.SelectedItem as ComboBoxItem).Content.ToString());
        }

        bool afterFirstRun = false;

        private void xdkName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (afterFirstRun)
            {
                connectToXDKButton.Content = "Connect";
                var converter = new System.Windows.Media.BrushConverter();
                var blueBrush = (Brush)converter.ConvertFromString("#FF3399FF");
                connectToXDKButton.Background = blueBrush;
                connectToXDKButton.BorderBrush = blueBrush;
            }
            afterFirstRun = true;
        }
    }
}
