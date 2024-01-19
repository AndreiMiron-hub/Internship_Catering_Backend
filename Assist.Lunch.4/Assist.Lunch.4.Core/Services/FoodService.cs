using Assist.Lunch._4.Core.Dtos.CommonDtos;
using Assist.Lunch._4.Core.Dtos.FoodDtos;
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
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository foodRepository;
        private readonly IRestaurantRepository restaurantRepository;
        private readonly IMapper mapper;

        public FoodService(IFoodRepository foodRepository,
            IRestaurantRepository restaurantRepository,
            IMapper mapper)
        {
            this.foodRepository = foodRepository;
            this.mapper = mapper;
            this.restaurantRepository = restaurantRepository;
        }

        public async Task<ResponseFoodDto> Add(AddFoodDto addFoodDto)
        {
            var restaurant = await restaurantRepository.GetByIdAsync(addFoodDto.RestaurantId);

            if (restaurant is null)
            {
                throw new KeyNotFoundException(RestaurantResources.NotFound);
            }

            var existingFood = await foodRepository.GetByRestaurantAndNameAsync(addFoodDto.RestaurantId, addFoodDto.Name);

            if (existingFood is not null)
            {
                throw new EntityAlreadyExistsException(FoodResources.FoodAlreadyExists);
            }

            var newFood = mapper.Map<Food>(addFoodDto);

            newFood = await foodRepository.InsertAsync(newFood);

            return mapper.Map<ResponseFoodDto>(newFood);
        }

        public async Task<MessageDto> Delete(Guid foodId)
        {
            var food = await foodRepository.GetByIdAsync(foodId);

            if (food is null)
            {
                throw new KeyNotFoundException(FoodResources.NotFound);
            }

            food.IsDeleted = true;

            await foodRepository.UpdateAsync(food);

            return MessagesConstructor.ReturnMessage(FoodResources.FoodDeleted,
                StatusCodes.Status200OK);
        }

        public async Task<ResponseDto<ResponseFoodDto>> GetAll()
        {
            var foods = await foodRepository.GetAllAsync();

            return mapper.Map<ResponseDto<ResponseFoodDto>>(foods);
        }

        public async Task<ResponseFoodDto> GetById(Guid foodId)
        {
            var food = await foodRepository.GetByIdAsync(foodId);

            if (food is null)
            {
                throw new KeyNotFoundException(FoodResources.NotFound);
            }

            return mapper.Map<ResponseFoodDto>(food);
        }

        public async Task<ResponseDto<ResponseFoodDto>> GetByRestaurant(Guid restaurantId)
        {
            var foods = await foodRepository.GetByRestaurantAsync(restaurantId);

            if (!foods.Any())
            {
                throw new KeyNotFoundException(FoodResources.NotFound);
            }

            return mapper.Map<ResponseDto<ResponseFoodDto>>(foods);
        }

        public async Task<ResponseFoodDto> Update(UpdateFoodDto updateFoodDto)
        {
            var food = await foodRepository.GetByIdAsync(updateFoodDto.Id);

            if (food is null)
            {
                throw new KeyNotFoundException(FoodResources.NotFound);
            }

            food = mapper.Map(updateFoodDto, food);

            await foodRepository.UpdateAsync(food);

            return mapper.Map<ResponseFoodDto>(food);
        }
    }
}
