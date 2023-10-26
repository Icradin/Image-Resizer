using System.Drawing;

namespace Image_Resizer
{


    partial class UIController
    {


        System.Drawing.Color darkBackground = ColorTranslator.FromHtml("#101010");

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
        public void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            button1 = new Button();
            checkedListBox1 = new CheckedListBox();
            button2 = new Button();
            button3 = new Button();
            button4 = new Button();
            checkedListBox2 = new CheckedListBox();
            button5 = new Button();
            button6 = new Button();
            button7 = new Button();
            button8 = new Button();
            button9 = new Button();
            button10 = new Button();
            button12 = new Button();
            button13 = new Button();
            button14 = new Button();
            button15 = new Button();
            label3 = new Label();
            button20 = new Button();
            imageList1 = new ImageList(components);
            fontDialog1 = new FontDialog();
            imageList2 = new ImageList(components);
            label1 = new Label();
            label2 = new Label();
            process1 = new System.Diagnostics.Process();
            propertyGrid1 = new PropertyGrid();
            button11 = new Button();
            button18 = new Button();
            textBox1 = new TextBox();
            progressBar1 = new ProgressBar();
            button16 = new Button();
            button17 = new Button();
            button19 = new Button();
            button21 = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new System.Drawing.Point(20, 70);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(512, 512);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new System.Drawing.Point(700, 70);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new System.Drawing.Size(512, 512);
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(20, 1670);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(270, 46);
            button1.TabIndex = 3;
            button1.Text = "Load Images";
            button1.UseVisualStyleBackColor = true;
            button1.Click += OpenImages;
            // 
            // checkedListBox1
            // 
            checkedListBox1.AllowDrop = true;
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.HorizontalScrollbar = true;
            checkedListBox1.Location = new System.Drawing.Point(20, 609);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new System.Drawing.Size(510, 1048);
            checkedListBox1.TabIndex = 4;
            //checkedListBox1.ItemCheck += ChangeOrriginalImagePreview;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(540, 70);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(150, 46);
            button2.TabIndex = 7;
            button2.Text = "2048";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new System.Drawing.Point(540, 131);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(150, 46);
            button3.TabIndex = 8;
            button3.Text = "1024";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new System.Drawing.Point(1230, 1670);
            button4.Name = "button4";
            button4.Size = new System.Drawing.Size(270, 46);
            button4.TabIndex = 9;
            button4.Text = "Open Temp Folder";
            button4.UseVisualStyleBackColor = true;
            button4.Click += OpenTempFolder;
            // 
            // checkedListBox2
            // 
            checkedListBox2.AllowDrop = true;
            checkedListBox2.CheckOnClick = true;
            checkedListBox2.FormattingEnabled = true;
            checkedListBox2.HorizontalScrollbar = true;
            checkedListBox2.Location = new System.Drawing.Point(700, 610);
            checkedListBox2.Name = "checkedListBox2";
            checkedListBox2.Size = new System.Drawing.Size(512, 1048);
            checkedListBox2.TabIndex = 10;
            //checkedListBox2.ItemCheck += ChangeProcessedImagePreview;
            // 
            // button5
            // 
            button5.Location = new System.Drawing.Point(540, 191);
            button5.Name = "button5";
            button5.Size = new System.Drawing.Size(150, 46);
            button5.TabIndex = 12;
            button5.Text = "512";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.Location = new System.Drawing.Point(540, 251);
            button6.Name = "button6";
            button6.Size = new System.Drawing.Size(150, 46);
            button6.TabIndex = 13;
            button6.Text = "265";
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click;
            // 
            // button7
            // 
            button7.Location = new System.Drawing.Point(540, 311);
            button7.Name = "button7";
            button7.Size = new System.Drawing.Size(150, 46);
            button7.TabIndex = 14;
            button7.Text = "128";
            button7.UseVisualStyleBackColor = true;
            button7.Click += button7_Click;
            // 
            // button8
            // 
            button8.Location = new System.Drawing.Point(540, 371);
            button8.Name = "button8";
            button8.Size = new System.Drawing.Size(150, 46);
            button8.TabIndex = 15;
            button8.Text = "64";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // button9
            // 
            button9.Location = new System.Drawing.Point(540, 431);
            button9.Name = "button9";
            button9.Size = new System.Drawing.Size(150, 46);
            button9.TabIndex = 16;
            button9.Text = "32";
            button9.UseVisualStyleBackColor = true;
            button9.Click += button9_Click;
            // 
            // button10
            // 
            button10.Location = new System.Drawing.Point(540, 492);
            button10.Name = "button10";
            button10.Size = new System.Drawing.Size(150, 46);
            button10.TabIndex = 17;
            button10.Text = "16";
            button10.UseVisualStyleBackColor = true;
            button10.Click += button10_Click;
            // 
            // button12
            // 
            button12.Location = new System.Drawing.Point(20, 1790);
            button12.Name = "button12";
            button12.Size = new System.Drawing.Size(270, 46);
            button12.TabIndex = 26;
            button12.Text = "Select Large Images";
            button12.UseVisualStyleBackColor = true;
            button12.Click += SelectLargeImages;
            // 
            // button13
            // 
            button13.Location = new System.Drawing.Point(950, 1670);
            button13.Name = "button13";
            button13.Size = new System.Drawing.Size(260, 46);
            button13.TabIndex = 25;
            button13.Text = "Overwrite Selected";
            button13.UseVisualStyleBackColor = true;
            button13.Click += OverwriteSelected;
            // 
            // button14
            // 
            button14.Location = new System.Drawing.Point(700, 1670);
            button14.Name = "button14";
            button14.Size = new System.Drawing.Size(240, 46);
            button14.TabIndex = 24;
            button14.Text = "Overwrite All";
            button14.UseVisualStyleBackColor = true;
            button14.Click += OverwriteAll;
            // 
            // button15
            // 
            button15.Location = new System.Drawing.Point(20, 1730);
            button15.Name = "button15";
            button15.Size = new System.Drawing.Size(270, 46);
            button15.TabIndex = 23;
            button15.Text = "Select All";
            button15.UseVisualStyleBackColor = true;
            button15.Click += SelectAllImages;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(1230, 20);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(161, 32);
            label3.TabIndex = 27;
            label3.Text = "Main Settings";
            // 
            // button20
            // 
            button20.Location = new System.Drawing.Point(1230, 1730);
            button20.Name = "button20";
            button20.Size = new System.Drawing.Size(270, 46);
            button20.TabIndex = 28;
            button20.Text = "Clear Temp Folder";
            button20.UseVisualStyleBackColor = true;
            button20.Click += ClearTempFolderButton;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth8Bit;
            imageList1.ImageSize = new System.Drawing.Size(16, 16);
            imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList2
            // 
            imageList2.ColorDepth = ColorDepth.Depth8Bit;
            imageList2.ImageSize = new System.Drawing.Size(16, 16);
            imageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(20, 20);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(275, 32);
            label1.TabIndex = 32;
            label1.Text = "Imported image preview";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(700, 30);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(243, 32);
            label2.TabIndex = 33;
            label2.Text = "Edited Image Preview";
            // 
            // process1
            // 
            process1.StartInfo.Domain = "";
            process1.StartInfo.LoadUserProfile = false;
            process1.StartInfo.Password = null;
            process1.StartInfo.StandardErrorEncoding = null;
            process1.StartInfo.StandardInputEncoding = null;
            process1.StartInfo.StandardOutputEncoding = null;
            process1.StartInfo.UserName = "";
            process1.SynchronizingObject = this;
            // 
            // propertyGrid1
            // 
            propertyGrid1.Location = new System.Drawing.Point(1228, 66);
            propertyGrid1.Name = "propertyGrid1";
            propertyGrid1.Size = new System.Drawing.Size(512, 514);
            propertyGrid1.TabIndex = 0;
            propertyGrid1.PropertyValueChanged += GridValueLimiter;
            propertyGrid1.Click += propertyGrid1_Click;
            // 
            // button11
            // 
            button11.Location = new System.Drawing.Point(700, 1730);
            button11.Name = "button11";
            button11.Size = new System.Drawing.Size(240, 46);
            button11.TabIndex = 37;
            button11.Text = "Restore All";
            button11.UseVisualStyleBackColor = true;
            button11.Click += RestoreAll;
            // 
            // button18
            // 
            button18.Location = new System.Drawing.Point(950, 1730);
            button18.Name = "button18";
            button18.Size = new System.Drawing.Size(260, 46);
            button18.TabIndex = 39;
            button18.Text = "Restore Last Selection";
            button18.UseVisualStyleBackColor = true;
            button18.Click += RestoreLastSelection;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(1230, 610);
            textBox1.MaximumSize = new System.Drawing.Size(512, 2000);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ScrollBars = ScrollBars.Both;
            textBox1.Size = new System.Drawing.Size(512, 1048);
            textBox1.TabIndex = 40;
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(20, 1850);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(1720, 46);
            progressBar1.TabIndex = 42;
            // 
            // button16
            // 
            button16.Location = new System.Drawing.Point(540, 550);
            button16.Name = "button16";
            button16.Size = new System.Drawing.Size(150, 46);
            button16.TabIndex = 44;
            button16.Text = "META";
            button16.UseVisualStyleBackColor = true;
            // 
            // button17
            // 
            button17.Location = new System.Drawing.Point(540, 610);
            button17.Name = "button17";
            button17.Size = new System.Drawing.Size(150, 46);
            button17.TabIndex = 45;
            button17.Text = "Meta Test";
            button17.UseVisualStyleBackColor = true;
            
