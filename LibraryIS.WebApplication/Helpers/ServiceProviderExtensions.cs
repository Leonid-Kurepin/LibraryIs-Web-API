using LibraryIS.BLL.Services;
using LibraryIS.BLL.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryIS.WebApplication.Helpers
{
    public static class ServiceProviderExtensions
    {
        public static void AddUserService(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }

        public static void AddMemberService(this IServiceCollection services)
        {
            services.AddScoped<IMemberService, MemberService>();
        }

        public static void AddBookService(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();

        }
    }
}