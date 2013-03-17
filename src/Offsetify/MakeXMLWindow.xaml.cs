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
using Microsoft.Win32;

namespace Offsetify
{
    public partial class MakeXMLWindow : Elysium.Controls.Window
    {
        private List<Offset> OffsetList = new List<Offset>();
        public MakeXMLWindow()
        {
            InitializeComponent();
            ToggleAllControls(false);
        }

        private void ToggleAllControls(bool state)
        {
            nameBox.IsEnabled = state;
            offsetBox.IsEnabled = state;
            typeBox.IsEnabled = state;
            savedValueBox.IsEnabled = state;
            defaultValueBox.IsEnabled = state;
            notesBox.IsEnabled = state;
            saveCurrentOffsetButton.IsEnabled = state;
        }

        private void offsetAddButton_Click(object sender, RoutedEventArgs e)
        {
            string newName = "New Offset" + OffsetList.Count.ToString();
            ToggleAllControls(true);
            OffsetList.Add(new Offset(newName));
            ComboBoxItem newOffsetCBI = new ComboBoxItem();
            newOffsetCBI.Content = newName;
            nameBox.Text = newName;
            offsetsBox.Items.Add(newOffsetCBI);
            offsetsBox.SelectedIndex++;
        }

        private void saveCurrentOffsetButton_Click(object sender, RoutedEventArgs e)
        {
            if (CheckValidOffset())
            {
                (offsetsBox.SelectedItem as ComboBoxItem).Content = nameBox.Text;

                OffsetList[offsetsBox.SelectedIndex].Name = nameBox.Text;
                OffsetList[offsetsBox.SelectedIndex].memOffset = offsetBox.Text;
                OffsetList[offsetsBox.SelectedIndex].Type = (typeBox.SelectedItem as ComboBoxItem).Content.ToString();
                OffsetList[offsetsBox.SelectedIndex].AssignedValue = savedValueBox.Text;
                OffsetList[offsetsBox.SelectedIndex].DefaultValue = defaultValueBox.Text;
                OffsetList[offsetsBox.SelectedIndex].Notes = GetString(notesBox);
            }
        }

        private bool CheckValidOffset()
        {
            if (nameBox.Text != "")
            {
                return true;
            }
            else
            {
                MessageBox.Show("You must give your offset a name. ");
                return false;
            }

            if (offsetBox.Text != "")
            {
                return true;
            }
            else
            {
                MessageBox.Show("You must enter an offset. ");
                return false;
            }

            if ((typeBox.SelectedItem as ComboBoxItem).Content.ToString() != "")
            {
                return true;
            }
            else
            {
                MessageBox.Show("You must enter a data type. ");
                return false;
            }
        }

        string GetString(RichTextBox rtb)
        {
            var textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text;
        }

        private void offsetsBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            nameBox.Text = OffsetList[offsetsBox.SelectedIndex].Name;
            savedValueBox.Text = OffsetList[offsetsBox.SelectedIndex].AssignedValue;
            defaultValueBox.Text = OffsetList[offsetsBox.SelectedIndex].DefaultValue;
            offsetBox.Text = OffsetList[offsetsBox.SelectedIndex].memOffset;
            typeBox.Text = OffsetList[offsetsBox.SelectedIndex].Type;

            FlowDocument myFlowDoc = new FlowDocument();
            myFlowDoc.Blocks.Add(new Paragraph(new Run(OffsetList[offsetsBox.SelectedIndex].Notes)));
            RichTextBox myRichTextBox = new RichTextBox();
            notesBox.Document = myFlowDoc;
        }

        private void saveCompleteButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog
            {
                DefaultExt = ".xml",
                Filter = "Offsetify XML (.xml)|*.xml"
            };

            if (dlg.ShowDialog() == true)
            {
                if (OffsetXML.WriteOffsetListToXML(dlg.FileName, OffsetList))
                {
                    MessageBox.Show("XML file created successfully. ");
                    this.Close();
                }
            }
        }
    }
}
