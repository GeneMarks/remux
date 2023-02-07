namespace Remux;

class Command
{
    public List<string> validFileExts = new List<string>() {
        ".mp4",
        ".mov",
        ".wmv",
        ".avi",
        ".flv",
        ".webm"
    };
    public bool   isValidCommand;
    public string input;
    public bool   isDir;
    public bool   isRecursive;

    public List<string> GetVideoFiles()
    {
        List<string> files = new List<string>();

        foreach (string ext in validFileExts)
        {
            if (isRecursive)
            {
                files.AddRange(Directory.GetFiles(
                    input,
                    "*" + ext,
                    SearchOption.AllDirectories
                ));
            } 
            else
                files.AddRange(Directory.GetFiles(input, "*" + ext));
        }

        return files;
    }

    public void ParseCommand(string[] args)
    {
        try
        {
            input       = args.Length >= 1 ? @args[0] : null;
            isDir       = Directory.Exists(input);
            isRecursive = args.Length > 1 && (@args[1] == "-r" || @args[1] == "--recursive") ? true : false;

            if (string.IsNullOrEmpty(input))
                throw new Exception("Please provide a file or folder input.");
            
            if (args.Length > 2)
                throw new Exception("Error: Too many arguments. Only supply the path and optional recursive flag.");

            if (!isDir)
            {
                if (!File.Exists(input))
                    throw new Exception("Error: The provided path is not a valid file or folder.");
                else if (!validFileExts.Any(x => x.Equals(Path.GetExtension(input), 
                            StringComparison.InvariantCultureIgnoreCase)))
                    throw new Exception("Error: The provided file is not a valid video file.");
            }
            else if (!GetVideoFiles().Any())
                throw new Exception("Error: The provided folder/s contains no valid video files.");

            if (args.Length == 2 && !isRecursive)
                throw new Exception("Error: The only available flag is '-r' or '--recursive'");
            else if (isRecursive && !isDir)
                throw new Exception("Error: Only folders can use the recursive flag.");
  
            isValidCommand = true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }
}
