using Flybilletter.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flybilletter.DAL.DBModel
{
    //Lagrer endringer i databasen
    public class DBEndring
    {
        [Key]
        public int ID { get; set; }
        public string Endring { get; set; }
        public DateTime Tidspunkt { get; set; }
    }
}
