using Discount.Grpc.Protos;

namespace Basket.API.gRPC
{
    public class DiscountGrpcProxy
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _grpcClient;

        public DiscountGrpcProxy(DiscountProtoService.DiscountProtoServiceClient grpcClient)
            => _grpcClient = grpcClient;

        public async Task<CouponModel> GetDiscount(string productName)
        {
            return await _grpcClient.GetDiscountAsync(new GetDiscountRequest { ProductName = productName });
        }
    }
}
