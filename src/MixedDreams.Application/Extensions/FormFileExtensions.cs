using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MixedDreams.Application.Extensions
{
    public static class FormFileExtensions
    {
        public const int ImageMinimumBytes = 512;
        public static bool IsImageAsync(this IFormFile formFile)
        {
            Dictionary<string, byte[][]> formats = new Dictionary<string, byte[][]>
            {
                {
                    "png", 
                    new byte[][] 
                    {
                        new byte[]{ 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } 
                    } 
                },
                {
                    "jpg",
                    new byte[][]
                    {
                        new byte[]{ 0xFF, 0xD8, 0xFF, 0xE0 },
                        new byte[]{ 0xFF, 0xD8, 0xFF, 0xDB },
                        new byte[]{ 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46 },
                        new byte[]{ 0x49, 0x46, 0x00, 0x01 },
                        new byte[]{ 0xFF, 0xD8, 0xFF, 0xEE },
                        new byte[]{ 0x69, 0x66, 0x00, 0x00 },
                        new byte[]{ 0xFF, 0xD8, 0xFF, 0xE0 },
                    }
                },
                {
                    "jpeg",
                    new byte[][]
                    {
                        new byte[]{ 0xFF, 0xD8, 0xFF, 0xDB },
                        new byte[]{ 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46 },
                        new byte[]{ 0x49, 0x46, 0x00, 0x01 },
                        new byte[]{ 0xFF, 0xD8, 0xFF, 0xEE },
                        new byte[]{ 0x69, 0x66, 0x00, 0x00 },
                        new byte[]{ 0xFF, 0xD8, 0xFF, 0xE0 },
                    }
                }
            };
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (formFile.ContentType.ToLower() != "image/jpg" &&
                        formFile.ContentType.ToLower() != "image/jpeg" &&
                        formFile.ContentType.ToLower() != "image/pjpeg" &&
                        formFile.ContentType.ToLower() != "image/gif" &&
                        formFile.ContentType.ToLower() != "image/x-png" &&
                        formFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(formFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(formFile.FileName).ToLower() != ".png"
                && Path.GetExtension(formFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(formFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!formFile.OpenReadStream().CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (formFile.Length < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                formFile.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }

                // Checking by header start
                string fileExt = formFile.FileName.Substring(formFile.FileName.LastIndexOf('.') + 1).ToUpper();
                byte[][] tmp = formats[fileExt.ToLower()];

                foreach (byte[] validHeader in tmp)
                {
                    byte[] header = new byte[validHeader.Length];

                    formFile.OpenReadStream().Seek(0, SeekOrigin.Begin);
                    formFile.OpenReadStream().Read(header, 0, header.Length);

                    if (validHeader.SequenceEqual(header))
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
