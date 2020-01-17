using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using TwistecData.Repositories.Interface;
using TwistData.Model.Connection;
using TwistecData.UI.Views;
using S7.Net;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TwistecData.Services.Interfaces;
using System.Windows.Input;
using System.Windows.Threading;
using TwistData.Model.WriteData;
using TwistData.Model.ReadData;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;
using System.ComponentModel;
using RabbitMQ.Client.Impl;
using System.Threading.Tasks;
using System.Linq;

namespace TwistecData.UI.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {

        #region Commands
        private readonly RelayCommand _connectCommand;
        public RelayCommand ConnectCommnad
        {
            get { return _connectCommand; }
        }

        private readonly RelayCommand _viewCommand;
        public RelayCommand ViewCommand
        {
            get { return _viewCommand; }
        }

        DispatcherTimer Timer = new DispatcherTimer();


        private readonly RelayCommand _viewFolder;
        public RelayCommand ViewFolder
        {
            get { return _viewFolder; }
        }

        private readonly RelayCommand _webPage;
        public RelayCommand WebPage
        {
            get { return _webPage; }

        }

        private readonly RelayCommand _resetSaving;
        public RelayCommand ResetSaving
        {
            get { return _resetSaving; }

        }

        #endregion

        #region Interfaces
        private readonly IPLCMethods _plcMethods;
        private readonly ICSVWritter _csvWritter;
        private readonly ICSVReader _csvReader;
        private readonly IPlcServices _plcServices;
        #endregion

        #region Properties

        private string _iptext;
        public string IPText
        {
            get { return _iptext; }
            set
            {
                if (value != _iptext)
                {
                    _iptext = value;
                    RaisePropertyChanged("IP");
                }
            }
        }

        private short _rack;
        public short Rack
        {
            get { return _rack; }
            set
            {
                if (value != _rack)
                {
                    _rack = value;
                    RaisePropertyChanged("Rack");
                }
            }
        }

        private short _slot;
        public short Slot
        {
            get { return _slot; }
            set
            {
                if (value != _slot)
                {
                    _slot = value;
                    RaisePropertyChanged("Slot");
                }
            }
        }

        private CpuType _selectedCPU;
        public CpuType SelectedCPU
        {
            get
            {
                return _selectedCPU;
            }
            set
            {
                if (value != _selectedCPU)
                {
                    _selectedCPU = value;
                    RaisePropertyChanged("Selected CPU");
                }
            }
        }
        private ObservableCollection<CpuType> _cpuBox;
        public ObservableCollection<CpuType> CPUBox
        {
            get
            {
                return _cpuBox;
            }
            set
            {
                if (value != _cpuBox)
                {
                    _cpuBox = value;
                    RaisePropertyChanged("CPU Box");
                }
            }
        }

        TeufelbergerConnection Data = new TeufelbergerConnection();

        private ObservableCollection<string> _machineBox;
        public ObservableCollection<string> MachineBox
        {
            get
            {
                return _machineBox;
            }
            set
            {
                if (value != _machineBox)
                {
                    _machineBox = value;
                    RaisePropertyChanged("Machine Box");
                }
            }
        }

        private string _selectedMachine;
        public string SelectedMachine
        {
            get
            {
                return _selectedMachine;
            }
            set
            {
                if (value != _selectedMachine)
                {
                    _selectedMachine = value;
                    RaisePropertyChanged("Selected Machine");
                }
            }
        }

        private short _machineIndex;
        public short MachineIndex
        {
            get { return _machineIndex; }
            set
            {
                if (value != _machineIndex)
                {
                    _machineIndex = value;
                    RaisePropertyChanged("MachineIndex");
                }
            }
        }

        private string _url;
        public string URL
        {
            get
            {
                return _url;
            }
            set
            {
                if (value != _url)
                {
                    _url = value;
                    RaisePropertyChanged("URL");
                }
            }
        }

        // Global Variables:
        List<PLConnection> Machines;
        List<Plc> plcs;
        List<int> _avoidList;

        #endregion

        #region Services
        private MessageViewModel _messageViewModel;
        private ViewPLCViewModel _viewPLCViewModel;
        #endregion

        #region Model
        TeufelbergerConnection teufelbergerConnection = new TeufelbergerConnection();
        List<WriteCSV> CSVModel = new List<WriteCSV>();
        readonly RDBTeufelberger data = new RDBTeufelberger();
        PLConnection plcConnection;



        #endregion

        public MainViewModel(IPLCMethods plcMethods, IPlcServices plcServices, ICSVWritter csvWritter, ICSVReader csvReader)
        {
            #region Commands
            _connectCommand = new RelayCommand(Connect);
            _viewCommand = new RelayCommand(View);
            _viewFolder = new RelayCommand(NewFolder);
            _webPage = new RelayCommand(OpenWeb);
            _resetSaving = new RelayCommand(Reset);


            #endregion

            #region Interfaces
            _plcMethods = plcMethods;
            _plcServices = plcServices;
            _csvReader = csvReader;
            _csvWritter = csvWritter;
            #endregion

            #region ViewModels
            _messageViewModel = new MessageViewModel();
            #endregion

            #region Methods 
            Fill();
            StartTimer();
            #endregion

        }

