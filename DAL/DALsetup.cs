using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DALsetup
    {
        public static void LogFeilTilFil(string metodeNavn, Exception e, string message)
        {
            //For å sende med metodenavn, send denne koden som parameter: "System.Reflection.MethodBase.GetCurrentMethod().Name"
            try
            {

                StreamWriter writer = new System.IO.StreamWriter(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\DAL\flybilletter-log.txt"), true);
                writer.WriteLine(DateTime.Now + ": " + metodeNavn + " - " + message + " - " + e.Message + e.InnerException);
                writer.Close();
            }catch (Exception ex)
            {
                //TODO: HVa skal vi gjøre her??
            }
        }

        internal static void rensFil()
        {
            try
            {
                if (!File.Exists(@"..\DAL\flybilletter-log.txt"))
                {
                    File.Create(@"..\DAL\flybilletter-log.txt");
                }
                else
                {
                    StreamWriter writer = new System.IO.StreamWriter(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, @"..\DAL\flybilletter-log.txt"), true);
                    File.WriteAllLines(@"..\DAL\flybilletter-log.txt", new string[] { "" });
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                //TODO: HVa skal vi gjøre her??
            }
        }
    }
}