            // 
            // button19
            // 
            button19.Location = new System.Drawing.Point(300, 1670);
            button19.Name = "button19";
            button19.Size = new System.Drawing.Size(270, 46);
            button19.TabIndex = 46;
            button19.Text = "Select All TGA";
            button19.UseVisualStyleBackColor = true;
            button19.Click += SelectTGA;
            // 
            // button21
            // 
            button21.Location = new System.Drawing.Point(540, 730);
            button21.Name = "button21";
            button21.Size = new System.Drawing.Size(150, 46);
            button21.TabIndex = 47;
            button21.Text = "Meta Test";
            button21.UseVisualStyleBackColor = true;
            button21.Click += CallLoadImages;
            // 
            // ImageResizer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1754, 1910);
            Controls.Add(button21);
            Controls.Add(button19);
            Controls.Add(button17);
            Controls.Add(button16);
            Controls.Add(progressBar1);
            Controls.Add(textBox1);
            Controls.Add(button18);
            Controls.Add(button11);
            Controls.Add(propertyGrid1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button20);
            Controls.Add(label3);
            Controls.Add(button12);
            Controls.Add(button13);
            Controls.Add(button14);
            Controls.Add(button15);
            Controls.Add(button10);
            Controls.Add(button9);
            Controls.Add(button8);
            Controls.Add(button7);
            Controls.Add(button6);
            Controls.Add(button5);
            Controls.Add(checkedListBox2);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(checkedListBox1);
            Controls.Add(button1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Name = "ImageResizer";
            Text = "Mass Texture Size Reducer";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion








        public PictureBox pictureBox1;
        public PictureBox pictureBox2;
        public Button button1;
        public CheckedListBox checkedListBox1;
        public Button button2;
        public Button button3;
        public Button button4;
        public CheckedListBox checkedListBox2;
        public Button button5;
        public Button button6;
        public Button button7;
        public Button button8;
        public Button button9;
        public Button button10;
        public Button button12;
        public Button button13;
        public Button button14;
        public Button button15;
        public Label label3;
        public Button button20;
        public ImageList imageList1;
        public FontDialog fontDialog1;
        public ImageList imageList2;
        public Label label1;
        public Label label2;
        public System.Diagnostics.Process process1;
        public PropertyGrid propertyGrid1;
        public Button button18;
        public Button button11;
        public TextBox textBox1;
        public ProgressBar progressBar1;
        public Button button16;
        public Button button17;
        public Button button19;
        private System.ComponentModel.IContainer components;
        public Button button21;
    }
}