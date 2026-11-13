using Microsoft.AspNetCore.Mvc;
using FloraSA.Models;
using System.Collections.Generic;
using System.IO;
using System;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;

namespace FloraSA.Controllers
{
    public class FlowerController : Controller
    {
        private readonly GenerativeModel generativeClient;

        public FlowerController(GenerativeModel generativeModel)
        {
             generativeClient = generativeModel;

            
        }
        //[HttpGet]
        public IActionResult Results()
        {
            return View(new Flower());
        }
        [HttpPost]
        public async Task<IActionResult> Results(Flower userUpload)
        {
            //Determining which image is uploaded 
            var image = userUpload.takePicture;

            if(userUpload.takePicture != null && userUpload.takePicture.Length>0)
            {
                image = userUpload.takePicture;
            }

            if(image == null  || image.Length == 0)
            {
                ModelState.AddModelError("", " Please upload or take a picture of any flower of your choice");
                return View("~/Views/Home/Index.cshtml", userUpload);
            }

            //Checking the file extension
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            var allowedExtensions = new[]
            {
                ".jpg",".jpeg",".png",".webp"
            
            };

            //validation of the extension
            if(!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", " Please enter only jpg ,png or webp image");
                return View("~/Views/Home/Index.cshtml", userUpload);
            }

            //reading the image into memory

            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            //userUpload.ImageBase64 = Convert.ToBase64String(memoryStream.ToArray());
            //userUpload.ImageContentType = userUpload.UploadImage.ContentType;


            Byte[] imageBytes = memoryStream.ToArray();
            string base64Image = Convert.ToBase64String(imageBytes);
            userUpload.ImageBase64 = base64Image;
            userUpload.ImageContentType = image.ContentType;


            //Send imageBytes to AI
            //service and request information in model.language

            //AI intergration

            var prompt = "Identify this flower or plant. Provide it's description , where it is originally from , what type of plant it is and few basic steps to care for it "+$"write the entire answer in  {userUpload.Language} ";
           // var imagePart = new Part { InlineData = new() { MimeType =image.ContentType, Data = base64Image } };

            try
            {

               

                var request = new GenerateContentRequest(prompt);
                await request.AddMedia(base64Image,image.ContentType);
                var response = await generativeClient.GenerateContent(request);
                userUpload.AiReponse = response.Text;
                //using(var imageStream = new MemoryStream(imageBytes))
                //{
                //    var dataUri = "data" + image.ContentType + ";base64" + base64Image;
                //    await request.AddMedia(dataUri);
                //}

              
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                ViewBag.Error = (ex.Message.Contains("503") || ex.Message.Contains("Unavailable")) ?
                    "The Ai services is busy right now .Please wait a minute and try again.":"Something went wrong while trying while trying to identify the plant.Please try again";
            }

            ViewBag.Language = userUpload.Language;
            ViewBag.ImageName = Path.GetFileName(image.FileName);
      


            return View("Results",userUpload);


        }
    }
}
