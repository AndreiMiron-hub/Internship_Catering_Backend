using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.FoodDtos;

namespace Assist.Lunch._4.Core.Interfaces
{
    public interface IFoodService
    {
        Task<ResponseFoodDto> Add(AddFoodDto addFoodDto);
        Task<MessageDto> Delete(Guid foodId);
        Task<ResponseDto<ResponseFoodDto>> GetAll();
        Task<ResponseFoodDto> GetById(Guid foodId);
        Task<ResponseDto<ResponseFoodDto>> GetByRestaurant(Guid restaurantId);
        Task<ResponseFoodDto> Update(UpdateFoodDto updateFoodDto);
    }
}
