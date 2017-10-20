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
    }
}
