using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.DestinationDtos;

namespace Assist.Lunch._4.Core.Interfaces
{
    public interface IDestinationService
    {
        Task<ResponseDestinationDto> Add(BaseDestinationDto baseDestinationDto);
        Task<MessageDto> Delete(Guid destinationId);
        Task<ResponseDto<ResponseDestinationDto>> GetAll();
        Task<ResponseDestinationDto> GetById(Guid destinationId);
        Task<ResponseDestinationDto> Update(DestinationDto destinationDto);
    }
}
