namespace NurseryMart.FileManagement
{
    public interface IS3Provider
    {
        Task Upload(Stream stream, string bucketName, string key);
        public Task Delete(string bucketName, string key);
        Task DeleteMultiple(string bucketName, IEnumerable<string> keys);
        Task<Stream> Download(string bucketName, string key);
    }
}
