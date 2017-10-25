using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.Model.DomeneModel
{
    public class Endring
    {
        public int ID { get; set; }
        public string EndringString { get; set; }
        public DateTime Tidspunkt { get; set; }
    }
}
