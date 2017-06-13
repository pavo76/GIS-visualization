using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GIS_visualization.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string src { get; set; }
        public string Author { get; set; }
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Dimensions { get; set; }
        public string InventoryMark { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}