namespace MastersMVC2.Utilities
{
    public static class FileUploadExtension
    {
        public static async Task<string?> SaveFileAsync(this IFormFile file, IWebHostEnvironment webHostEnvironment, string folderName, long maxSize)
        {
            if (file.Length > maxSize)
            {
                throw new InvalidOperationException($"Limit is  {maxSize / 1024 / 1024} MB.");
            }
            string rootPath = webHostEnvironment.WebRootPath;
            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string folderPath = Path.Combine(rootPath, folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine(folderName, fileName).Replace("\\", "/");
        }
    }
}
