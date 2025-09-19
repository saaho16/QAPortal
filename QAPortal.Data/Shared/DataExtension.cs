using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QAPortal.Data.Repositories;

namespace QAPortal.Data
{
    public static class DataDependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IQuestionsRepo, QuestionsRepo>();
            services.AddScoped<IAnswersRepo, AnswersRepo>();
            services.AddScoped<IApprovalRepo, ApprovalRepo>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}