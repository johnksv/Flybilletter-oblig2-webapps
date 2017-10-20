using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLKunde : IBLLKunde
    {
        private DBKunde dbkunde = new DBKunde();
        private DBPostnummer dbPostnummer = new DBPostnummer();

        public string HentPoststed(string postnummer)
        {
            Postnummer postnr = dbPostnummer.HentPoststed(postnummer);
            return  postnr == null ? "UGYLDIG POSTNUMMER" : postnr.Poststed;
        }


        public bool LeggInn(IEnumerable<Kunde> kunder)
        {
            return dbkunde.LeggInn(kunder);
        }

        public bool LeggInn(Kunde innKunde)
        {
            return dbkunde.LeggInn(innKunde) != null;
        }
    }
}
