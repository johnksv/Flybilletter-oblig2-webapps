using AutoMapper;
using Flybilletter.DAL;
using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;
using DAL;
using System.Diagnostics.CodeAnalysis;

namespace Flybilletter.DAL.DBModel
{
    [ExcludeFromCodeCoverage]
    //Lagrer endringer i databasen
    public class DBEndring : IDBEndring
    {
        [Key]
        public int ID { get; set; }
        public string Endring { get; set; }
        public DateTime Tidspunkt { get; set; }

        public List<Endring> Hent()
        {
            using (var db = new DB())
            {
                try
                {
                    return Mapper.Map<List<Endring>>(db.Endringer);
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBEndring:Hent", e, "En feil oppsto da metoden prøvde å hente alle registrerte endringer fra databasen");
                    return null;
                }
            }
        }
    }
}
