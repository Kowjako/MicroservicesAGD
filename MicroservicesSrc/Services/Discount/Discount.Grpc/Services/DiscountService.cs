using AutoMapper;
using Discount.Grpc.Protos;
using Discount.Grpc.Repository;
using Grpc.Core;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repo;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;

        public DiscountService(IDiscountRepository repo, ILogger<DiscountService> logger, IMapper mapper)
            => (_repo, _logger, _mapper) = (repo, logger, mapper);

        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request,
            ServerCallContext context)
        {

        }

        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request,
            ServerCallContext context)
        {

        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repo.GetDiscount(request.ProductName);
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Can't find specific discount"));
            }
            _logger.LogInformation($"Discount was retrieved for productName = {request.ProductName}");
            return _mapper.Map<CouponModel>(coupon);
        }

        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request,
            ServerCallContext context)
        {

        }
    }
}
