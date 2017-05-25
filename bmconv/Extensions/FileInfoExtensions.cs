using System.IO;

namespace OpenCGSS.Tools.BeatmapConverter.Extensions {
    public static class FileInfoExtensions {

        public static string GetSafeFileName(this FileInfo fileInfo) {
            var extension = fileInfo.Extension;
            if (string.IsNullOrEmpty(extension)) {
                return fileInfo.Name;
            } else {
                return fileInfo.Name.Substring(0, fileInfo.Name.Length - extension.Length);
            }
        }

    }
}
