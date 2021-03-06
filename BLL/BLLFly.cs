﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;
using Flybilletter.DAL.Interfaces;
using Flybilletter.DAL.DBModel;
using System.Diagnostics.CodeAnalysis;

namespace BLL
{
    public class BLLFly : IBLLFly
    {
        private IDBFly dbFly;

        [ExcludeFromCodeCoverage]
        public BLLFly()
        {
            dbFly = new DBFly();
        }

        public BLLFly(IDBFly stub)
        {
            dbFly = stub;
        }

        public List<Fly> HentAlle()
        {
            return dbFly.HentAlle();
        }

        public bool LeggTil(Fly fly)
        {
            return dbFly.LeggInn(fly);
        }

        public bool Endre(Fly fly)
        {
            return dbFly.Endre(fly);
        }

        public bool Slett(int ID)
        {
            return dbFly.Slett(ID);
        }
    }
}
