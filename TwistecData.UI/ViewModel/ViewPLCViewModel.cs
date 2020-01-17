using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using S7.Net;
using TwistData.Model.Connection;
using TwistData.Model.ReadData;
using TwistData.Model.WriteData;
using TwistecData.Repositories.Interface;
using TwistecData.Services.Interfaces;
using TwistecData.UI.Views;

namespace TwistecData.UI.ViewModel
{
    public class ViewPLCViewModel : ViewModelBase
    {
        #region Properties

        private int _OrdenFabricacion;
        public int OrdenFrabricacion
        {
            get { return _OrdenFabricacion; }
            set
            {
                if (value != _OrdenFabricacion)
                {
                    _OrdenFabricacion = value;
                    RaisePropertyChanged("Orden de Fabricación");
                }
            }
        }

        private int _creelPosition;
        public int CreelPosition
        {
            get { return _creelPosition; }
            set
            {
                if (value != _creelPosition)
                {
                    _creelPosition = value;
                    RaisePropertyChanged("Creel Position");
                }
            }

        }

        private int _metros;
        public int Metros
        {
            get { return _metros; }
            set
            {
                if (value != _metros)
                {
                    _metros = value;
                    RaisePropertyChanged("Metros");
                }
            }

        }

        private float _actualRPM;
        public float ActualRPM
        {
            get { return _actualRPM; }
            set
            {
                if (value != _actualRPM)
                {
                    _actualRPM = value;
                    RaisePropertyChanged("RPM");
                }
            }

        }

        private float _actualTPM;
        public float ActualTPM
        {
            get { return _actualTPM; }
            set
            {
                if (value != _actualTPM)
                {
                    _actualTPM = value;
                    RaisePropertyChanged("TPM");
                }
            }

        }

        private string _carticulo;
        public string Carticulo
        {
            get { return _carticulo; }
            set
            {
                if (value != _carticulo)
                {
                    _carticulo = value;
                    RaisePropertyChanged("Articulo");
                }
            }

        }

        private double _minuts;
        public double Minuts
        {
            get { return _minuts; }
            set
            {
                if (value != _minuts)
                {
                    _minuts = value;
                    RaisePropertyChanged("Minuts");
                }
            }

        }

        private double _horas;
        public double Horas
        {
            get { return _horas; }
            set
            {
                if (value != _horas)
                {
                    _horas = value;
                    RaisePropertyChanged("Horas");
                }
            }

        }

        private ushort _mmin;
        public ushort Mmin
        {
            get { return _mmin; }
            set
            {
                if (value != _mmin)
                {
                    _mmin = value;
                    RaisePropertyChanged("M Min");
                }
            }
        }

        private string _ip;
        public string IP
        {
            get { return _ip; }
            set
            {
                if (value != _ip)
                {
                    _ip = value;
                    RaisePropertyChanged("IP");
                }
            }

        }

        private CpuType _cpu;
        public CpuType CPU
        {
            get { return _cpu; }
            set
            {
                if (value != _cpu)
                {
                    _cpu = value;
                    RaisePropertyChanged("CPU");
                }
            }

        }

        #region Variables de Escritura
        private int _setMetrosHecho;
        public int SetMetrosHechos
        {
            get { return _setMetrosHecho; }
            set
            {
                if (value != _setMetrosHecho)
                {
                    _setMetrosHecho = value;
                    RaisePropertyChanged("Metros Hechos");
                }
            }


        }

        private int _setOrden;
        public int SetOrden
        {
            get { return _setOrden; }
            set
            {
                if (value != _setOrden)
                {
                    _setOrden = value;
                    RaisePropertyChanged("Set Orden de Fabricación");
                }
            }
        }

        private string _setArticulo;
        public string SetArticulo
        {
            get { return _setArticulo; }
            set
            {
                if (value != _setArticulo)
                {
                    _setArticulo = value;
                    RaisePropertyChanged("Set Articulo");
                }
            }
        }

        private int _setPosicion;
        public int SetPosicion
        {
            get { return _setPosicion; }
            set
            {
                if (value != _setPosicion)
                {
                    _setPosicion = value;
                    RaisePropertyChanged("Set Posicion");
                }
            }
        }

        public string path;

        private readonly string ConnectionError = "PLC connection not found ";

        #endregion

        #region Models

        private readonly PLConnection _pLConnection;
        readonly RDBTeufelberger data = new RDBTeufelberger();
        readonly Plc plc;

        #endregion

        #endregion

        #region Commands

        private readonly RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get { return _refreshCommand; }
        }

        private readonly RelayCommand _exportCommand;
        public RelayCommand ExportCommand
        {
            get { return _exportCommand; }
        }

        private readonly RelayCommand _writteArticleCommand;
        public RelayCommand WritteArticleCommand
        {
            get { return _writteArticleCommand; }
        }

        private readonly RelayCommand _writteOrderCommand;
        public RelayCommand WritteOrderCommand
        {
            get { return _writteOrderCommand; }
        }

        private readonly RelayCommand _writtePositionCommand;
        public RelayCommand WrittePositionCommand
        {
            get { return _writtePositionCommand; }
        }

