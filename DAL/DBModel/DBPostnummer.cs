using AutoMapper;
using DAL;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Flybilletter.DAL.DBModel
{
    public class DBPostnummer : IDBPostnummer
    {
        [Key]
        public string Postnr { get; set; }
        public string Poststed { get; set; }
        public virtual List<DBKunde> Kunder { get; set; }

        public Postnummer HentPoststed(string postnummer)
        {
            var regex = new Regex("^[0-9]{4}$");
            bool isMatch = regex.IsMatch(postnummer);

            if (isMatch)
            {
                using (var db = new DB())
                {
                    try
                    {
                        return Mapper.Map<Postnummer>(db.Poststeder.FirstOrDefault(model => model.Postnr == postnummer));
                    }catch(Exception e)
                    {
                        DALsetup.LogFeilTilFil(System.Reflection.MethodBase.GetCurrentMethod().Name, e, "En feil oppsto da metoden prøvde å hente postnummer " + postnummer);
                        return null;
                    }
                }
            }
            return null;
        }
    }
}
