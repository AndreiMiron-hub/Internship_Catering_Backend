using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.RestaurantDtos;
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
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IMapper mapper;

        public RestaurantService(IRestaurantRepository restaurantRepository,
            IMapper mapper)
        {
            this.restaurantRepository = restaurantRepository;
            this.mapper = mapper;
        }

        public async Task<ResponseRestaurantDto> Add(BaseRestaurantDto baseRestaurantDto)
        {
            var restaurant = await restaurantRepository.GetByNameAsync(baseRestaurantDto.Name);

            if (restaurant is not null)
            {
                throw new EntityAlreadyExistsException(RestaurantResources.AlreadyExists);
            }

            var newRestaurant = mapper.Map<Restaurant>(baseRestaurantDto);

            newRestaurant = await restaurantRepository.InsertAsync(newRestaurant);

            return mapper.Map<ResponseRestaurantDto>(newRestaurant);
        }

        public async Task<ResponseDto<ResponseRestaurantDto>> GetAll()
        {
            var restaurants = await restaurantRepository.GetAllAsync();

            return mapper.Map<ResponseDto<ResponseRestaurantDto>>(restaurants);
        }

        public async Task<ResponseRestaurantDto> GetById(Guid id)
        {
            var restaurant = await restaurantRepository.GetByIdAsync(id);

            if (restaurant is null)
            {
                throw new KeyNotFoundException(RestaurantResources.NotFound);
            }

            return mapper.Map<ResponseRestaurantDto>(restaurant);
        }

        public async Task<ResponseRestaurantDto> Update(RestaurantDto restaurantDto)
        {
            var restaurant = await restaurantRepository.GetByIdAsync(restaurantDto.Id);

            if (restaurant is null)
            {
                throw new KeyNotFoundException(RestaurantResources.NotFound);
            }

            restaurant = mapper.Map<Restaurant>(restaurantDto);

            restaurant = await restaurantRepository.UpdateAsync(restaurant);

            return mapper.Map<ResponseRestaurantDto>(restaurant);
        }

        public async Task<MessageDto> Delete(Guid restaurantId)
        {
            var restaurant = await restaurantRepository.GetByIdAsync(restaurantId);

            if (restaurant is null)
            {
                throw new KeyNotFoundException(RestaurantResources.NotFound);
            }

            restaurant.IsDeleted = true;

            await restaurantRepository.UpdateAsync(restaurant);

            return MessagesConstructor.ReturnMessage(RestaurantResources.RestaurantDeleted,
                StatusCodes.Status200OK);
        }
    }
}
