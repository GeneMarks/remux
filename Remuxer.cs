using Microsoft.VisualBasic.FileIO;
namespace Remux;

class Remuxer
{
    public void Remux(string[] args)
    {
        Command command = new Command();
        command.ParseCommand(args);

        if (command.isValidCommand)
        {
            if (command.isDir)
                foreach (string f in command.GetVideoFiles()) Process(f);
            else
                Process(command.input);

            Logger.LogMsg(
                ConsoleColor.Green,
                "Done!"
            );
        }

        void Process(string inFile)
        {
            string outFile()
            {
                string dir = Path.GetDirectoryName(inFile);
                string filename = Path.GetFileNameWithoutExtension(inFile);

                return dir + "\\" + filename + ".mkv";
            }
            string remuxCommand = "/C mkvmerge -o \"" + outFile() + "\" \"" + inFile + "\"";

            try
            {
                // run mkvmerge
                System.Diagnostics.Process.Start("cmd.exe", remuxCommand).WaitForExit();

                // janky check to see if remux was "successful" before deleting src
                if (File.Exists(outFile()))
                {
                    Logger.LogMsg(
                        ConsoleColor.Yellow,
                        "Deleting input file: " + inFile
                    );
                    FileSystem.DeleteFile(
                        inFile,
                        UIOption.OnlyErrorDialogs,
                        RecycleOption.SendToRecycleBin
                    );
                }
                else
                    throw new Exception("Unable to delete input file.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }
    }
}