using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Dandelion.Data
{
    public partial class User
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public string Login { get; set;}
        public string Password { get; set;}
        public string Status { get; set;}
    }
}
