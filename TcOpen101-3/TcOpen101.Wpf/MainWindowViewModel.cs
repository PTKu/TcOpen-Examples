using TcOpenPlc;

namespace TcOpen101.Wpf
{
    public class MainWindowViewModel
    {              
        public TcOpenPlcTwinController Plc { get { return App.Plc; } }        
    }
}
