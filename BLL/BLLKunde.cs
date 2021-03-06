﻿using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.DAL.Stub;
using System.Diagnostics.CodeAnalysis;

namespace BLL
{
    public class BLLKunde : IBLLKunde
    {
        private IDBKunde dbkunde;
        private IDBPostnummer dbPostnummer;

        [ExcludeFromCodeCoverage]
        public BLLKunde()
        {
            dbkunde = new DBKunde();
            dbPostnummer = new DBPostnummer();
        }

        public BLLKunde(IDBKunde stub, IDBPostnummer postnummerStub)
        {
            this.dbkunde = stub;
            this.dbPostnummer = postnummerStub;
        }

        public string HentPoststed(string postnummer)
        {
            Postnummer postnr = dbPostnummer.Hent(postnummer);
            return  postnr == null ? "UGYLDIG POSTNUMMER" : postnr.Poststed;
        }

        public bool LeggInn(Kunde innKunde)
        {
            return dbkunde.LeggInn(innKunde);
        }

        public bool Endre(Kunde kunde)
        {
            return dbkunde.Endre(kunde);
        }
        
        public List<Kunde> HentAlle()
        {
            return dbkunde.HentAlle();
        }

        public bool Slett(int id)
        {
            return dbkunde.Slett(id);
        }
    }
}
