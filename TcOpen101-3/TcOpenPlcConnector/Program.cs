using System;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using TcOpen.Inxton;
using TcOpen.Inxton.Logging;
using TcOpenPlc;
using Vortex.Adapters.Connector.Tc3.Adapter;

namespace TcOpenPlcConnector
{
    class Program
    {
        static string AMS_ID = Environment.GetEnvironmentVariable("Tc3Target");
        static void Main(string[] args)
        {
            var plc = new TcOpenPlcTwinController(Tc3ConnectorAdapter.Create(AMS_ID, 851, true));
            plc.Connector.BuildAndStart();

            TcoAppDomain.Current.Builder.SetUpLogger(new SerilogAdapter(new LoggerConfiguration()
                                                                        .WriteTo.Console()
                                                                        .WriteTo.Notepad()));
                                                                   
            plc.MAIN._simpleContext._logger.StartLoggingMessages(TcoCore.eMessageCategory.All, 10);

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
