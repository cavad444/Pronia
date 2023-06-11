namespace Pronia.Helpers
{
    public static class CustomFileManager
    {
        public static string SaveFile(string rootPath, string folder, IFormFile file)
        {
            string newFileName = GenerateUniqueFileName(file);
            string path = Path.Combine(rootPath, folder, newFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return newFileName;
        }

        public static void DeleteAllFiles(string rootPath, string folder, List<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                string path = Path.Combine(rootPath, folder, fileName);

                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }

        public static bool DeleteFile(string rootPath, string folder, string fileName)
        {
            string path = Path.Combine(rootPath, folder, fileName);

            if (File.Exists(path))
            {
                File.Delete(path);
                return true;
            }

            return false;
        }

        private static string GenerateUniqueFileName(IFormFile file)
        {
            string fileName = Guid.NewGuid().ToString() + (file.FileName.Length <= 64 ? file.FileName : (file.FileName.Substring(file.FileName.Length - 64)));
            return fileName;
        }
    }
}