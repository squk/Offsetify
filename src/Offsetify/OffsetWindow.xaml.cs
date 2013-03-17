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
using HaloDevelopmentExtender;
using HaloReach3d;
using System.IO;

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

        public OffsetWindow(string filename)
        {
            InitializeComponent();
            this.Title = new FileInfo(filename).Name;
            XboxManager xmb = new XboxManager();
            xdkName.Text = xmb.DefaultConsole;
            OffsetXML offsetXML = new OffsetXML(filename);
            OffsetList = offsetXML.OffsetList;
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
            pokeXbox(Convert.ToUInt32(CurrentOffset, 0x10), CurrentType, CurrentAssigned);
        }     

        private void defaultPokeButton_Click(object sender, RoutedEventArgs e)
        {
            pokeXbox(Convert.ToUInt32(CurrentOffset, 0x10), CurrentType, CurrentDefault);
        }

        private void customPokeButton_Click(object sender, RoutedEventArgs e)
        {
            pokeXbox(Convert.ToUInt32(CurrentOffset, 0x10), CurrentType, customValueBox.Text);
        }

        private void customPeekButton_Click(object sender, RoutedEventArgs e)
        {
            customValueBox.Text = getValue(Convert.ToUInt32(CurrentOffset, 0x10), CurrentType);
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

        public string getValue(uint offset, string type)
        {
            string hex = "X";
            object rn = null;
            if (xdkName.Text != "")
            {
                XboxDebugCommunicator Xbox_Debug_Communicator = new XboxDebugCommunicator(xdkName.Text);
                if (!Xbox_Debug_Communicator.Connected)
                {
                    try
                    {
                        Xbox_Debug_Communicator.Connect();
                    }
                    catch
                    {
                    }
                }
                XboxMemoryStream xbms = Xbox_Debug_Communicator.ReturnXboxMemoryStream();
                HaloReach3d.IO.EndianIO IO = new HaloReach3d.IO.EndianIO(xbms, HaloReach3d.IO.EndianType.BigEndian);
                IO.Open();
                IO.In.BaseStream.Position = offset;
                if ((type == "String") | (type == "string"))
                {
                    rn = IO.In.ReadString();
                }
                if ((type == "Float") | (type == "float"))
                {
                    rn = IO.In.ReadSingle();
                }
                if ((type == "Double") | (type == "double"))
                {
                    rn = IO.In.ReadDouble();
                }
                if ((type == "Short") | (type == "short"))
                {
                    rn = IO.In.ReadInt16().ToString(hex);
                }
                if ((type == "Byte") | (type == "byte"))
                {
                    rn = IO.In.ReadByte().ToString(hex);
                }
                if ((type == "Long") | (type == "long"))
                {
                    rn = IO.In.ReadInt32().ToString(hex);
                }
                if ((type == "Quad") | (type == "quad"))
                {
                    rn = IO.In.ReadInt64().ToString(hex);
                }
                IO.Close();
                xbms.Close();
                Xbox_Debug_Communicator.Disconnect();
                return rn.ToString();
            }
            MessageBox.Show("XDK Name/IP not set");
            return "No Console Detected";
        }

        public void pokeXbox(uint offset, string poketype, string amount)
        {
            try
            {
                if (xdkName.Text == "")
                {
                    MessageBox.Show("XDK Name/IP not set");
                }
                else
                {
                    XboxDebugCommunicator Xbox_Debug_Communicator = new XboxDebugCommunicator(xdkName.Text);
                    if (!Xbox_Debug_Communicator.Connected)
                    {
                        try
                        {
                            Xbox_Debug_Communicator.Connect();
                        }
                        catch
                        {
                        }
                    }
                    XboxMemoryStream xbms = Xbox_Debug_Communicator.ReturnXboxMemoryStream();
                    HaloReach3d.IO.EndianIO IO = new HaloReach3d.IO.EndianIO(xbms, HaloReach3d.IO.EndianType.BigEndian);
                    IO.Open();
                    IO.Out.BaseStream.Position = offset;
                    if (poketype == "Unicode String")
                    {
                        IO.Out.WriteUnicodeString(amount, amount.Length);
                    }
                    if (poketype == "ASCII String")
                    {
                        IO.Out.WriteUnicodeString(amount, amount.Length);
                    }
                    if ((poketype == "String") | (poketype == "string"))
                    {
                        IO.Out.Write(amount);
                    }
                    if ((poketype == "Float") | (poketype == "float"))
                    {
                        IO.Out.Write(float.Parse(amount));
                    }
                    if ((poketype == "Double") | (poketype == "double"))
                    {
                        IO.Out.Write(double.Parse(amount));
                    }
                    if ((poketype == "Short") | (poketype == "short"))
                    {
                        IO.Out.Write((short)Convert.ToUInt32(amount, 0x10));
                    }
                    if ((poketype == "Byte") | (poketype == "byte"))
                    {
                        IO.Out.Write((byte)Convert.ToUInt32(amount, 0x10));
                    }
                    if ((poketype == "Long") | (poketype == "long"))
                    {
                        IO.Out.Write((long)Convert.ToUInt32(amount, 0x10));
                    }
                    if ((poketype == "Quad") | (poketype == "quad"))
                    {
                        IO.Out.Write((long)Convert.ToUInt64(amount, 0x10));
                    }
                    if ((poketype == "Int") | (poketype == "int"))
                    {
                        IO.Out.Write(Convert.ToUInt32(amount, 0x10));
                    }
                    IO.Close();
                    xbms.Close();
                    Xbox_Debug_Communicator.Disconnect();
                }
            }
            catch
            {
                MessageBox.Show("Couldn't Poke XDK");
            }
        }
    }
}
