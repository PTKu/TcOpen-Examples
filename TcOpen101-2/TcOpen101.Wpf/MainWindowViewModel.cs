using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TcOpen.Inxton;
using TcOpen.Inxton.Logging;
using TcOpenPlc;
using Vortex.Adapters.Connector.Tc3.Adapter;

namespace TcOpen101.Wpf
{
    public class MainWindowViewModel
    {
        // Replace with your amsid
        // e.g.: static string AMS_ID = "10.0.0.1.1.1";
        static string AMS_ID = Environment.GetEnvironmentVariable("Tc3Target");

        public MainWindowViewModel()
        {
            // Create instance of PLC twin
            Plc = new TcOpenPlcTwinController(Tc3ConnectorAdapter.Create(AMS_ID, 851, true));
            // Kiks off twin operations
            Plc.Connector.BuildAndStart().ReadWriteCycleDelay = 250;

            // TcOpen application configuartion
            TcoAppDomain.Current.Builder.SetUpLogger(new SerilogAdapter(new LoggerConfiguration()
                                                                        .WriteTo.Console()
                                                                        .WriteTo.Notepad()
                                                                        .MinimumLevel.Verbose()))
                                        .SetDispatcher(new TcoCore.Wpf.Threading.Dispatcher());

            // Starts context's logger
            Plc.MAIN._simpleContext._logger.StartLoggingMessages(TcoCore.eMessageCategory.All, 10);
        }

        /// <summary>
        /// Gets the plc twin.
        /// </summary>
        public TcOpenPlcTwinController Plc { get; }
    }
}
