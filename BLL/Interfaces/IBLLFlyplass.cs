﻿using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLFlyplass
    {
        List<Flyplass> HentAlle();
        bool LeggInn(Flyplass flyplass);
        Flyplass Hent(string id);
        bool Endre(Flyplass item);
    }
}
