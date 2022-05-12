using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Dandelion.Data
{
    public partial class Comment
    {
        public int Id { get; set;}
        public string Name { get; set; }
        public string Text { get; set;}
        public int ItemId { get; set; }
    }
}
