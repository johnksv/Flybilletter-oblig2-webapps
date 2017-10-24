using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    internal class DALsetup
    {
        public static void LogFeilTilFil(string metodeNavn, Exception e)
        {
            //For å sende med metodenavn, send denne koden som parameter: "System.Reflection.MethodBase.GetCurrentMethod().Name"
            try
            {

                StreamWriter writer = new System.IO.StreamWriter(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\DAL\flybilletter-log.txt"), true);
                writer.WriteLine(DateTime.Now + ": " + metodeNavn + " - " + e.InnerException);
                writer.Close();
            }catch (Exception ex)
            {
                //TODO: HVa skal vi gjøre her??
            }
        }
    }
}
