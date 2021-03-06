﻿using AutoMapper;
using DAL;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Flybilletter.DAL.DBModel
{
    [ExcludeFromCodeCoverage]
    public class DBPostnummer : IDBPostnummer
    {
        [Key]
        public string Postnr { get; set; }
        public string Poststed { get; set; }
        public virtual List<DBKunde> Kunder { get; set; }

        public Postnummer Hent(string postnummer)
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
                        DALsetup.LogFeilTilFil("DBPostnummer:Hent", e, "En feil oppsto da metoden prøvde å hente poststed.");
                        return null;
                    }
                }
            }
            return null;
        }
    }
}
