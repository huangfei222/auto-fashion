namespace Genesis.Engine.Core.Logging;

public static class Logger
{
    public static void Info(string message)
    {
        Console.WriteLine($"[Info] {message}");
    }
}