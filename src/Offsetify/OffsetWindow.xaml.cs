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
using System.IO;
using XDevkit;

namespace Offsetify
{
    /// <summary>
    /// Interaction logic for OffsetWindow.xaml
    /// </summary>
    public partial class OffsetWindow : Elysium.Controls.Window
    {
        private string currentAssigned;
        private string currentDefault;
        private string currentName;
        private string CurrentOffset;
        private string CurrentType;
        #region accessor
        public string CurrentAssigned
        {
            get
            {
                return currentAssigned;
            }
            set
            {
                currentAssigned = value;
                assignedValueBox.Text = value;
            }
        }

        public string CurrentDefault
        {
            get
            {
                return currentDefault;
            }
            set
            {
                currentDefault = value;
                defaultValueBox.Text = value;
            }
        }

        public string CurrentName
        {
            get
            {
                return currentName;
            }
            set
            {
                currentName = value;
                offsetNameBlock.Text = value;
            }
        }
        #endregion

        private List<Offset> OffsetList;

        private RealTimeEditing rte;

        public OffsetWindow(string filename)
        {
            InitializeComponent();

            rte = new RealTimeEditing();

            this.Title = new FileInfo(filename).Name;
            XboxManager xmb = new XboxManager();
            xdkName.Text = xmb.DefaultConsole;
            OffsetList = OffsetXML.ReadOffsetListFromXML(filename);
            foreach (Offset offset in OffsetList)
            {
                ComboBoxItem offsetComboBoxItem = new ComboBoxItem
                {
                    Content = offset.Name
                };
                offsetsBox.Items.Add(offsetComboBoxItem);
                offsetsBox.SelectedIndex = 0;
            }
        }

        private void assignedPokeButton_Click(object sender, RoutedEventArgs e)
        {
            rte.PokeXbox(Convert.ToUInt32(CurrentOffset, 0x10), CurrentType, CurrentAssigned);
        }     

        private void defaultPokeButton_Click(object sender, RoutedEventArgs e)
        {
            rte.PokeXbox(Convert.ToUInt32(CurrentOffset, 0x10), CurrentType, CurrentDefault);
        }

        private void customPokeButton_Click(object sender, RoutedEventArgs e)
        {
            rte.PokeXbox(Convert.ToUInt32(CurrentOffset, 0x10), CurrentType, customValueBox.Text);
        }

        private void customPeekButton_Click(object sender, RoutedEventArgs e)
        {
            customValueBox.Text = rte.PeekXbox(Convert.ToUInt32(CurrentOffset, 0x10), CurrentType);
        }

        private void offsetsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentName = OffsetList[offsetsBox.SelectedIndex].Name;
            CurrentOffset = OffsetList[offsetsBox.SelectedIndex].memOffset;
            CurrentType = OffsetList[offsetsBox.SelectedIndex].Type;
            dataTypeBlock.Text = CurrentType + " at " + CurrentOffset;
            CurrentDefault = OffsetList[offsetsBox.SelectedIndex].DefaultValue;
            CurrentAssigned = OffsetList[offsetsBox.SelectedIndex].AssignedValue;

            FlowDocument myFlowDoc = new FlowDocument();
            myFlowDoc.Blocks.Add(new Paragraph(new Run(OffsetList[offsetsBox.SelectedIndex].Notes)));
            RichTextBox myRichTextBox = new RichTextBox();
            notesBox.Document = myFlowDoc;
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

        bool afterFirstRun;

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
