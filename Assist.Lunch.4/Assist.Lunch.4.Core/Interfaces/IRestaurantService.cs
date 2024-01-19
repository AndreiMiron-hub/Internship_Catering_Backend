using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.RestaurantDtos;

namespace Assist.Lunch._4.Core.Interfaces
{
    public interface IRestaurantService
    {
        Task<ResponseRestaurantDto> Add(BaseRestaurantDto baseRestaurantDto);
        Task<MessageDto> Delete(Guid restaurantId);
        Task<ResponseDto<ResponseRestaurantDto>> GetAll();
        Task<ResponseRestaurantDto> GetById(Guid id);
        Task<ResponseRestaurantDto> Update(RestaurantDto restaurantDto);
    }
}
