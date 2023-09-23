using Microsoft.AspNetCore.Http;
using Market.Application.Services.Interfaces;
using Market.Core;
using Market.Core.Global;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace Market.Application.Services
{
    public class FileService : IFileService
    {
        private readonly GlobalInfo globalInfo;
        private readonly AppSettingsConfiguration settings;
        
        public FileService(AppSettingsConfiguration settings
            , GlobalInfo globalInfo)
        {
            this.settings = settings;
            this.globalInfo = globalInfo;
        }
        public string UploadFile(IFormFile file)
        {
            return UploadFiles(new List<IFormFile> { file }).First();
        }

        public List<string> UploadFiles(List<IFormFile> files)
        {
            string pathToSave = GetTempAttachmentPath();
            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            var fileNames = new List<string>();
            foreach (var formFile in files)
            {
                var fname = ContentDispositionHeaderValue.Parse(formFile.ContentDisposition).FileName.Trim('"');
                var fnameSplit = fname.Split(".");
                var fNewName = Guid.NewGuid().ToString() + "." + fnameSplit[fnameSplit.Length - 1];
                var fullPath = Path.Combine(pathToSave, fNewName);
                fileNames.Add(fNewName);
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        formFile.CopyToAsync(stream);
                    }
                }
                else
                    throw new AppException(ExceptionEnum.InCorrectFileLength);

            }
            return fileNames;
        }
        public bool CopyFileToActualFolder(string FileName, string tempPath, string pathToMove)
        {
            if (!Directory.Exists(pathToMove))
                Directory.CreateDirectory(pathToMove);
            var tempFilePath = Path.Combine(tempPath, FileName);
            var destinationFilePath = Path.Combine(pathToMove, FileName);
            if (File.Exists(tempFilePath))
            {
                if (!File.Exists(destinationFilePath))
                    File.Copy(tempFilePath, destinationFilePath);
                return true;
            }
            return false;
        }

        public bool DeleteFileFromTempFolder(string FileName)
        {
            try
            {
                string pathToRemove = GetTempAttachmentPath();
                if (Directory.Exists(pathToRemove))
                {
                    var tempFilePath = Path.Combine(pathToRemove, FileName);
                    if (File.Exists(tempFilePath))
                    {
                        File.Delete(tempFilePath);
                    }
                }
            }
            catch (Exception ex) { }
            return true;
        }
        public string GetTempAttachmentPath()
        {
            return settings.AttachmentSettings.TempAttachment;
        }
        public string GetActualAttachmentPath()
        {
            return settings.AttachmentSettings.ActualAttachment;
        }

    }
}
