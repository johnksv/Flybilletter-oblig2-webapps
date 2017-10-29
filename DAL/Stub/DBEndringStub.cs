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

namespace Flybilletter.DAL.Stub
{
    //Lagrer endringer i databasen
    public class DBEndringStub : IDBEndring
    {

        public List<Endring> HentAlle()
        {
            var endringer = new List<Endring>()
            {
                new Endring() {},
                new Endring() {}
            };
            return endringer;
        }
    }
}
