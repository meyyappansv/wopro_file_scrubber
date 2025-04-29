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
            try
            {
                bool flag2 = false;
                DirectoryInfo directoryInfo = new DirectoryInfo(path);
                foreach (FileInfo file in directoryInfo.GetFiles(searchPattern1))
                {
                    string path1 = path + "\\" + file.Name;
                    File.Copy(path1, path1+"_bkp", overwrite: true);
                    processFile(path1);


                }

                foreach (FileInfo file in directoryInfo.GetFiles(searchPattern2))
                {
                    string path2 = path + "\\" + file.Name;
                    File.Copy(path2, path2 + "_bkp", overwrite: true);
                    processFile(path2);

                }


                MessageBox.Show("Scrubbing All File Complete");
                folderLocation.Text = "";
            }
            catch (Exception ex)
            {

            }
           
        }
        private void processFile(string path)
        {
            
            var lines = new List<string>(File.ReadAllLines(path));
            var newLines = new List<string>();
            string fileOrder = "";

            foreach (var line in lines)
            {
                if (line.Contains("Checklist Log for Work Order:") && line.Contains("File Order:")) //match condition
                {
                    // Update the matched line
                    Match match = Regex.Match(line, @"File Order:\s*\d+");
                    if (match.Success)
                    {
                        fileOrder = match.Value; // <-- the full matched text
                       
                       
                    }
                    else
                    {
                        Console.WriteLine("No match found.");
                    }
                   
                    string updatedLine = Regex.Replace(line, @"File Order:\s*\d+,", "");
                    string normalized = Regex.Replace(updatedLine, @"\t+", "\t");
                    newLines.Add(normalized);

                    // Add a new line after it
                    newLines.Add(": "+ fileOrder);
                }
                else
                {
                    newLines.Add(line);
                }
            }

            File.WriteAllLines(path, newLines);

            Console.WriteLine("Update complete!");
        }
    }
}
