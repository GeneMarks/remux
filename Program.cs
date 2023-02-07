using System.Runtime.InteropServices;
using System.Text;
namespace Remux;

class Program
{
    static void Main(string[] args)
    {
        Logger logger = new Logger();
        try
        {
            string mkvPath = new PathFinder().GetFullPath("mkvmerge.exe");
            if (string.IsNullOrEmpty(mkvPath))
                throw new Exception("mkvmerge not installed as environment variable!");

            new Remuxer().Remux(args);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }
}

class Logger
{
    public static void LogError(Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(ex.Message);
        Console.ResetColor();
    }

    public static void LogMsg(ConsoleColor color, string msg)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(msg);
        Console.ResetColor();
    }
}

class PathFinder
{
    [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = false)]
    static extern bool PathFindOnPath([In, Out] StringBuilder pszFile, [In] string[] ppszOtherDirs);
    
    private const int MAX_PATH = 260;
    public string GetFullPath(string exeName)
    {
        if (exeName.Length >= MAX_PATH)
        {
            throw new ArgumentException($"The executable name '{exeName}' must have less than {MAX_PATH} characters.",
                nameof(exeName));
        }

        StringBuilder sb = new StringBuilder(exeName, MAX_PATH);
        return PathFindOnPath(sb, null) ? sb.ToString() : null;
    }
}
         
