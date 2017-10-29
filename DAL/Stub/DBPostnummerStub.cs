using Flybilletter.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flybilletter.Model.DomeneModel;

namespace Flybilletter.DAL.Stub
{
    public class DBPostnummerStub : IDBPostnummer
    {
        private List<Postnummer> gyldigePostnummer = new List<Postnummer>()
        {
            new Postnummer() {Postnr = "0001", Poststed = "OSLO"},
            new Postnummer() {Postnr = "0010", Poststed = "OSLO"},
            new Postnummer() {Postnr = "0015", Poststed = "OSLO"},
            new Postnummer() {Postnr = "0018", Poststed = "OSLO"},
        };
        public Postnummer Hent(string postnummer)
        {
            return gyldigePostnummer.FirstOrDefault(post => post.Postnr == postnummer);
        }
    }
}
