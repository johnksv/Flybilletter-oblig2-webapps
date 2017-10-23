using Flybilletter.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;
using Model.DomeneModel;
using System;

namespace Flybilletter.DAL.DBModel
{
    public class DBAdmin : IDBAdmin
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        // NiceToHave: DateTime LastLogin
        // DB metoder under her

        public bool LeggInn(Admin admin)
        {
            using (var db = new DB())
            {
                try
                {
                    //TODO: Gjennomfør
                    return true;
                } catch (Exception e)
                {
                    return false;
                    //Logge feil?
                }
            }
        }

        public DBAdmin Hent(string username)
        {
            using(var db = new DB())
            {
                try
                {
                    return db.Administratorer.Find(username); //Admin == null betyr feil brukernavn
                } catch (Exception e)
                {
                    // Logge feil
                    return null;
                }
            }
        }
    }
}
