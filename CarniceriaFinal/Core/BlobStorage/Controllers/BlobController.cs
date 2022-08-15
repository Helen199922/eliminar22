using CarniceriaFinal.Core.BlobStorage.DTOs;
using CarniceriaFinal.Core.BlobStorage.Services.IServices;
using CarniceriaFinal.Core.CustomException;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarniceriaFinal.Core.BlobStorage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        public readonly IBlobService _blobService;
        public BlobController(IBlobService blobService)
        {
            this._blobService = blobService;
        }

        [HttpGet("blob/{file}")]
        public IActionResult Getblob(string file)
        {
            RSEntity<Uri> rsEntity = new();
            try
            {
                return Ok(rsEntity.Send(_blobService.GetBlobAsync(file)));

            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }

        }
        [HttpPost("upload-image")]
        public async Task<IActionResult> Postblob(UploadImageEntity data)
        {
            RSEntity<UploadImageResponseEntity> rsEntity = new();
            try
            {
                var image = await _blobService.UploadFileBlobAsync(data.image, data.contentType);
                return Ok(rsEntity.Send(image));

            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }

        }
        [HttpPost("upload-multi-image")]
        public async Task<IActionResult> Postblob(List<UploadMultiImageEntity> data)
        {
            RSEntity<List<UploadMultiImageResponseEntity>> rsEntity = new();
            try
            {
                var image = await _blobService.UploadMultiFileBlobAsync(data);
                return Ok(rsEntity.Send(image));

            }
            catch (RSException err)
            {
                return StatusCode(err.Code, rsEntity.Fail(err.MessagesError));
            }

        }
    }
}
