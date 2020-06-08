using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Store.Data.EF
{
    public static class ServerCollectionExtensions
    {
        public static IServiceCollection AddEfRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<StoreDbContext>(
                options =>
                {
                    options.UseSqlServer(connectionString);
                },
                ServiceLifetime.Transient);

            services.AddScoped<IBookRepository, IBookRepository>();
            services.AddScoped<IOrderRepository, IOrderRepository>();

            return services;
        }
    }
}
