using AutoMapper;
using Flybilletter.Model.DomeneModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DBModel
{
    public class DBAdmin
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        // DB metoder under her
    }
}
