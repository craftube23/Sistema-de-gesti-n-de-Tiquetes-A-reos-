using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Sistema_de_gesti_n_de_Tiquetes_Areos_.Data;

public static class DbContextFactory
{
    public static AppDbContext Create()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection");

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString,
                ServerVersion.AutoDetect(connectionString))
            .Options;

        return new AppDbContext(options);
    }
}