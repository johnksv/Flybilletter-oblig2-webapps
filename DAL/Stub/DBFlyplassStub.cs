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

        private List<Flyplass> flyplasser = new List<Flyplass>()
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

        public Flyplass Hent(string tilFlyplassID)
        {
            return flyplasser.FirstOrDefault(f => f.ID == tilFlyplassID);
        }

        public List<Flyplass> HentAlle()
        {
            return flyplasser;
        }

        public bool LeggInn(Flyplass flyplass)
        {

            if(flyplass != null && flyplass.By != null && flyplass.ID != null && flyplass.Land != null && flyplass.Navn != null)
            {
                flyplasser.Add(flyplass);
                return true;

            }
            return false;
        }
    }
}
