using Microsoft.Extensions.DependencyInjection;
using Repository;

namespace regGRPC.Extensions.IOC
{
    public static class RepositoryInjection
    {
        public static IServiceCollection RepositoryIOC(this IServiceCollection services)
        {
            services.AddScoped<ILevelTypeRepository, LevelTypeRepository>();
            return services;
        }
    }
}
