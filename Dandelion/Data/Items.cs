using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

#nullable disable

namespace Dandelion.Data
{
    public partial class Item
    {
        public int Id { get; set; }
        public int CollectionId { get; set; }
        public byte[]? Pic { get; set; }
        public string Name { get; set; }
        public DateTime CreateDate { get; set; }
        public string? Tegs { get; set; }

        public float? Float1 { get; set; }
        public float? Float2 { get; set; }
        public float? Float3 { get; set; }

        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }

        public string? Text1 { get; set; }
        public string? Text2 { get; set; }
        public string? Text3 { get; set; }

        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }

        public bool? Checkbox1 { get; set; }
        public bool? Checkbox2 { get; set; }
        public bool? Checkbox3 { get; set; }

        public Item(Item a)
        {
            Id = a.Id;
            CollectionId = a.CollectionId;
            Pic = a.Pic;
            Name = a.Name;
            CreateDate = a.CreateDate;
            Tegs = a.Tegs;

            Float1 = a.Float1;
            Line1 = a.Line1;
            Text1 = a.Text1;
            Date1 = a.Date1;
            Checkbox1 = a.Checkbox1;

            Float2 = a.Float2;
            Line2 = a.Line2;
            Text2 = a.Text2;
            Date2 = a.Date2;
            Checkbox2 = a.Checkbox2;

            Float3 = a.Float3;
            Line3 = a.Line3;
            Text3 = a.Text3;
            Date3 = a.Date3;
            Checkbox3 = a.Checkbox3;
        }
        public Item() { }
    }
}
