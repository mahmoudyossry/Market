using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Application.Services.Interfaces
{
    public interface IFileService
    {
        string  UploadFile(IFormFile file);
        List<string> UploadFiles(List<IFormFile> files);
        bool CopyFileToActualFolder(string FileName, string tempPath, string pathToMove);
        bool DeleteFileFromTempFolder(string FileName);
        string GetTempAttachmentPath();
        string GetActualAttachmentPath();
    }
}
