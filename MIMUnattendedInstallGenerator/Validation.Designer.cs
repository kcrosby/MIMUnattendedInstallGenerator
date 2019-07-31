namespace MIMUnattendedInstallGenerator
{
    partial class Validation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.outputTb = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.currentTaskTb = new System.Windows.Forms.TextBox();
            this.errorPb = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorPb)).BeginInit();
            this.SuspendLayout();
            // 
            // outputTb
            // 
            this.outputTb.Location = new System.Drawing.Point(18, 138);
            this.outputTb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.outputTb.Multiline = true;
            this.outputTb.Name = "outputTb";
            this.outputTb.Size = new System.Drawing.Size(822, 533);
            this.outputTb.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 48);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 2;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // currentTaskTb
            // 
            this.currentTaskTb.Location = new System.Drawing.Point(155, 52);
            this.currentTaskTb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.currentTaskTb.Name = "currentTaskTb";
            this.currentTaskTb.Size = new System.Drawing.Size(481, 26);
            this.currentTaskTb.TabIndex = 3;
            this.currentTaskTb.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // errorPb
            // 
            this.errorPb.Image = global::MIMUnattendedInstallGenerator.Properties.Resources.error;
            this.errorPb.Location = new System.Drawing.Point(717, 31);
            this.errorPb.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.errorPb.Name = "errorPb";
            this.errorPb.Size = new System.Drawing.Size(75, 77);
            this.errorPb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.errorPb.TabIndex = 4;
            this.errorPb.TabStop = false;
            this.errorPb.Visible = false;
            // 
            // Validation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 692);
            this.Controls.Add(this.errorPb);
            this.Controls.Add(this.currentTaskTb);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.outputTb);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Validation";
            this.Text = "Validation";
            ((System.ComponentModel.ISupportInitialize)(this.errorPb)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox outputTb;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox currentTaskTb;
        private System.Windows.Forms.PictureBox errorPb;
    }
}