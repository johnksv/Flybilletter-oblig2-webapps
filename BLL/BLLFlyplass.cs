﻿using Flybilletter.DAL.DBModel;
using Flybilletter.DAL.Interfaces;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLLFlyplass : IBLLFlyplass
    {
        private IDBFlyplass dbFlyplass;

        public BLLFlyplass()
        {
            dbFlyplass = new DBFlyplass();
        }

        public BLLFlyplass(IDBFlyplass stub)
        {
            this.dbFlyplass = stub;
        }

        public List<Flyplass> HentAlle()
        {
            return dbFlyplass.HentAlle();
        }
    }
}
