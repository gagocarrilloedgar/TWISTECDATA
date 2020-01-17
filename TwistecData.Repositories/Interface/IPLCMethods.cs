using S7.Net;
using System;
using System.Collections.Generic;
using System.Text;
using TwistData.Model.Connection;
using TwistData.Model.ReadData;
using TwistData.Model.WriteData;

namespace TwistecData.Repositories.Interface
{
    public interface IPLCMethods
    {
        short FillShort(Plc plc,string code);
        int FillInt(Plc plc, string code);
        float FillFloat(Plc plc, string code);
        double FillDoule(Plc plc, string code);
        string FillString(Plc plc, int DB, int position, int lenght);
        uint FillUint(Plc plc, string code);
        bool CheckTextBox();
        bool CheckIP(string IP, string emptyvalue);
        bool WriteShort(Plc plc,int data, string connection);
        Byte[] FillEmptySpaces(string mystring, int stringlenght);
        bool CheckConnection(Plc plc);
        void StartNewConnection(Plc plc);
        void StopConnection(Plc plc);
        string ExtractMachineName(TeufelbergerConnection connectionData, string IP);
        bool WriteInt(Plc plc, int data,string connection);
        bool WriteFloat(Plc plc, int data, string connection);
        bool WriteString(Plc plc, string data, string connection, int lenght, int DBNumber, int DBPosition);
    }
}
