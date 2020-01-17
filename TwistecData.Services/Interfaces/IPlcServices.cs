using S7.Net;
using System;
using System.Collections.Generic;
using System.Text;
using TwistData.Model.Connection;
using TwistData.Model.ReadData;
using TwistData.Model.WriteData;
using TwistecData.Repositories;

namespace TwistecData.Services.Interfaces
{
    public interface IPlcServices
    {

        List<Plc> GetPlcs(List<PLConnection> Machines);
        List<PLConnection> GetMachines(TeufelbergerConnection data);

        string[] GetCSVData(uint metros, ushort orden, uint RPM, ushort posicion, uint TPM, ushort mmin, uint horas, uint minuts);

        void SaveCSV(string[] param, string path);

        WriteCSV FillCSV(Plc plc, RDBTeufelberger data);

        TeufelbergerConnection GetPathList();

    }
}
