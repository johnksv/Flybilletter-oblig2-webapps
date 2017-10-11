using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Flybilletter.DAL.DBModel
{
    public class DBFlyplass
    {
        public string ID { get; set; }
        public string Navn { get; set; }
        public string By { get; set; }
        public string Land { get; set; }
        public virtual List<DBRute> Ruter { get; set; }

        public static List<Flyplass> HentAlle()
        {
            List<Flyplass> flyplasser = null;
            using (var db = new DB())
            {
                flyplasser = db.Flyplasser.Select(model => new Flyplass() {
                    ID = model.ID,
                    By = model.By,
                    Land = model.Land,
                    Navn =model.Navn,
                    //TODO: Skal vi virkelig mappe denne??
                    Ruter = model.Ruter.Select(m => new Rute () {
                        BasePris = m.BasePris,
                        // Flygninger = m.Flygninger TODO:hvor langt kan vi "hente" data?? Om vi skal ha flyginger må vi mappe dette fra DBFlygninger til Flygninger.
                    }).ToList()
                    
                }).ToList();
            }

            return flyplasser;

        }
    }
}