using S7.Net;
using System;
using System.Collections.Generic;
using System.Text;
using TwistData.Model.Connection;
using TwistData.Model.ReadData;
using TwistData.Model.WriteData;
using TwistecData.Repositories;
using TwistecData.Services.Interfaces;

namespace TwistecData.Services
{
    public class PlcServices : IPlcServices
    {
        public List<PLConnection> GetMachines(TeufelbergerConnection data)
        {
            List<PLConnection> result = new List<PLConnection>();
            var Length = Enum.GetNames(typeof(CpuType)).Length;

            for (int i = 0; i < Length - 1 ; i++)
            {
                result.Add(new PLConnection(CpuType.S71200, data.IPCodes[i]));
            }

            return result;
        }

        public List<Plc> GetPlcs(List<PLConnection> Machines)
        {
            List<Plc> result = new List<Plc>();
            var Length = Machines.Count;
            for (int i = 0; i < Length; i++)
            {
                result.Add(new Plc(Machines[i].CPUType, Machines[i].CurrentIp, Machines[i].Rack, Machines[i].Slot));
            }

            return result;
        }

        public string[] GetCSVData(uint metros, ushort orden, uint RPM, ushort posicion, uint TPM, ushort mmin, uint horas, uint minuts)
        {
            string[] param = new string[8];
            param[0] = metros.ToString();
            param[1] = orden.ToString();
            param[2] = RPM.ToString();
            param[3] = posicion.ToString();
            param[4] = TPM.ToString();
            param[5] = mmin.ToString();
            param[6] = horas.ToString();
            param[7] = minuts.ToString();

            return param;
        }

        public void SaveCSV(string[] param, string path)
        {
            CSVWritter csv = new CSVWritter(path);
            //Open file
            csv.Open();
            //Write param
            csv.Write(param);
            //Close file
            csv.Close();
        }

        public WriteCSV FillCSV(Plc plc, RDBTeufelberger data)
        {
            WriteCSV model = new WriteCSV();
            PLCMethods _plcMethods = new PLCMethods();

            model.FabricationOrder = _plcMethods.FillInt(plc, data.CodeOrder);
            model.CreelPosition = _plcMethods.FillInt(plc, data.CodePosition);
            model.Meters = _plcMethods.FillInt(plc, data.CodeMeters);
            model.ActualRPM = _plcMethods.FillFloat(plc, data.CodeRPM);
            model.ActualTPM = _plcMethods.FillFloat(plc, data.CodeTPM);
            model.Cartoiculo = _plcMethods.FillString(plc, data.ArticleDB, data.ArticlePosition, data.ArticleLenght);
            model.Minutes = _plcMethods.FillDoule(plc, data.CodeMinutes);
            model.Hores = _plcMethods.FillDoule(plc, data.CodeHours);

            return model;
        }

        public TeufelbergerConnection GetPathList()
        {
            TeufelbergerConnection connection = new TeufelbergerConnection();

            connection.MachinePaths.Add(connection.url + connection.name1 + connection.FileType);
            connection.MachinePaths.Add(connection.url + connection.name2 + connection.FileType);
            connection.MachinePaths.Add(connection.url + connection.name3 + connection.FileType);
            connection.MachinePaths.Add(connection.url + connection.name4 + connection.FileType);
            connection.MachinePaths.Add(connection.url + connection.name5 + connection.FileType);

            return connection;

        }
    }
}
