namespace WindowsFormsApp1
{
    partial class Form1
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
            this.hWindowControl1 = new HalconDotNet.HWindowControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnReadQR = new System.Windows.Forms.Button();
            this.cbCamera = new Sunny.UI.UIComboBox();
            this.btnRefresh = new Sunny.UI.UIButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(21, 12);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(966, 457);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(966, 457);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnRefresh);
            this.panel1.Controls.Add(this.cbCamera);
            this.panel1.Controls.Add(this.btnReadQR);
            this.panel1.Controls.Add(this.hWindowControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1156, 554);
            this.panel1.TabIndex = 3;
            // 
            // btnReadQR
            // 
            this.btnReadQR.Location = new System.Drawing.Point(42, 512);
            this.btnReadQR.Name = "btnReadQR";
            this.btnReadQR.Size = new System.Drawing.Size(75, 23);
            this.btnReadQR.TabIndex = 3;
            this.btnReadQR.Text = "QR";
            this.btnReadQR.UseVisualStyleBackColor = true;
            // 
            // cbCamera
            // 
            this.cbCamera.DataSource = null;
            this.cbCamera.FillColor = System.Drawing.Color.White;
            this.cbCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cbCamera.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cbCamera.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cbCamera.Location = new System.Drawing.Point(1003, 36);
            this.cbCamera.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbCamera.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbCamera.Name = "cbCamera";
            this.cbCamera.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbCamera.Size = new System.Drawing.Size(140, 29);
            this.cbCamera.SymbolSize = 24;
            this.cbCamera.TabIndex = 4;
            this.cbCamera.Text = "uiComboBox1";
            this.cbCamera.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbCamera.Watermark = "";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnRefresh.Location = new System.Drawing.Point(159, 500);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 35);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "uiButton1";
            this.btnRefresh.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1156, 554);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnReadQR;
        private Sunny.UI.UIComboBox cbCamera;
        private Sunny.UI.UIButton btnRefresh;
    }
}

