// using Microsoft.EntityFrameworkCore;
// using Microsoft.EntityFrameworkCore.Design;
// using Microsoft.Extensions.Configuration;


// namespace QAPortal.Data
// {
//     public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//     {
//         public AppDbContext CreateDbContext(string[] args)
//         {
//             // Load configuration from appsettings.json
//             IConfigurationRoot configuration = new ConfigurationBuilder()
//                 .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../QAPortal.Presentation"))
//                 .AddJsonFile("appsettings.json")
//                 .Build();

//             var connectionString = configuration.GetConnectionString("DefaultConnection");

//             var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//             optionsBuilder.UseSqlServer(connectionString);

//             return new AppDbContext(optionsBuilder.Options);
            
//         }
//     }
// }