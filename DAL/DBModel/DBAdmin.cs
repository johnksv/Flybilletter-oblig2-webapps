using AutoMapper;
using Flybilletter.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;
using Flybilletter.Model.DomeneModel;
using System;
using System.Linq;
using System.Text;
using DAL;
using System.Collections.Generic;
using Flybilletter.Model.ViewModel;
using System.Diagnostics.CodeAnalysis;

namespace Flybilletter.DAL.DBModel
{
    [ExcludeFromCodeCoverage]
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
        public string Brukernavn { get; set; }
        public byte[] Passord { get; set; }
        public string Salt { get; set; }

        public bool LeggInn(Admin admin)
        {
            using (var db = new DB())
            {
                try
                {
                    if (admin != null)
                    {
                        var salt = LagSalt();
                        DBAdmin dbadmin = new DBAdmin()
                        {
                            Brukernavn = admin.Brukernavn,
                            Salt = salt,
                            Passord = HashPassord(admin.Passord, salt)
                        };
                        db.Administratorer.Add(dbadmin);

                        db.Endringer.Add(new DBEndring()
                        {
                            Tidspunkt = DateTime.Now,
                            Endring = "Legg til administrator med brukernavn " + admin.Brukernavn
                        });
                        db.SaveChanges();
                        return true;
                    }
                    return false;
                } catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBAdmin:LeggInn", e, "En feil oppsto da metoden prøvde å legge inn administrator med brukernavn " + admin.Brukernavn);
                    return false;
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
                    DALsetup.LogFeilTilFil("DBAdmin:Hent", e, "En feil oppsto da metoden prøvde å administrator, brukernavn: " + username);
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

        /* 
            Denne metoden genererer et dummy-salt. Random-klassen vil kun være pseudo-random, 
            men for å illustrere effekten av å legge til en tilfeldig string på passordet er dette godt nok.
        */
        private string LagSalt()
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

        public List<Admin> HentAlle()
        {
            using (var db = new DB())
            {
                try
                {
                    var dbadminer = db.Administratorer.ToList();
                    var adminer = new List<Admin>();
                    foreach (var dbadmin in dbadminer)
                    {
                        var admin = Mapper.Map<Admin>(dbadmin);
                        adminer.Add(admin);
                    }
                    return adminer;
                }
                catch(Exception e)
                {
                    DALsetup.LogFeilTilFil("DBAdmin:HentAlle", e, "En feil oppsto da metoden prøvde å returnere alle administratorer fra databasen");
                    return new List<Admin>(); // Tom liste
                }
            }
        }

        public bool EndrePassord(string username, string password)
        {
            using (var db = new DB())
            {
                try
                {
                    var salt = LagSalt();
                    var hash = HashPassord(password, salt);
                    var admin = db.Administratorer.Find(username);
                    admin.Passord = hash;
                    admin.Salt = salt;
                    db.Endringer.Add(new DBEndring()
                    {
                        Tidspunkt = DateTime.Now,
                        Endring = "Endrer passord på admin " + username
                    });
                    db.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBAdmin:EndrePassord", e, "En feil oppsto da metoden prøvde å endre passordet til admin " + username);
                }
            }
            return false;
        }

        public bool Slett(string username)
        {
            using (var db = new DB())
            {
                try
                {
                    var admin = db.Administratorer.Where(kunde => kunde.Brukernavn == username).FirstOrDefault();
                    if (admin != null)
                    {
                        db.Administratorer.Remove(admin);
                        db.Endringer.Add(new DBEndring()
                        {
                            Tidspunkt = DateTime.Now,
                            Endring = $"Slettet administrator {admin.Brukernavn} "
                        });
                        db.SaveChanges();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    DALsetup.LogFeilTilFil("DBAdmin:Slett", e, "En feil oppsto da metoden prøvde å slette administrator.");
                }
                return false;
            }
        }
    }
}
