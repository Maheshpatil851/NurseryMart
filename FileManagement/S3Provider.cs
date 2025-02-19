using Amazon.S3;
using Amazon.S3.Model;
using NurseryMart.Contract;

namespace NurseryMart.FileManagement
{
    public class S3Provider : IS3Provider
    {
        IConfiguration _configuration;
        public S3Provider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Upload(Stream stream, string bucketName, string key)
        {
            var client = new AmazonS3Client(_configuration["ThirdPartyServices:AWS:s3:aws-access-key"], _configuration["ThirdPartyServices:AWS:s3:aws-access-secret"], region: Amazon.RegionEndpoint.APSouth1);
            try
            {
                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    InputStream = stream,
                    //ContentType = "text/plain"
                };

                //putRequest.Metadata.Add("x-amz-meta-title", "");
                PutObjectResponse response2 = await client.PutObjectAsync(putRequest);
            }
            catch (AmazonS3Exception e)
            {
                //throw new RestException(System.Net.HttpStatusCode.BadRequest, $"Error encountered on server. Message:'{e.Message}' when uploading an object");
            }
            catch (Exception e)
            {
                //Console.WriteLine(
                //    "Unknown encountered on server. Message:'{0}' when writing an object"
                //    , e.Message);
            }
        }
        public async Task Delete(string bucketName, string key)
        {
            var client = new AmazonS3Client(_configuration["ThirdPartyServices:AWS:s3:aws-access-key"], _configuration["ThirdPartyServices:AWS:s3:aws-access-secret"], region: Amazon.RegionEndpoint.APSouth1);
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                Console.WriteLine("Deleting an object");
                await client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (AmazonS3Exception e)
            {
                throw new RestException(System.Net.HttpStatusCode.BadRequest, $"Error encountered on server. Message:'{e.Message}' when deleting an object");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when deleting an object", e.Message);
            }
        }
        public async Task DeleteMultiple(string bucketName, IEnumerable<string> keys)
        {
            var client = new AmazonS3Client(_configuration["ThirdPartyServices:AWS:s3:aws-access-key"], _configuration["ThirdPartyServices:AWS:s3:aws-access-secret"], region: Amazon.RegionEndpoint.APSouth1);
            try
            {
                var deleteObjectsRequest = new DeleteObjectsRequest
                {
                    BucketName = bucketName,
                    Objects = new List<KeyVersion>()
                };

                foreach (var key in keys)
                {
                    deleteObjectsRequest.Objects.Add(new KeyVersion { Key = key });
                }

                Console.WriteLine("Deleting objects...");
                var response = await client.DeleteObjectsAsync(deleteObjectsRequest);

                Console.WriteLine($"Successfully deleted {response.DeletedObjects.Count} objects.");
            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine($"Error encountered on server. Message:'{e.Message}' when deleting objects.");
                throw new RestException(System.Net.HttpStatusCode.BadRequest, $"Error encountered on server. Message:'{e.Message}' when deleting objects");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unknown error encountered. Message:'{e.Message}' when deleting objects.");
                throw; // Re-throw the exception to be handled by the caller
            }
        }
        public async Task<Stream> Download(string bucketName, string key)
        {
            var client = new AmazonS3Client(_configuration["ThirdPartyServices:AWS:s3:aws-access-key"], _configuration["ThirdPartyServices:AWS:s3:aws-access-secret"], region: Amazon.RegionEndpoint.APSouth1);

            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = key
            };
            var response = await client.GetObjectAsync(request);
            return response.ResponseStream;
        }
    }
}
