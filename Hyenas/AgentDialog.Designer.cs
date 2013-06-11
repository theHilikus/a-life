namespace Hyenas {
    partial class AgentDialog {
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
            this.agentPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.refreshButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // agentPropertyGrid
            // 
            this.agentPropertyGrid.Location = new System.Drawing.Point(12, 12);
            this.agentPropertyGrid.Name = "agentPropertyGrid";
            this.agentPropertyGrid.Size = new System.Drawing.Size(221, 316);
            this.agentPropertyGrid.TabIndex = 0;
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(77, 334);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // AgentDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(245, 369);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.agentPropertyGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "AgentDialog";
            this.Text = "Agent Parameters";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid agentPropertyGrid;
        private System.Windows.Forms.Button refreshButton;
    }
}