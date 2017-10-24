﻿using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.DAL.Interfaces
{
    public interface IDBFly
    {
        List<Fly> HentAlle();
        bool Oppdater(Fly fly);
        Fly Hent(int iD);
    }
}
