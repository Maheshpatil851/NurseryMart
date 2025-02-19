using Microsoft.Extensions.Logging;

namespace NurseryMart.FileManagement
{
    public class FileService : IFileService
    {
        private readonly ILogger<FileService> _logger;
        private readonly IS3Provider _s3Provider;

        public FileService( IS3Provider s3Provider ,ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<FileService>();
            _s3Provider = s3Provider;
        }

        public async Task UploadToS3(Stream stream, string bucketName, string key)
        {
            await _s3Provider.Upload(stream, bucketName, key);
        }
        public async Task DeleteMultiple(string bucketName, IEnumerable<string> keys)
        {
            await _s3Provider.DeleteMultiple(bucketName, keys);
        }
        public async Task DeleteFromS3(string bucketName, string key)
        {
            await _s3Provider.Delete(bucketName, key);
        }

        public async Task<Stream> DownloadFromS3(string bucketName, string key)
        {
            return await _s3Provider.Download(bucketName, key);
        }
    }
}
