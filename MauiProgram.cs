using CommunityToolkit.Maui;
using Managing.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Managing; // Ensure 'App' is defined in this namespace
using Microsoft.EntityFrameworkCore.Sqlite; // Add this using directive
using Managing.Services;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
          .UseMauiApp<App>() // Ensure 'App' class exists in 'Managing' namespace
          .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                // your font settings
            });

        // Configurar EF Core SQLite:
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "managing.db3");
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlite($"Data Source={dbPath}")); // UseSqlite is in Microsoft.EntityFrameworkCore.Sqlite

        // Register your services
        builder.Services.AddTransient<ProductService>();
        builder.Services.AddTransient<ReceiptPageModel>();
        // ... etc.

        return builder.Build();
    }
}