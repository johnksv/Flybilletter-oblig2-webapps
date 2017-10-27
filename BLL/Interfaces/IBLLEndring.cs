using Flybilletter.Model.DomeneModel;
using Flybilletter.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IBLLEndring
    {
        List<Endring> Hent();
        List<FeilFraFilViewModel> ParseFeil(string filePath);
    }
}
