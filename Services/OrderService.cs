using NurseryMart.Communication;
using NurseryMart.Contract;
using NurseryMart.Entities;
using NurseryMart.FileManagement;
using NurseryMart.IRepository;
using NurseryMart.Services.Abstraction;
using NurseryMart.Utility;
using System.Security.Policy;

namespace NurseryMart.Services
{
    public class OrderService : IOrderService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ICommunicationService _communicationService;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        public OrderService(IRepositoryManager repositoryManager ,ICommunicationService communicationService ,IConfiguration configuration , IFileService fileService )
        {
            _repositoryManager = repositoryManager;
            _communicationService = communicationService;
            _configuration = configuration;
            _fileService = fileService;
        }

        public AccountResponseDto Account { get; set; }

        public async Task<dynamic> CreateOrder(CreateOrderDto entity , CancellationToken cancellationToken)
        {
           if(entity == null) throw new RestException(System.Net.HttpStatusCode.BadRequest,ErrorConstant.InvalidInput);
           if(entity.Quantity <= 0) throw new RestException(System.Net.HttpStatusCode.BadRequest, ErrorConstant.InvalidInput);
           var product = await _repositoryManager.ProductRepository.FindOneAsync(_ => _.ProductId == entity.ProductId , cancellationToken);
           var totalAmount = product.Price * entity.Quantity;
            var newOrder = new OrderDetails
            {
                ProductId = entity.ProductId,
                Trail = new Trail { CreatedBy = Account.Id, CreatedOn = DateTimeOffset.UtcNow, IsActive = true },
                Discount = product.DiscountPrice ?? 0,
                SubTotal = totalAmount,
                Quantity = entity.Quantity,
                Mobile = entity.Mobile,
                Address = entity.Address,
                Pincode = entity.Pincode,
                AlternateMobile = entity.AlterNateMobileNumber,
                CustomerId = Account.Id,
            };
            await _repositoryManager.OrderDetailsRepository.CreateOneAsync(newOrder,cancellationToken);
            return new ResponseDto(newOrder , 1,"Order Placed Successfully" ,"success");
        }

        public async Task<dynamic> SendMessage( CancellationToken cancellationToken)
        {
            dynamic dynamicObject = new Dictionary<string, object>();
            var newWhatsappProvider = new WhatsappProviderDto
            {
                ContentId = "",
                dynamicObject = dynamicObject,
                MobNo = "7350633397"
            };
            await _communicationService.SendTwilioWhatsAppMessage(newWhatsappProvider);
            return new ResponseDto(true, 1, "Order Placed Successfully", "success");
        }


        public async Task<dynamic> UploadMedia(IList<IFormFile> fileMedias, CancellationToken cancellationToken = default)
        {
            if (!fileMedias.Any()) throw new RestException(System.Net.HttpStatusCode.BadRequest, "Invalid Inputs.");
            var resourcesBucket = _configuration["ThirdPartyServices:AWS:s3:resource-bucket"];
            var awsBucketBaseUrl = _configuration["ThirdPartyServices:AWS:s3:base-url"];
            var returnFilePaths = new List<dynamic>();
            foreach (var media in fileMedias)
            {
                using (var stream = new MemoryStream())
                {
                    var key = $"media/{Guid.NewGuid()}{Path.GetExtension(media.FileName)}";
                    await media.CopyToAsync(stream);
                    await _fileService.UploadToS3(stream, resourcesBucket, key);
                    returnFilePaths.Add(new { url = $"{string.Format(awsBucketBaseUrl, resourcesBucket)}{key}", contentType = media.ContentType });

                }
            }
            return new ResponseDto(returnFilePaths, returnFilePaths.Count, "media uploaded successfully", "succuess");
        }

        public async Task<dynamic> GetFileFromS3( string key ,CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(key)) throw new RestException(System.Net.HttpStatusCode.BadRequest, "Invalid Inputs.");
            var resourcesBucket = _configuration["ThirdPartyServices:AWS:s3:resource-bucket"];
            var awsBucketBaseUrl = _configuration["ThirdPartyServices:AWS:s3:base-url"];
            var returnFilePaths = new List<dynamic>();
            var data = await _fileService.DownloadFromS3(resourcesBucket, key);
            return new ResponseDto(data,1 ,"data fetched successfully","success");
        }





    }
}
