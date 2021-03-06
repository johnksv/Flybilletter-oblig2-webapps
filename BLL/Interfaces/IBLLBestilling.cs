﻿using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLBestilling
    {

        Bestilling LeggInn(List<Kunde> kunder, BestillingViewModel gjeldende);
        Bestilling Hent(string referanse);
        List<Bestilling> HentAlle();
        bool Slett(string referanse);
        bool SlettSomKunde(string referanse);
        bool VerifiserKredittkort(string CVCstring, string utlop, out string feilmelding);
        string EksistererReferanse(string baseUrl, string referanse);
    }
}
