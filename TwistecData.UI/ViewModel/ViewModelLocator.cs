/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:TwistecData.UI"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using TwistecData.Repositories;
using TwistecData.Repositories.Interface;
using GalaSoft.MvvmLight.Ioc;
using TwistecData.Services.Interfaces;
using TwistecData.Services;

namespace TwistecData.UI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<IPLCMethods, PLCMethods>();
            SimpleIoc.Default.Register<IPlcServices, PlcServices>();
            SimpleIoc.Default.Register<CSVWritter>(() =>
            {
                return new CSVWritter(System.AppDomain.CurrentDomain.BaseDirectory + "SavedDocs");
            });

            SimpleIoc.Default.Register<ICSVWritter>(() => {
                return SimpleIoc.Default.GetInstance<CSVWritter>();

            });

            SimpleIoc.Default.Register<CSVReader>(() =>
            {
                return new CSVReader(System.AppDomain.CurrentDomain.BaseDirectory + "SavedDocs");
            });

            SimpleIoc.Default.Register<ICSVReader>(() => {
                return SimpleIoc.Default.GetInstance<CSVReader>();
            });
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ViewPLCViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public ViewPLCViewModel ViewPLC
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ViewPLCViewModel>();
            }
        }

        public MessageViewModel MessageView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MessageViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}