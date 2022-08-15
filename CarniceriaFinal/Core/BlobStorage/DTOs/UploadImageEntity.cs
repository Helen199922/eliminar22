using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.BlobStorage.DTOs
{
    public class UploadImageEntity
    {
        public string? image { get; set; }
        public string? contentType { get; set; }
    }
    public class UploadMultiImageEntity
    {
        public int idImage { get; set; }
        public string? image { get; set; }
        public string? contentType { get; set; }
    }
}
