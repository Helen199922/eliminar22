using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CarniceriaFinal.Core.BlobStorage.DTOs;
using CarniceriaFinal.Core.BlobStorage.Services.IServices;
using CarniceriaFinal.Core.CustomException;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.BlobStorage.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private const string AllowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public Uri GetBlobAsync(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("images");
            var blobClient = containerClient.GetBlobClient(name);
            var exist = blobClient.Exists();//verifica con la extensión
            return blobClient.Uri;        
        }

        public async Task<UploadImageResponseEntity> UploadFileBlobAsync(string image, string contentType)
        {

            try
            {
                var customTermsSignatureData = image;
                var encodedImage = customTermsSignatureData.Split(',')[1];
                var decodedImage = Convert.FromBase64String(encodedImage);
                var nameImage = this.getNamePhoto();
                var containerClient = _blobServiceClient.GetBlobContainerClient("images");
                string name = nameImage + "." + contentType[6..];
                BlobClient blobClient = containerClient.GetBlobClient(nameImage + "." + contentType[6..]);

                using (var fileStream = new MemoryStream(decodedImage))
                {
                    var result = await blobClient.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = contentType });
                    if (!result.GetRawResponse().Status.Equals(201))
                    {
                        throw new Exception();
                    }
                }
                return new UploadImageResponseEntity() { imageUrl = blobClient.Uri.AbsoluteUri };
            }
            catch (Exception)
            {
                throw new RSException("error", 500).SetMessage("Ha ocurrido un error al subir su imagen. Intente más tarde.");
            }
            
        }
        public string? getNamePhoto()
        {
            var bytes = new byte[50];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(bytes);
            }

            return new string(bytes.Select(x => AllowableCharacters[x % AllowableCharacters.Length]).ToArray());
        }

    }
}
