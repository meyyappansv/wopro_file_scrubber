using Serilog;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace wopro_file_scrubber
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private void scrubFiles_Click(object sender, EventArgs e)
        {
            bool flag1 = false;
            string searchPattern1 = "*_cm.txt";
            string searchPattern2 = "*_pt.txt";
            string path = folderLocation.Text;
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File(path + "\\file_scrubber.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

         
            try
            {
                bool flag2 = false;
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string folderPath = Path.Combine(path, $"File_Scrubber_bkp_{timestamp}");
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                Directory.CreateDirectory(folderPath);
                foreach (FileInfo file in directoryInfo.GetFiles(searchPattern1))
                {
                    string sourceFileName = path + "\\" + file.Name;
                    string destinationFileName = folderPath + "\\" + file.Name;
                    processFile(sourceFileName, destinationFileName);


                }

                foreach (FileInfo file in directoryInfo.GetFiles(searchPattern2))
                {
                    string sourceFileName = path + "\\" + file.Name;
                    string destinationFileName = folderPath + "\\" + file.Name + "_bkp";
                    processFile(sourceFileName, destinationFileName);

                }
                Log.Information("Completed scrubbing all files");
                Log.CloseAndFlush();
                MessageBox.Show("Scrubbing All File Complete");
                folderLocation.Text = "";
            }
            catch (Exception ex)
            {

            }
           
        }
        private void processFile(string sourcePath, string destinationPath)
        {
            Log.Information("***** Start Processing file: " + sourcePath + "*****");
            var lines = new List<string>(File.ReadAllLines(sourcePath));
            var newLines = new List<string>();
            string fileOrder = "";
            
            bool doesFileNeedFixing = false;
            foreach (var line in lines)
            {
                string updatedLine = "";
                if (line.Contains("Checklist Log for Work Order:") && line.Contains("File Order:")) //match condition
                {
                    // Update the matched line
                    Log.Information("Fixing Header line: " + line);
                    doesFileNeedFixing = true;
                    Match match = Regex.Match(line, @"File Order:\s*\d+");
                    if (match.Success)
                    {
                        fileOrder = match.Value; // <-- the full matched text
                       
                       
                    }
                    else
                    {
                        Console.WriteLine("No match found.");
                    }
                   
                    updatedLine = Regex.Replace(line, @"File Order:\s*\d+,", "");
                    string normalized = Regex.Replace(updatedLine, @"\t+", "\t");
                    newLines.Add(normalized);

                    // Add a new line after it
                    newLines.Add(": "+ fileOrder);
                }
                else if (line.Trim().StartsWith("FAI:"))
                {
                    //Making the deviation lines to comment lines, for qcData processor
                    doesFileNeedFixing = true; 
                    updatedLine = Regex.Replace(line, @"FAI:", ": FAI:");
                    Log.Information("Fixing deviation line: " + line);
                    newLines.Add(updatedLine);
                }
                else if (line.Trim().StartsWith("Deviation:"))
                {
                    //Making the deviation lines to comment lines, for qcData processor
                    doesFileNeedFixing = true;
                    updatedLine = Regex.Replace(line, @"Deviation:", ": Deviation:");
                    Log.Information("Fixing deviation line: " + line);
                    newLines.Add(updatedLine);
                }
                else if (line.Trim().StartsWith("Deviation Link:"))
                {
                    //Making the deviation lines to comment lines, for qcData processor
                    doesFileNeedFixing = true;
                    updatedLine = Regex.Replace(line, @"Deviation Link:", ": Deviation Link:");
                    Log.Information("Fixing deviation line: " + line);
                    newLines.Add(updatedLine);
                }
            
                else
                {   
                        newLines.Add(line);
                    
                }
            }


            if (doesFileNeedFixing)
            {

                
                DateTime originalCreationTime = File.GetCreationTime(sourcePath);
                DateTime originalLastAccessTime = File.GetLastAccessTime(sourcePath);
                DateTime originalLastWriteTime = File.GetLastWriteTime(sourcePath);
                File.Copy(sourcePath, destinationPath, overwrite: true);

                File.WriteAllLines(sourcePath, newLines);

                File.SetCreationTime(sourcePath, originalCreationTime);
                File.SetLastAccessTime(sourcePath, originalLastAccessTime);
                File.SetLastWriteTime(sourcePath, originalLastWriteTime);
                Log.Information("Resetting timestamp");
                Log.Information("***** Finished Processing file: " + sourcePath + "*****");
            }
        }
    }
}
