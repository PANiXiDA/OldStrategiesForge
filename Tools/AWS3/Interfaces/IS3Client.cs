using Amazon.S3;
using Amazon.S3.Model;

namespace Tools.AWS3.Interfaces;

public interface IS3Client
{
    /// <summary>
    /// Отправляет файл в бакет c базовым префиксом
    /// </summary>
    /// <param name="stream">Стрим</param>
    /// <param name="fileExtension">Расширение файла</param>
    /// <param name="fileType"></param>
    /// <returns>Ключ нового файла</returns>
    Task<string?> PutFile(Stream stream, string fileExtension, string fileType);
    /// <summary>
    /// Отправляет файл в бакет
    /// </summary>
    /// <param name="stream">Стрим</param>
    /// <param name="keyPrefix">Префикс пути</param>
    /// <param name="fileExtension">Расширение файла</param>
    /// <param name="fileType"></param>
    /// <returns>Ключ нового файла</returns>
    Task<string?> PutFile(Stream stream, string keyPrefix, string fileExtension, string fileType);

    Task<GetObjectResponse> GetFile(string key);
    Task<bool> DeleteFile(string key);

    /// <summary>
    /// Генерирует временную ссылку для файла
    /// </summary>
    /// <param name="key">Ключ файла в бакете</param>
    /// <param name="expirationTime">Срок действия ссылки</param>
    /// <param name="verb">HTTP метод (GET/PUT и т.д.)</param>
    /// <returns>Временная ссылка</returns>
    Task<string> GeneratePresignedUrl(string key, TimeSpan expirationTime, HttpVerb verb = HttpVerb.GET);
}