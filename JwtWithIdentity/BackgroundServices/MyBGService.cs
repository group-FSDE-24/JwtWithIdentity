
namespace JwtWithIdentity.BackgroundServices;

public class MyBGService : IHostedService
{
    // ////////////////////////////////////////////////////////////////

    // // Example 1: 

    // public Task StartAsync(CancellationToken cancellationToken)
    // {
    //     Console.WriteLine("BgService started...");
    //     return Task.CompletedTask;
    // }
    // 
    // public Task StopAsync(CancellationToken cancellationToken)
    // {
    //     Console.WriteLine("BgService stoped...");
    //     return Task.CompletedTask;
    // }

    // ////////////////////////////////////////////////////////////////
    
    // ////////////////////////////////////////////////////////////////

    // BgService with Timer

    private Timer _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("BgService started...");
        _timer = new Timer(Run, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(3));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Dispose();
        Console.WriteLine("BgService stoped...");
        return Task.CompletedTask;
    }

    private void Run(object? obj)
    {
        Console.WriteLine("Ay Aydin, Ay Aydin");
    }

    // ////////////////////////////////////////////////////////////////


}
