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
using Microsoft.Win32;

namespace Offsetify
{
    public partial class MainWindow : Elysium.Controls.Window
    {
        public MainWindow()
        {
            Elysium.Manager.Apply(Application.Current, Elysium.Theme.Dark, Brushes.YellowGreen, Brushes.White);
        }

        private void openXMLFile_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Would you like to edit this XML? (Don't know? hit no) ", "Edit?", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                OpenFileDialog dlg = new OpenFileDialog
                {
                    DefaultExt = ".xml",
                    Filter = "Offsetify XML (.xml)|*.xml"
                };

                if (dlg.ShowDialog() == true)
                {
                    new OffsetWindow(dlg.FileName).Show();
                }
            }
            else
            {
                OpenFileDialog dlg = new OpenFileDialog
                {
                    DefaultExt = ".xml",
                    Filter = "Offsetify XML (.xml)|*.xml"
                };

                if (dlg.ShowDialog() == true)
                {
                    new MakeXMLWindow(OffsetXML.ReadOffsetListFromXML(dlg.FileName)).Show();
                }
            }
            
        }

        private void createFileButton_Click(object sender, RoutedEventArgs e)
        {
            new MakeXMLWindow().Show();
        }

        private void manualPokerButton_Click(object sender, RoutedEventArgs e)
        {
            new ManualPoker().Show();
        }
    }
}