        private readonly RelayCommand _writteMetersCommand;
        public RelayCommand WritteMetersCommand
        {
            get { return _writteMetersCommand; }
        }

        private readonly RelayCommand _backToMenuCommand;
        public RelayCommand BackToMenuCommand
        {
            get { return _backToMenuCommand; }
        }

        #endregion

        #region Interfaces
        private readonly ICSVReader _csvReader;
        private readonly ICSVWritter _csvWritter;
        private readonly IPLCMethods _plcMethods;
        private readonly IPlcServices _plcServices;
        #endregion

        #region Services
        private MessageViewModel _messageViewModel;
        private MainViewModel _mainViewModel;
        #endregion

        public ViewPLCViewModel(ICSVWritter csvWritter, ICSVReader csvReader, IPLCMethods plcMethods, PLConnection pLConnection, IPlcServices plcServices)
        {
            #region RelayCommands
            _refreshCommand = new RelayCommand(Refresh);
            _exportCommand = new RelayCommand(Export);
            _writteArticleCommand = new RelayCommand(WritteArticle);
            _writteOrderCommand = new RelayCommand(WritteOrder);
            _writtePositionCommand = new RelayCommand(WrittePosition);
            _writteMetersCommand = new RelayCommand(WritteMeters);
            _backToMenuCommand = new RelayCommand(BacktoMenu);
            #endregion

            #region Interfaces
            _csvReader = csvReader;
            _csvWritter = csvWritter;
            _plcMethods = plcMethods;
            _plcServices = plcServices;
            #endregion

            #region Services
            _messageViewModel = new MessageViewModel();
            #endregion

            #region Connection
            _pLConnection = pLConnection;
            //plc = new Plc(pLConnection.CPUType, pLConnection.CurrentIp, pLConnection.Rack, pLConnection.Slot);
            //_plcMethods.StartNewConnection(plc);
            #endregion

            #region Fill Data
            Fill(plc);
            #endregion

            #region ViewsModels
            #endregion

        }

        private void BacktoMenu()
        {
            var view = new MainWindow();
            view.Show();
        }

        private void WrittePosition()
        {
            try
            {
                _plcMethods.WriteShort(plc, SetPosicion, data.CodePosition);
            }
            catch (Exception)
            {

                ShowDialog(ConnectionError);
            }

        }

        private void WritteMeters()
        {
            try
            {
                _plcMethods.WriteInt(plc, SetMetrosHechos, data.CodeMeters);
            }
            catch (Exception)
            {

                ShowDialog(ConnectionError);
            }

        }

        private void WritteOrder()
        {
            try
            {
                _plcMethods.WriteShort(plc, SetOrden, data.CodeOrder);
            }
            catch (Exception)
            {

                ShowDialog(ConnectionError);
            }

        }

        private void WritteArticle()
        {
            var result = _plcMethods.WriteString(plc, SetArticulo, data.CodeArticle, data.ArticleLenght, data.ArticleDB, data.ArticlePosition);
            if (result == false)
            {
                ShowDialog(ConnectionError);
            }

        }

        private void Export()
        {
            try
            {
                WriteCSV writeCSV = new WriteCSV();
                writeCSV = _plcServices.FillCSV(plc, data);
                var param = _plcServices.GetCSVData((uint)writeCSV.Meters, (ushort)writeCSV.FabricationOrder, (uint)writeCSV.ActualRPM, (ushort)writeCSV.CreelPosition, (uint)writeCSV.ActualTPM, (ushort)writeCSV.Mmin, (uint)writeCSV.Hores, (uint)writeCSV.Minutes);
                _plcServices.SaveCSV(param, path);
            }
            catch (Exception)
            {

                ShowDialog(" PLC connection not found ");
            }

        }

        private void Refresh()
        {
            try
            {
                Fill(plc);
            }
            catch (Exception)
            {

                ShowDialog(" Connection error, try again");
            }

        }

        private void Fill(Plc plc)
        {
            try
            {
                OrdenFrabricacion = _plcMethods.FillInt(plc, data.CodeOrder);
                CreelPosition = _plcMethods.FillInt(plc, data.CodePosition);
                Metros = _plcMethods.FillInt(plc, data.CodeMeters);
                ActualRPM = _plcMethods.FillFloat(plc, data.CodeRPM);
                ActualTPM = _plcMethods.FillFloat(plc, data.CodeTPM);
                Carticulo = _plcMethods.FillString(plc, data.ArticleDB, data.ArticlePosition, data.ArticleLenght);
                Minuts = _plcMethods.FillDoule(plc, data.CodeMinutes);
                Horas = _plcMethods.FillDoule(plc, data.CodeHours);
            }
            catch (Exception)
            {
                ShowDialog("Machine " + _pLConnection.CurrentIp + " not connected");
            }
            

        }

        private void ShowDialog(string message)
        {
            var view = new MessageView
            {
                DataContext = _messageViewModel
            };
            _messageViewModel.Message = message;
            view.Show();
            WDBTeufelberger model = new WDBTeufelberger();
        }
    }
}
