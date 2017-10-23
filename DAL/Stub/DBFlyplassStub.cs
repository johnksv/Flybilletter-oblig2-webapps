using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBFlyplassStub : IDBFlyplass
    {
        public Flyplass Hent(string tilFlyplassID)
        {
            throw new NotImplementedException();
        }

        public List<Flyplass> HentAlle()
        {
            var resultat = new List<Flyplass>()
            {
             new Flyplass()
            {
                ID = "OSL",
                By = "Oslo",
                Land = "Norge",
                Navn = "Gardermoen Lufthavn"

            },
                new Flyplass()
            {
                ID = "BOO",
                By = "Bodø",
                Land = "Norge",
                Navn = "Bodø Lufthavn"

            },
             new Flyplass()
            {
                ID = "MXP",
                By = "Milano",
                Land = "Italia",
                Navn = "Malpensa lufthavn"

            }
        };

            return resultat;
        }
    }
}
