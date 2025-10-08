using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tours.Core.UseCases;

public class ImageService : IImageService
{
    public ImageService() { }

    public string SaveImage(string folderPath, byte[] imageData, string folderName)
    {
        var fileName = Guid.NewGuid() + ".png";

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var filePath = Path.Combine(folderPath, fileName);

        File.WriteAllBytes(filePath, imageData);
        return $"images/{folderName}/{fileName}";



    }

    public void DeleteOldImage(string oldImagePath)
    {
        if (File.Exists(oldImagePath))
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }

    }
}
