using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetoLibrary.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public byte[] ImageData { get; set; }

    }
}
