namespace Hyenas {
    partial class GUI {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mainArea = new System.Windows.Forms.Panel();
            this.statusBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.stepButton = new System.Windows.Forms.Button();
            this.restartButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // mainArea
            // 
            this.mainArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.mainArea.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.mainArea.Location = new System.Drawing.Point(40, 52);
            this.mainArea.Name = "mainArea";
            this.mainArea.Size = new System.Drawing.Size(661, 666);
            this.mainArea.TabIndex = 0;
            // 
            // statusBox
            // 
            this.statusBox.Location = new System.Drawing.Point(725, 533);
            this.statusBox.Multiline = true;
            this.statusBox.Name = "statusBox";
            this.statusBox.Size = new System.Drawing.Size(210, 169);
            this.statusBox.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(299, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(390, 10);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // stepButton
            // 
            this.stepButton.Location = new System.Drawing.Point(524, 10);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(48, 23);
            this.stepButton.TabIndex = 4;
            this.stepButton.Text = "Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.stepButton_Click);
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(596, 10);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(51, 23);
            this.restartButton.TabIndex = 5;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(741, 52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(149, 106);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "World";
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(947, 750);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.stepButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.statusBox);
            this.Controls.Add(this.mainArea);
            this.Name = "GUI";
            this.Text = "A-Life: Hyenas";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel mainArea;
        private System.Windows.Forms.TextBox statusBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button stepButton;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}

