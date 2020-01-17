using System;
using System.Collections.Generic;
using System.Text;
using S7.Net;
using TwistData.Model.Connection;
using TwistData.Model.ReadData;
using TwistData.Model.WriteData;
using TwistecData.Repositories.Interface;

namespace TwistecData.Repositories
{
    public class PLCMethods:IPLCMethods
    {
    
        public bool CheckConnection(Plc plc)
        {
            bool Isconnected;
            try
            {
                plc.Open();
                Isconnected = true;
            }
            catch (Exception)
            {

                Isconnected = false;
            }
              
            return Isconnected;
        }

        public bool CheckIP(string IP,string emptyvalue)
        {
            string Trim = IP.Replace(".", emptyvalue);
            bool result = false;

            for (int i = 0; i < Trim.Length; i++)
            {
                if (char.IsDigit(Trim[i]))
                {
                    result = true;
                }
            }

            return result;
        }

        public bool CheckTextBox()
        {
            throw new NotImplementedException();
        }

        public string ExtractMachineName(TeufelbergerConnection connectionData, string IP)
        {
            string machineName;

            if (IP == connectionData.IP1)
            {
                machineName = connectionData.name1;
            }
            else if(IP == connectionData.IP2)
            {
                machineName = connectionData.name2;
            }
            else if (IP == connectionData.IP3)
            {
                machineName = connectionData.name3;
            }
            else if (IP == connectionData.IP4)
            {
                machineName = connectionData.name4;
            }
            else
            {
                machineName = connectionData.name5;
            }
            
            return machineName;
        }

        public double FillDoule(Plc plc, string code)
        {
            return Math.Round(((uint)plc.Read(code)).ConvertToFloat());
        }

        public Byte[] FillEmptySpaces(string mystring,int stringlenght)
        {
            Byte[] result;
            int filling = stringlenght - mystring.Length;
            string finalString = new String(' ', filling);
            result = Encoding.UTF8.GetBytes(finalString);

            return result;
        }

        public float FillFloat(Plc plc, string code)
        {
            return (((uint)plc.Read(code)).ConvertToFloat());
        }

        public int FillInt(Plc plc, string code)
        {
            return ((ushort)plc.Read(code)).ConvertToShort();
        }

        public short FillShort(Plc plc, string code)
        {
            return ((ushort)plc.Read(code)).ConvertToShort();
        }

        public string FillString(Plc plc, int DB, int position, int lenght)
        {
            return Encoding.UTF8.GetString(plc.ReadBytes(DataType.DataBlock, DB, position, lenght)).Trim(' ');
        }

        public void FillUint(Plc plc, string code)
        {
            ((uint)plc.Read("DB100.DBD10")).ConvertToInt();
        }

        public void StartNewConnection(Plc plc)
        {
            plc.Open();
        }

        public void StopConnection(Plc plc)
        {
            plc.Close();
        }

        public bool WriteFloat(Plc plc, int data, string connection)
        {
            throw new NotImplementedException();
        }

        public bool WriteInt(Plc plc, int data, string connection)
        {
            bool WriteGood;
            try
            {
                plc.Write(connection, (ushort)data);
                WriteGood = true;
            }
            catch (Exception)
            {

                WriteGood = false;
            }

            return WriteGood;
        }

        public bool WriteShort(Plc plc, int data, string connection)
        {
            bool WriteGood;
            try
            {
                plc.Write(connection, (ushort)data);
                WriteGood = true;
            }
            catch (Exception)
            {

                WriteGood = false;
            }

            return WriteGood;
        }

        public bool WriteString(Plc plc, string data, string connection,int lenght,int DBNumber,int DBPosition)
        {
            bool result;
            try
            {
                plc.WriteBytes(DataType.DataBlock, DBNumber, DBPosition, FillEmptySpaces(data, lenght));

                result = true;
            }
            catch (Exception)
            {

                result = false; 
            }

            return result;
        }

        uint IPLCMethods.FillUint(Plc plc, string code)
        {
            throw new NotImplementedException();
        }
    }
}
