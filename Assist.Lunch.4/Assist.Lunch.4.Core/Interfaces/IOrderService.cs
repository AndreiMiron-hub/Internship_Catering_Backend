using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.OrderDtos;

namespace Assist.Lunch._4.Core.Interfaces
{
    public interface IOrderService
    {
        Task<ResponseOrderDto> Add(BaseOrderDto baseOrderDto);
        Task<MessageDto> Delete(Guid orderId);
        Task<ResponseDto<ResponseOrderDto>> GetAll();
        Task<ResponseOrderDto> GetById(Guid orderId);
        Task<ResponseDto<ResponseOrderDto>> GetByUser(Guid userId);
        Task<ResponseOrderHistoryDto> GetHistoryByUser(RequestOrderHistoryDto requestOrderHistoryDto);
        Task<ResponseDto<ResponseOrderDto>> GetTodaysOrders();
        Task<ResponseOrderDto> GetTodaysOrderByUser(Guid userId);
        Task<ResponseOrderDto> Update(UpdateOrderDto updatedOrderDto);
    }
}
