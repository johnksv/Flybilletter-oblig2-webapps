using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.DAL.Interfaces
{
    public interface IDBFlygning
    {
        List<Flygning> HentFlygningerFra(Flyplass flyplass);
        List<Flygning> HentFlygningerTil(Flyplass flyplass);
        Flygning Finn(int ID);
        List<Flygning> HentAlle(DateTime dateTime);
        Flygning Hent(int id);
        bool Endre(Flygning flygning);
        bool EndreStatus(int id);
        bool LeggInn(Flygning flygning);
        bool Endre(int id, DateTime nyAvgangstid);
    }
}
