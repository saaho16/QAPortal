using Microsoft.Extensions.DependencyInjection;
using QAPortal.Business.Services;
using QAPortal.Data;
using QAPortal.Business.Mappers;

namespace QAPortal.Business
{
    public static class BusinessDependencyInjection
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services, string connectionString)
        {
            // Register Data layer dependencies
            services.AddDataLayer(connectionString);


            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IApprovalService, ApprovalService>();
            services.AddScoped<IQuestionService, QuestionsService>();
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IJWTServices, JWTService>();

            services.AddAutoMapper(cfg => cfg.AddProfile<UserMapper>());
            services.AddAutoMapper(cfg => cfg.AddProfile<QAMapper>());

            return services;
        }
    }
}