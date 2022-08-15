using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.BlobStorage.DTOs
{
    public class UploadImageResponseEntity
    {
        public string? imageUrl { get; set; }
    }
    public class UploadMultiImageResponseEntity : UploadImageResponseEntity
    {
        public int idImage { get; set; }
    }
}
