using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetoLibrary.DAL;
using MetoLibrary.DAL.Repositorys;
using System.IO;

namespace MetoLibrary.BLL.Services
{
    public class ImageService
    {
        private readonly ImageRepository _imageRepository;

        public ImageService(ImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<bool> ProcessImageAsync(string imagePath)
        {
            // Görüntü işleme mantığını buraya ekleyin (eğer varsa)
            return await _imageRepository.SaveImageAsync(imagePath);
        }
    }
}
