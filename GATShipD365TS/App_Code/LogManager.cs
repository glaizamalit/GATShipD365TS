using log4net;

namespace GATShipD365TS.App_Code
{
    public static class LogManager
    {
        public static readonly ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}
