using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.DAL.Stub;

namespace BLL
{
    public class BLLKunde : IBLLKunde
    {
        private IDBKunde dbkunde;
        private IDBPostnummer dbPostnummer;
        private DBKundeStub dBKundeStub;

        public BLLKunde()
        {
            dbkunde = new DBKunde();
            dbPostnummer = new DBPostnummer();
        }

        public BLLKunde(IDBKunde stub, IDBPostnummer postnummerStub)
        {
            this.dbkunde = stub;
            this.dbPostnummer = postnummerStub;
        }

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
