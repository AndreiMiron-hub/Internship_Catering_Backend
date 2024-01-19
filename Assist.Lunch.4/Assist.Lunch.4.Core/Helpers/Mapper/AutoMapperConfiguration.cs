using Assist.Lunch._4.Core.Helpers.Mapper.MappingProfiles;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Assist.Lunch._4.Core.Helpers.Mapper
{
    public static class AutoMapperConfiguration
    {
        public static void AdCustomConfiguredAutoMapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DestinationProfileMapper());
                cfg.AddProfile(new FoodProfileMapper());
                cfg.AddProfile(new OrderProfileMapper());
                cfg.AddProfile(new UserProfileMapper());
                cfg.AddProfile(new RestaurantProfileMapper());
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
