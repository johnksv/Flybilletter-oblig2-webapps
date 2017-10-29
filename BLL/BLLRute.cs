using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;
using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.ViewModel;
using System.Diagnostics.CodeAnalysis;

namespace BLL
{
    public class BLLRute : IBLLRute
    {
        private IDBRute dbRute;

        [ExcludeFromCodeCoverage]
        public BLLRute() : this(new DBRute())
        {
        }

        public BLLRute(IDBRute rutestub)
        {
            this.dbRute = rutestub;
        }

        public List<Rute> HentAlle()
        {
            return dbRute.HentAlle();
        }

        public bool Endre(Rute rute)
        {
            return dbRute.Endre(rute);
        }

        public bool LeggInn(NyRuteViewModel rute)
        {
            Rute nyRute = new Rute()
            {
                Fra = new Flyplass()
                {
                    ID = rute.FraFlyplassID
                },
                Til = new Flyplass()
                {
                    ID = rute.TilFlyplassID
                },
                Reisetid = rute.Reisetid,
                BasePris = rute.Basepris
            };

            return dbRute.LeggInn(nyRute);
        }

        public bool Slett(int id)
        {
            return dbRute.Slett(id);
        }
    }
}