        private void Reset()
        {
            StartTimer();
            _avoidList = Enumerable.Repeat(0, teufelbergerConnection.MachinePaths.Count).ToList();
        }

        private void OpenWeb()
        {
            Process.Start("http://www.twistechnology.com");
        }

        private void NewFolder()
        {
            try
            {
                Process.Start(Data.url);
            }
            catch (Exception)
            {

                ShowDialog("URL not found");
            }
        }

        private void View()
        {
            PLConnection connection = new PLConnection(_selectedCPU, _selectedMachine)
            {
                Rack = 2,
                Slot = 0,
            };

            teufelbergerConnection = _plcServices.GetPathList();

            _viewPLCViewModel = new ViewPLCViewModel(_csvWritter, _csvReader, _plcMethods, connection, _plcServices)
            {
                path = teufelbergerConnection.MachinePaths[MachineIndex],
            };

            var view = new ViewPLC
            {
                DataContext = _viewPLCViewModel,
            };

            _viewPLCViewModel.IP = _selectedMachine;
            plcConnection.CurrentIp = _selectedMachine;
            view.Show();

        }

        private void Connect()
        {
            Machines = _plcServices.GetMachines(Data);
            plcs = _plcServices.GetPlcs(Machines);

            for (int i = 0; i < Machines.Count; i++)
            {
                if (_plcMethods.CheckConnection(plcs[i]) == true)
                {
                    _plcMethods.StartNewConnection(plcs[i]);
                }
                else
                {
                    ShowDialog("Connection " + plcs[i].IP + " not found");
                };
            }
        }

        private void Fill()
        {
            plcConnection = new PLConnection(SelectedCPU, SelectedMachine);

            Slot = plcConnection.Slot;
            Rack = plcConnection.Rack;

            CPUBox = new ObservableCollection<CpuType>()
            {
                 CpuType.S71200,
                 CpuType.S71500,
                 CpuType.S7200,
                 CpuType.S7300,
                 CpuType.S7400,
            };
            MachineBox = new ObservableCollection<string>
            {
                Data.IP1,
                Data.IP2,
                Data.IP3,
                Data.IP4,
                Data.IP5,
            };
            IPText = Data.IP1;
        }

        //This function will save each connection to a .csv file on the runtime  folder. If there is no connection in one of them 
        private void SaveData(object state, EventArgs e)
        {
            Machines = _plcServices.GetMachines(Data);
            _avoidList = new List<int>();
            plcs = _plcServices.GetPlcs(Machines);

            for (int i = 0; i < teufelbergerConnection.MachinePaths.Count; i++)
            {
                if (plcs[i].IsConnected)
                {
                    _avoidList.Add(i);
                }
            }

            if (_avoidList.Count >= teufelbergerConnection.MachinePaths.Count)
            {
                for (int i = 0; i < teufelbergerConnection.MachinePaths.Count; i++)
                {
                    if (_avoidList[i] != i)
                    {
                        _plcMethods.StartNewConnection(plcs[i]);
                        teufelbergerConnection = _plcServices.GetPathList();
                        CSVModel.Add(_plcServices.FillCSV(plcs[i], data));
                        var param = _plcServices.GetCSVData((uint)CSVModel[i].Meters, (ushort)CSVModel[i].FabricationOrder, (uint)CSVModel[i].ActualRPM, (ushort)CSVModel[i].CreelPosition, (uint)CSVModel[i].ActualTPM, (ushort)CSVModel[i].Mmin, (uint)CSVModel[i].Hores, (uint)CSVModel[i].Minutes);
                        _plcServices.SaveCSV(param, teufelbergerConnection.MachinePaths[i]);
                    }
                    else
                    {
                        ShowDialog("Machine " + teufelbergerConnection.IPCodes[i] + "is not available");
                    }
                }
            }
            else
            {
                ShowDialog("There are no machines available");
                Timer.Stop();
            }

            

            
        }

        private void StartTimer()
        {
            Timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 1)
            };
            Timer.Tick += SaveData;
            Timer.Start();
        }

        /* private void Test(object state, EventArgs e)
         {
             string[] x = new string[8];
             x[0] = "sfas";
             x[1] = "sdfasd";
             x[2] = "sfas";
             x[3] = "sdfasd";
             x[4] = "sfas";
             x[5] = "sdfasd";
             x[6] = "sfas";
             x[7] = "sdfasd";
             _url = System.AppDomain.CurrentDomain.BaseDirectory + "Docs";
             string access = _url + "\\Test.txt";
             _plcServices.SaveCSV(x, access );
         }*/

        private void ShowDialog(string message)
        {
            var view = new MessageView();
            view.DataContext = _messageViewModel;
            _messageViewModel.Message = message;
            view.Show();
            WDBTeufelberger model = new WDBTeufelberger();
        }


    }
}