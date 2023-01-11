using Microsoft.VisualBasic.FileIO;
namespace Remux;

class Remuxer
{
    string input;
    string dir;
    string filename;

    public void Remux(string[] args)
    {
        Command command = new Command();
        command.ParseCommand(args);

        if (command.isValidCommand)
        {
            input = command.input;
            dir = Path.GetDirectoryName(input);
            filename = Path.GetFileNameWithoutExtension(input);
            string remuxCommand;

            if (command.isDir)
            {
                foreach (string f in command.GetVideoFiles())
                {
                    dir = Path.GetDirectoryName(f);
                    filename = Path.GetFileNameWithoutExtension(f);
                    remuxCommand = "/C mkvmerge -o \"" + dir + "\\" + filename + ".mkv\" \"" + f + "\"";
                    
                    System.Diagnostics.Process.Start("cmd.exe", remuxCommand).WaitForExit();
                    Cleanup(f);
                }
            }
            else
            {
                remuxCommand = "/C mkvmerge -o \"" + dir + "\\" + filename + ".mkv\" \"" + input + "\"";
                
                System.Diagnostics.Process.Start("cmd.exe", remuxCommand).WaitForExit();
                Cleanup(input);
            }

            Logger.LogMsg(
                ConsoleColor.Green,
                "Done!"
            );
        }
    }

    private void Cleanup(string fileToDel)
    {
        try
        {
            // janky check to see if remux was "successful" before deleting src
            if (File.Exists(dir + "\\" + filename + ".mkv"))
            {
                Logger.LogMsg(
                    ConsoleColor.Yellow,
                    "Deleting input file: " + fileToDel
                );
                FileSystem.DeleteFile(
                    fileToDel,
                    UIOption.AllDialogs,
                    RecycleOption.SendToRecycleBin
                );
            }
            else
            {
                throw new Exception("Unable to delete input file.");
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }
}