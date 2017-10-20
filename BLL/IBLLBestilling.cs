using Flybilletter.Model.DomeneModel;
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

        Bestilling FinnBestilling(string referanse);
        bool VerifiserKredittkort(string CVCstring, string utlop, out string feilmelding);
        Bestilling LeggInn(List<Kunde> kunder, BestillingViewModel gjeldende);
        bool EksistererReferanse(string referanse);

    }
}
