using Flybilletter.Model.DomeneModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Flybilletter.DAL.DBModel
{
    public class DBPoststed
    {
        [Key]
        public string Postnr { get; set; }
        public string Poststed { get; set; }
        public virtual List<DBKunde> Kunder { get; set; }

        public static Postnummer HentPoststed(string postnummer)
        {
            var regex = new Regex("^[0-9]{4}$");
            bool isMatch = regex.IsMatch(postnummer);

            Postnummer poststed = null;
            if (isMatch)
            {
                using (var db = new DB())
                {
                    DBPoststed dbpoststed = db.Poststeder.FirstOrDefault(model => model.Postnr == postnummer);
                    poststed = new Postnummer()
                    {
                        Postnr = dbpoststed.Postnr,
                        Poststed = dbpoststed.Poststed
                    };
                }
            }

            return poststed;
        }
    }
}
