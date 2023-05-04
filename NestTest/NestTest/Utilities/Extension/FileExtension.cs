using NestTest.Models;

namespace NestTest.Utilities.Extension
{
    public static class FileExtension
    {
        public static string CutFileName(this IFormFile file, int maxSize = 60)
        {
            if (file.FileName.Length > maxSize)
            {
                return file.FileName.Substring(file.FileName.Length - maxSize);
            }
            return file.FileName;
        }
        public static bool Checksize(this IFormFile file , int kb)
        {
            if (file.Length / 1024 > kb) return true;
            return false;
        }
        public static bool CheckType(this IFormFile file,string type)
        {
            if (file.ContentType.Contains(type)) return true;
            return false;
        }
        public static string SaveFile(this IFormFile file , string savePath)
        {
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string path = Path.Combine(savePath, fileName);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            return fileName;
        }
        public static void SaveFileTest(this IFormFile file, string path)
        {
            path = Path.Combine(NestTest.Utilities.Constant.Constants.RootPath, path);
            using (var writer = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(writer);
            }
        }
    }
}
