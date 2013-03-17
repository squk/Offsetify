using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XDevkit;
using HaloDevelopmentExtender;
using HaloReach3d;
using System.Windows;

namespace Offsetify
{
    class RealTimeEditing
    {
        private string xdkName;
        public bool isConnected;

        public RealTimeEditing()
        {
            isConnected = false;
        }

        public RealTimeEditing(string XdkName)
        {
            xdkName = XdkName;
        }

        public bool Connect()
        {
            XboxDebugCommunicator Xbox_Debug_Communicator = new XboxDebugCommunicator(xdkName);
            if (!Xbox_Debug_Communicator.Connected)
            {
                try
                {
                    Xbox_Debug_Communicator.Connect();
                    isConnected = true;
                    return true;
                }
                catch
                {
                    isConnected = false;
                    return false;
                }
            }
            else
            {
                isConnected = true;
                return true;
            }
        }

        public void PokeXbox(uint offset, string poketype, string amount)
        {
            if (isConnected)
            {
                try
                {
                    if (xdkName == "")
                    {
                        MessageBox.Show("XDK Name/IP not set");
                    }
                    else
                    {
                        XboxDebugCommunicator Xbox_Debug_Communicator = new XboxDebugCommunicator(xdkName);
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
            else
            {
                MessageBox.Show("You are not connected to your XDK");
            }
        }

        public string PeekXbox(uint offset, string type)
        {
            string hex = "X";
            object rn = null;
            if (xdkName != "")
            {
                XboxDebugCommunicator Xbox_Debug_Communicator = new XboxDebugCommunicator(xdkName);
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
    }
}
