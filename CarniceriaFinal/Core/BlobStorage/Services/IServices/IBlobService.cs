using Azure.Storage.Blobs.Models;
using CarniceriaFinal.Core.BlobStorage.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.BlobStorage.Services.IServices
{
    public interface IBlobService
    {
        public Uri GetBlobAsync(string name);
        public Task<UploadImageResponseEntity> UploadFileBlobAsync(string image, string contentType);
        Task<List<UploadMultiImageResponseEntity>> UploadMultiFileBlobAsync(List<UploadMultiImageEntity> data);
    }
}
