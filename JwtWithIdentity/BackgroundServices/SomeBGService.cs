


using JwtWithIdentity.Datas;
using Microsoft.EntityFrameworkCore;

namespace JwtWithIdentity.BackgroundServices;

public class SomeBGService : BackgroundService
{

    // ////////////////////////////////////////////////////////////////

    // // Simple Version

    // protected override Task ExecuteAsync(CancellationToken stoppingToken)
    // {
    //     Console.WriteLine("SomeBGService started...");
    // 
    //     return Task.CompletedTask;
    // }

    // ////////////////////////////////////////////////////////////////
    
    // ////////////////////////////////////////////////////////////////

    // // TIMER Example 

    private Timer _timer;
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public SomeBGService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Execute ise baslayir");
        _timer = new Timer(Run, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("BG service ise basladi...");
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Bg service isini yekunlasdirdi...");
        return base.StopAsync(cancellationToken);
    }

    private void Run(object? obj)
    {
        Console.WriteLine("Hakuna Matata");
    }

    ////////////////////////////////////////////////////////////////

}
