using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLKunde
    {

        public static string HentPoststed(string postnummer)
        {
            Postnummer postnr = DBPostnummer.HentPoststed(postnummer);
            return  postnr == null ? "UGYLDIG POSTNUMMER" : postnr.Poststed;
        }


        public static bool LeggInn(IEnumerable<Kunde> kunder)
        {
            return DBKunde.LeggInn(kunder);
        }

        public static bool LeggInn(Kunde innKunde)
        {
            return DBKunde.LeggInn(innKunde) != null;
        }
    }
}
