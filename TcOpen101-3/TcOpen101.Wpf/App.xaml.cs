using Serilog;
using System;
using System.Windows;
using TcOpen.Inxton;
using TcOpen.Inxton.Logging;
using TcOpenPlc;
using Vortex.Adapters.Connector.Tc3.Adapter;

namespace TcOpen101.Wpf
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static TcOpenPlcTwinController _plc;

        // Replace with your amsid
        // e.g.: static string AMS_ID = "10.0.0.1.1.1";
        private static string AMS_ID = Environment.GetEnvironmentVariable("Tc3Target");

        private static volatile object mutex = new object();

        /// <summary>
        /// Creates an instance of <see cref="App"/>.
        /// </summary>
        public App() : base()
        {
            // Kicks off twin operations
            Plc.Connector.BuildAndStart().ReadWriteCycleDelay = 250;

            // TcOpen application configuration
            TcoAppDomain.Current.Builder.SetUpLogger(new SerilogAdapter(new LoggerConfiguration()
                                                                        .WriteTo.Console()
                                                                        .WriteTo.Notepad()
                                                                        .MinimumLevel.Verbose()))
                                        .SetDispatcher(new TcoCore.Wpf.Threading.Dispatcher());

            // Starts context's logger
            Plc.MAIN._simpleContext._logger.StartLoggingMessages(TcoCore.eMessageCategory.All, 10);
        }

        /// <summary>
        /// Creates twin connector for runtime or design mode.
        /// </summary>
        /// <returns>Plc twin</returns>
        private static TcOpenPlcTwinController CreateTwin()
        {
            if (!IsInDesign)
            {
                return new TcOpenPlcTwinController(Tc3ConnectorAdapter.Create(AMS_ID, 851, true));
            }
            else
            {
                return new TcOpenPlcTwinController(new Vortex.Connector.ConnectorAdapter(typeof(Vortex.Connector.DummyConnectorFactory)), new object[] { string.Empty });
            }
        }

        /// <summary>
        /// Gets true when running in design mode
        /// </summary>
        private static bool IsInDesign
        {
            get
            {
                return System.ComponentModel.DesignerProperties.GetIsInDesignMode(new DependencyObject());
            }
        }

        /// <summary>
        /// Gets Plc twin for this application.
        /// </summary>
        public static TcOpenPlcTwinController Plc
        {
            get
            {
                if (_plc == null)
                {
                    lock (mutex)
                    {
                        if (_plc == null)
                        {
                            _plc = CreateTwin();
                        }
                    }
                }

                return _plc;
            }
        }
    }
}
