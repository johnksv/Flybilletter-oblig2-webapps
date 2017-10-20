using Flybilletter.DAL.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Flybilletter.DAL.DBModel
{
    public class DBAdmin : IDBAdmin
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        // DB metoder under her
    }
}
