using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;
using DAL;
using System.IO;
using System.Text.RegularExpressions;

namespace BLL
{
    public class BLLEndring : IBLLEndring
    {
        private IDBEndring dbEndring;

        public BLLEndring() : this(new DBEndring())
        {
        }

        public BLLEndring(IDBEndring endringStub)
        {
            dbEndring = endringStub;
        }

        public List<Endring> Hent()
        {
            return dbEndring.HentAlle();
        }

        public List<FeilFraFilViewModel> ParseFeil(string filePath)
        {

            List<FeilFraFilViewModel> feil = new List<FeilFraFilViewModel>();
            try
            {
                //FileInfo fi = new FileInfo(filePath);
                StreamReader read = File.OpenText(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filePath));
                string line;
                while ((line = read.ReadLine()) != null)
                {
                    string[] parts = Regex.Split(line, ": ");
                    FeilFraFilViewModel feilObjekt = new FeilFraFilViewModel
                    {
                        Tidspunkt = parts[0],
                        Feilmelding = parts[1]
                    };
                    feil.Add(feilObjekt);
                }
            }
            catch (Exception e)
            {
                DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil skjedde når metoden prøvde å tolke linjene i " + filePath);
            }
            return feil;
        }
    }

}
