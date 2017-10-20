using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.DAL.Interfaces
{
    public interface IDBBestilling
    {

        Bestilling FinnBestilling(string referanse);
        void LeggInn(Bestilling bestilling);
        bool EksistererReferanse(string referanse);
    }
}
