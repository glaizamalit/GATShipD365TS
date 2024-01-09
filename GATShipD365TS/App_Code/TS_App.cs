using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GATShipD365TS.App_Code
{
    internal static class TS_App
    {
        public static string errorMessage;
   

        public static void Start()
        {
            Window.Show();
            Console.WriteLine(Config.SystemCode + " Started...");
          

            LogManager.Log.Info(Config.SystemCode + " Started...");
            Window.Hide();
        }

        public static void End()
        {
            Window.Show();
            Console.WriteLine(Config.SystemCode + " Ended...");
            LogManager.Log.Info(Config.SystemCode + " Ended...");
            //if (errorMessage != null || errorMessage !="")
            //{
            //    Console.WriteLine("Please see error message...");
            //    Console.WriteLine(errorMessage);
            //}


            //if (errorMessage.Count() > 0)
            //{

            //    LogManager.Log.Error(Config.SystemCode + " Ended with ff error." + errorMessage);
            //}
            //else
            //{

            //    LogManager.Log.Info(Config.SystemCode + " Ended.");
            //}

            Window.Hide();
        }
    }
}
