using Flybilletter.DAL.Interfaces;
using System.Collections.Generic;

namespace Flybilletter.DAL.DBModel
{
    public class DBFly : IDBFly
    {
        public int ID { get; set; }
        public string Modell { get; set; } //Modell-navn til flyet
        public int AntallSeter { get; set; }
        public virtual List<DBFlygning> Flygninger { get; set; }
    }
}