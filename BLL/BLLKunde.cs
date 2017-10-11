using Flybilletter.DAL.DBModel;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLKunde
    {

        public static string HentPoststed(string postnummer)
        {
            Postnummer postnr = DBPoststed.HentPoststed(postnummer);
            return  postnr == null ? "UGYLDIG POSTNUMMER" : postnr.Poststed;
        }
    }
}
