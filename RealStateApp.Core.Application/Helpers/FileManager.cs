using Microsoft.AspNetCore.Http;


namespace RealStateApp.Core.Application.Helpers
{
    public class FileManager
    {
        public static string UploadFile(IFormFile file, string id, bool isEditMode = false, string imagePath = "", bool property = false)
        {
            if (isEditMode && file == null)
            {
                return imagePath;
            }

            string basePath = property ? $"/Images/Property/{id}" : $"/Images/User/{id}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot{basePath}");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Guid guid = Guid.NewGuid();
            FileInfo fileInfo = new(file.FileName);
            string fileName = guid + fileInfo.Extension;
            string fileNameWithPath = Path.Combine(path, fileName);

            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            if (isEditMode && !string.IsNullOrEmpty(imagePath))
            {
                string[] oldImagePart = imagePath.Split("/");
                string oldImagePath = oldImagePart[^1];
                string completeImageOldPath = Path.Combine(path, oldImagePath);
                if (System.IO.File.Exists(completeImageOldPath))
                {
                    System.IO.File.Delete(completeImageOldPath);
                }
            }

            return $"{basePath}/{fileName}";
        }

        public static void DeleteFile(string imagePath, bool property = false)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return;
            }
            string[] imageParts = imagePath.Split("/");
            string fileName = imageParts[^1];
            string basePath = property ? $"/Images/Property/file/{fileName}" : $"/Images/User/file/{fileName}";
            string path = Path.Combine(Directory.GetCurrentDirectory(), basePath);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

    }
}
