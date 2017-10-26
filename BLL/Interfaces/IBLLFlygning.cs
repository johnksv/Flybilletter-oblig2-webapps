using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLFlygning
    {

        List<Reise> FinnReiseforslag(string fraFlyplassID, string tilFlyplassID, DateTime avreiseDag);
        void FinnReisemuligheter(SokViewModel innSok, out FlygningerViewModel reiser, out List<Reise> flygningerTur, out List<Reise> flygningerRetur);
        List<Flygning> HentAlle(DateTime dateTime);
        Flygning HentEnFlygning(int id);
        bool OppdaterFlygning(Flygning flygning);
        bool OppdaterStatus(int id);
    }
}
