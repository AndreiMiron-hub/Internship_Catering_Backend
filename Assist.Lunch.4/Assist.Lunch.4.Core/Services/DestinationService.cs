using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.DestinationDtos;
using Assist.Lunch._4.Core.Helpers.ExceptionHandler.CustomExceptions;
using Assist.Lunch._4.Core.Helpers.Messages;
using Assist.Lunch._4.Core.Interfaces;
using Assist.Lunch._4.Domain.Entitites;
using Assist.Lunch._4.Infrastructure.Interfaces;
using Assist.Lunch._4.Resources;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Assist.Lunch._4.Core.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly IDestinationRepository destinationRepository;
        private readonly IMapper mapper;

        public DestinationService(IDestinationRepository destinationRepository,
            IMapper mapper)
        {
            this.destinationRepository = destinationRepository;
            this.mapper = mapper;
        }

        public async Task<ResponseDestinationDto> Add(BaseDestinationDto baseDestinationDto)
        {
            var destination = await destinationRepository.GetByNameAsync(baseDestinationDto.Name);

            if (destination is not null)
            {
                throw new EntityAlreadyExistsException(DestinationResources.DestinationAlreadyAdded);
            }

            var newDestination = mapper.Map<Destination>(baseDestinationDto);

            newDestination = await destinationRepository.InsertAsync(newDestination);

            return mapper.Map<ResponseDestinationDto>(newDestination);
        }

        public async Task<MessageDto> Delete(Guid destinationId)
        {
            var destination = await destinationRepository.GetByIdAsync(destinationId);

            if (destination is null)
            {
                throw new KeyNotFoundException(DestinationResources.NotFound);
            }

            destination.IsDeleted = true;

            await destinationRepository.UpdateAsync(destination);

            return MessagesConstructor.ReturnMessage(DestinationResources.DestinationDeleted,
                StatusCodes.Status200OK);
        }

        public async Task<ResponseDto<ResponseDestinationDto>> GetAll()
        {
            var destinations = await destinationRepository.GetAllAsync();

            return mapper.Map<ResponseDto<ResponseDestinationDto>>(destinations);
        }

        public async Task<ResponseDestinationDto> GetById(Guid destinationId)
        {
            var destination = await destinationRepository.GetByIdAsync(destinationId);

            if (destination is null)
            {
                throw new KeyNotFoundException(DestinationResources.NotFound);
            }

            return mapper.Map<ResponseDestinationDto>(destination);
        }

        public async Task<ResponseDestinationDto> Update(DestinationDto destinationDto)
        {
            var destination = await destinationRepository.GetByIdAsync(destinationDto.Id);

            if (destination is null)
            {
                throw new KeyNotFoundException(DestinationResources.NotFound);
            }

            destination = mapper.Map(destinationDto, destination);

            await destinationRepository.UpdateAsync(destination);

            return mapper.Map<ResponseDestinationDto>(destination);
        }
    }
}
