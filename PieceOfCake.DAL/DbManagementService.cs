using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PieceOfCake.DAL;

public class DbManagementService
{
    public static void MigrationInitialization(IApplicationBuilder app)
    {
        using(var serviceScope = app.ApplicationServices.CreateScope())
        {
            serviceScope.ServiceProvider.GetService<PocDbContext>().Database.Migrate();
        }
    }
}
