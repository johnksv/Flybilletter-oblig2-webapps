﻿using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLKunde
    {
        bool LeggInn(IEnumerable<Kunde> kunder);
        bool LeggInn(Kunde innKunde);
        bool Endre(Kunde kunde);
        Kunde HentEnKunde(int id);
        List<Kunde> HentAlle();
        string HentPoststed(string postnummer);
        bool Slett(int id);
    }
}
