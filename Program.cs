using Azure.Storage.Blobs;
using Azure.Data.Tables;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using ST10158190Part1_CLDV_B.Services;

namespace ST10158190Part1_CLDV_B
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

           
            var connectionString = builder.Configuration.GetConnectionString("AzureStorage");
            builder.Services.AddSingleton<BlobService>();
            builder.Services.AddSingleton<TableService>();          
            builder.Services.AddSingleton<FileService>();
            builder.Services.AddSingleton<QueueService>();

            
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
            
        }
    }
}