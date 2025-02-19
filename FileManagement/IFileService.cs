namespace NurseryMart.FileManagement
{
    public interface IFileService
    {
        Task UploadToS3(Stream stream, string bucketName, string key);
        Task DeleteFromS3(string bucketName, string key);
        Task<Stream> DownloadFromS3(string bucketName, string key);
        Task DeleteMultiple(string bucketName, IEnumerable<string> keys);
    }
}
