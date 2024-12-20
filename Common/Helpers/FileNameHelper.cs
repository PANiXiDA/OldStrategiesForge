namespace Common.Helpers;

public static class FileNameHelper
{
    /// <summary>
    /// Извлекает имя файла из пути S3.
    /// </summary>
    /// <param name="s3Path">Путь к файлу в формате S3.</param>
    /// <returns>Имя файла без параметров.</returns>
    public static string GetFileName(string s3Path)
    {
        if (string.IsNullOrEmpty(s3Path))
            throw new ArgumentException("Path cannot be null or empty", nameof(s3Path));

        var fileNameWithParams = s3Path.Substring(s3Path.LastIndexOf('/') + 1);
        return fileNameWithParams.Contains('?')
            ? fileNameWithParams.Substring(0, fileNameWithParams.IndexOf('?'))
            : fileNameWithParams;
    }

    /// <summary>
    /// Устанавливает имя файла в пути S3.
    /// </summary>
    /// <param name="s3Path">Исходный путь к файлу в формате S3.</param>
    /// <param name="fileName">Новое имя файла.</param>
    /// <returns>Обновленный путь с новым именем файла.</returns>
    public static string SetFileName(string s3Path, string fileName)
    {
        if (string.IsNullOrEmpty(s3Path))
            throw new ArgumentException("Path cannot be null or empty", nameof(s3Path));

        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

        var basePath = s3Path.Contains('?')
            ? s3Path.Substring(0, s3Path.IndexOf('?'))
            : s3Path;

        return $"{basePath.Substring(0, basePath.LastIndexOf('/') + 1)}{fileName}";
    }
}

