namespace wopro_file_scrubber
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            folderLocation = new TextBox();
            scrubFiles = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(67, 118);
            label1.Name = "label1";
            label1.Size = new Size(101, 15);
            label1.TabIndex = 0;
            label1.Text = "Result Files Folder";
            // 
            // folderLocation
            // 
            folderLocation.Location = new Point(193, 118);
            folderLocation.Name = "folderLocation";
            folderLocation.Size = new Size(153, 23);
            folderLocation.TabIndex = 1;
            // 
            // scrubFiles
            // 
            scrubFiles.Location = new Point(122, 171);
            scrubFiles.Name = "scrubFiles";
            scrubFiles.Size = new Size(118, 23);
            scrubFiles.TabIndex = 2;
            scrubFiles.Text = "Scrub Files";
            scrubFiles.UseVisualStyleBackColor = true;
            scrubFiles.Click += scrubFiles_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(436, 283);
            Controls.Add(scrubFiles);
            Controls.Add(folderLocation);
            Controls.Add(label1);
            Name = "Form1";
            Text = "woPro File Scrubber 2.0.2";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox folderLocation;
        private Button scrubFiles;
    }
}
