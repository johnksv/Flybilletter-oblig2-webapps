using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBKundeStub : IDBKunde
    {

        private List<Kunde> kunder = new List<Kunde>()
        {
            new Kunde()
            {
               Fornavn = "Ola",
               Etternavn ="Nordmann",
               Adresse ="Testveien 1",
               EPost ="test@test.no",
               Fodselsdag = new DateTime(1970,1,1),
               Postnr= "0001",
               Poststed ="Oslo",
               Tlf = "12345678"
            }
    };

        public List<Kunde> HentAlle()
        {
            return kunder;
        }

        public Kunde Hent(int id)
        {
            throw new NotImplementedException();
        }

        public DBKunde LeggInn(Kunde innKunde)
        {
            if (innKunde != null
                && innKunde.Fornavn != null
                && innKunde.Etternavn != null
                && innKunde.Fodselsdag != null
                && innKunde.EPost != null
                && innKunde.Tlf != null
                && innKunde.Postnr != null
                && innKunde.Poststed != null)
            {
                return new DBKunde();
            }
            return null;
        }

        public bool Endre(Kunde innKunde)
        {
            if (innKunde != null
               && innKunde.Fornavn != null
               && innKunde.Etternavn != null
               && innKunde.Fodselsdag != null
               && innKunde.EPost != null
               && innKunde.Tlf != null
               && innKunde.Postnr != null
               && innKunde.Poststed != null)
            {
                return true;
            }
            return false;
        }

        public bool Slett(int id)
        {
            return id > 0;
        }
    }
}
