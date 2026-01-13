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
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.btnGetExposureRange = new Sunny.UI.UIButton();
            this.nudExposure = new System.Windows.Forms.NumericUpDown();
            this.btnRefresh = new Sunny.UI.UIButton();
            this.cbCamera = new Sunny.UI.UIComboBox();
            this.btnReadQR = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudExposure)).BeginInit();
            this.SuspendLayout();
            // 
            // hWindowControl1
            // 
            this.hWindowControl1.BackColor = System.Drawing.Color.Black;
            this.hWindowControl1.BorderColor = System.Drawing.Color.Black;
            this.hWindowControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hWindowControl1.ImagePart = new System.Drawing.Rectangle(0, 0, 640, 480);
            this.hWindowControl1.Location = new System.Drawing.Point(0, 0);
            this.hWindowControl1.Name = "hWindowControl1";
            this.hWindowControl1.Size = new System.Drawing.Size(838, 452);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(838, 452);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1016, 452);
            this.panel1.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.hWindowControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(838, 452);
            this.panel3.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.uiLabel1);
            this.panel2.Controls.Add(this.btnGetExposureRange);
            this.panel2.Controls.Add(this.nudExposure);
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Controls.Add(this.cbCamera);
            this.panel2.Controls.Add(this.btnReadQR);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(838, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(178, 452);
            this.panel2.TabIndex = 6;
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(26, 134);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(123, 23);
            this.uiLabel1.TabIndex = 8;
            this.uiLabel1.Text = "Exposure (µs):\n\n\n\n";
            // 
            // btnGetExposureRange
            // 
            this.btnGetExposureRange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetExposureRange.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetExposureRange.Location = new System.Drawing.Point(42, 202);
            this.btnGetExposureRange.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnGetExposureRange.Name = "btnGetExposureRange";
            this.btnGetExposureRange.Size = new System.Drawing.Size(100, 35);
            this.btnGetExposureRange.TabIndex = 7;
            this.btnGetExposureRange.Text = "Get Range";
            this.btnGetExposureRange.TipsFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetExposureRange.Click += new System.EventHandler(this.btnGetExposureRange_Click);
            // 
            // nudExposure
            // 
            this.nudExposure.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudExposure.Location = new System.Drawing.Point(29, 159);
            this.nudExposure.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nudExposure.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudExposure.Name = "nudExposure";
            this.nudExposure.Size = new System.Drawing.Size(120, 21);
            this.nudExposure.TabIndex = 6;
            this.nudExposure.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudExposure.ValueChanged += new System.EventHandler(this.nudExposure_ValueChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.btnRefresh.Location = new System.Drawing.Point(31, 49);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 32);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "uiButton1";
            this.btnRefresh.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cbCamera
            // 
            this.cbCamera.DataSource = null;
            this.cbCamera.FillColor = System.Drawing.Color.White;
            this.cbCamera.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cbCamera.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cbCamera.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cbCamera.Location = new System.Drawing.Point(9, 14);
            this.cbCamera.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbCamera.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbCamera.Name = "cbCamera";
            this.cbCamera.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbCamera.Size = new System.Drawing.Size(140, 27);
            this.cbCamera.SymbolSize = 24;
            this.cbCamera.TabIndex = 4;
            this.cbCamera.Text = "uiComboBox1";
            this.cbCamera.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cbCamera.Watermark = "";
            this.cbCamera.SelectedIndexChanged += new System.EventHandler(this.cbCamera_SelectedIndexChanged);
            // 
            // btnReadQR
            // 
            this.btnReadQR.Location = new System.Drawing.Point(42, 110);
            this.btnReadQR.Name = "btnReadQR";
            this.btnReadQR.Size = new System.Drawing.Size(75, 21);
            this.btnReadQR.TabIndex = 3;
            this.btnReadQR.Text = "QR";
            this.btnReadQR.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 452);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudExposure)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private HalconDotNet.HWindowControl hWindowControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnReadQR;
        private Sunny.UI.UIComboBox cbCamera;
        private Sunny.UI.UIButton btnRefresh;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIButton btnGetExposureRange;
        private System.Windows.Forms.NumericUpDown nudExposure;
    }
}

