﻿using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.DAL.Interfaces
{
    public interface IDBBestilling
    {

        Bestilling Hent(string referanse);
        void LeggInn(Bestilling bestilling);
        bool EksistererReferanse(string referanse);
        List<Bestilling> HentAlle();
        bool Slett(string referanse);
    }
}
