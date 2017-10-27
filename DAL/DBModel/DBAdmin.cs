using Flybilletter.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;
using Model.DomeneModel;
using System;
using System.Linq;
using System.Text;

namespace Flybilletter.DAL.DBModel
{
    public class DBAdmin : IDBAdmin
    {

        IDBAdmin dbAdmin;

        public DBAdmin()
        {

        }

        public DBAdmin(IDBAdmin dbAdmin)
        {
            this.dbAdmin = dbAdmin;
        }

        [Key]
        public string Username { get; set; }
        public byte[] Password { get; set; }
        public string Salt { get; set; }
        // NiceToHave: DateTime LastLogin
        // DB metoder under her

        public bool LeggInn(Admin admin)
        {
            using (var db = new DB())
            {
                try
                {
                    if (admin != null)
                    {
                        var salt = lagSalt();
                        DBAdmin dbadmin = new DBAdmin()
                        {
                            Username = admin.Username,
                            Salt = salt,
                            Password = HashPassord(admin.Password, salt)
                        };
                        db.Administratorer.Add(dbadmin);

                        db.Endringer.Add(new DBEndring()
                        {
                            Tidspunkt = DateTime.Now,
                            Endring = "Legg til administrator med brukernavn " + admin.Username
                        });
                        db.SaveChanges();
                    }
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

        public byte[] HashPassord(string password, string salt)
        {
            var algorithm = System.Security.Cryptography.SHA256.Create();
            byte[] str = Encoding.ASCII.GetBytes(String.Concat(password, salt));
            return algorithm.ComputeHash(str);
        }

        private string lagSalt()
        {
            Random r = new Random();
            var saltLength = r.Next(10, 20);
            StringBuilder str = new StringBuilder();
            for (var i = 0; i < saltLength; i++)
            {
                str.Append((char)r.Next(65, 122));
            }
            return str.ToString();
        }

    }
}
