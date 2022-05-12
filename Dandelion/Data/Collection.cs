using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using static System.Net.Mime.MediaTypeNames;

#nullable disable

namespace Dandelion.Data
{
    public partial class Collection
    {
        public int Id { get; set;}
        public string Login { get; set; }
        public byte[]? Pic { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tag { get; set; }

        public string? Float1 { get; set; }
        public string? Float2 { get; set; }
        public string? Float3 { get; set; }

        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }

        public string? Text1 { get; set; }
        public string? Text2 { get; set; }
        public string? Text3 { get; set; }

        public string? Date1 { get; set; }
        public string? Date2 { get; set; }
        public string? Date3 { get; set; }

        public string? Checkbox1 { get; set; }
        public string? Checkbox2 { get; set; }
        public string? Checkbox3 { get; set; }

        public Collection(Collection a)
        {
            Id = a.Id;
            Login = a.Login;
            Pic = a.Pic;
            Name = a.Name;
            Description = a.Description;
            Tag = a.Tag;

            Float1 = a.Float1;
            Line1 = a.Line1;
            Text1 = a.Text1;
            Date1= a.Date1;
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
        public Collection(){}
}
    }
