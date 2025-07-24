using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.BuildingBlocks.Core.UseCases;

public interface IImageService
{
    string SaveImage(string folderPath, byte[] imageData, string folderName);

    void DeleteOldImage(string oldPath); 
}
