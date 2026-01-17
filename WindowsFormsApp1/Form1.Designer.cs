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
            this.btnReadQR = new Sunny.UI.UIButton();
            this.btnLoadRoi = new Sunny.UI.UIButton();
            this.lbRoi = new Sunny.UI.UIListBox();
            this.btnSave = new Sunny.UI.UIButton();
            this.btnDrawRoi = new Sunny.UI.UIButton();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.btnGetExposureRange = new Sunny.UI.UIButton();
            this.nudExposure = new System.Windows.Forms.NumericUpDown();
            this.btnRefresh = new Sunny.UI.UIButton();
            this.cbCamera = new Sunny.UI.UIComboBox();
            this.uiContextMenuStrip1 = new Sunny.UI.UIContextMenuStrip();
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
            this.hWindowControl1.Size = new System.Drawing.Size(905, 499);
            this.hWindowControl1.TabIndex = 0;
            this.hWindowControl1.WindowSize = new System.Drawing.Size(905, 499);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1083, 499);
            this.panel1.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.hWindowControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(905, 499);
            this.panel3.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnReadQR);
            this.panel2.Controls.Add(this.btnLoadRoi);
            this.panel2.Controls.Add(this.lbRoi);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnDrawRoi);
            this.panel2.Controls.Add(this.uiLabel1);
            this.panel2.Controls.Add(this.btnGetExposureRange);
            this.panel2.Controls.Add(this.nudExposure);
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Controls.Add(this.cbCamera);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(905, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(178, 499);
            this.panel2.TabIndex = 6;
            // 
            // btnReadQR
            // 
            this.btnReadQR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReadQR.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadQR.Location = new System.Drawing.Point(31, 94);
            this.btnReadQR.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnReadQR.Name = "btnReadQR";
            this.btnReadQR.ShowFocusLine = true;
            this.btnReadQR.Size = new System.Drawing.Size(100, 38);
            this.btnReadQR.TabIndex = 13;
            this.btnReadQR.Text = "QR";
            this.btnReadQR.TipsFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnReadQR.Click += new System.EventHandler(this.btnReadQR_Click);
            // 
            // btnLoadRoi
            // 
            this.btnLoadRoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadRoi.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadRoi.Location = new System.Drawing.Point(42, 330);
            this.btnLoadRoi.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLoadRoi.Name = "btnLoadRoi";
            this.btnLoadRoi.Size = new System.Drawing.Size(100, 38);
            this.btnLoadRoi.TabIndex = 12;
            this.btnLoadRoi.Text = "Load";
            this.btnLoadRoi.TipsFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLoadRoi.Click += new System.EventHandler(this.btnLoadRoi_Click);
            // 
            // lbRoi
            // 
            this.lbRoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbRoi.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.lbRoi.ItemSelectForeColor = System.Drawing.Color.White;
            this.lbRoi.Location = new System.Drawing.Point(29, 385);
            this.lbRoi.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lbRoi.MinimumSize = new System.Drawing.Size(1, 1);
            this.lbRoi.Name = "lbRoi";
            this.lbRoi.Padding = new System.Windows.Forms.Padding(2);
            this.lbRoi.ShowText = false;
            this.lbRoi.Size = new System.Drawing.Size(132, 100);
            this.lbRoi.TabIndex = 11;
            this.lbRoi.Text = "uiListBox1";
            this.lbRoi.SelectedIndexChanged += new System.EventHandler(this.lbRoi_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Location = new System.Drawing.Point(42, 286);
            this.btnSave.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 38);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Save";
            this.btnSave.TipsFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDrawRoi
            // 
            this.btnDrawRoi.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDrawRoi.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDrawRoi.Location = new System.Drawing.Point(42, 242);
            this.btnDrawRoi.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnDrawRoi.Name = "btnDrawRoi";
            this.btnDrawRoi.Size = new System.Drawing.Size(100, 38);
            this.btnDrawRoi.TabIndex = 9;
            this.btnDrawRoi.Text = "Draw Roi";
            this.btnDrawRoi.TipsFont = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDrawRoi.Click += new System.EventHandler(this.btnDrawRoi_Click);
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.uiLabel1.Location = new System.Drawing.Point(26, 145);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(123, 25);
            this.uiLabel1.TabIndex = 8;
            this.uiLabel1.Text = "Exposure (µs):\n\n\n\n";
            // 
            // btnGetExposureRange
            // 
            this.btnGetExposureRange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGetExposureRange.Font = new System.Drawing.Font("SimSun", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetExposureRange.Location = new System.Drawing.Point(42, 198);
            this.btnGetExposureRange.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnGetExposureRange.Name = "btnGetExposureRange";
            this.btnGetExposureRange.Size = new System.Drawing.Size(100, 38);
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
            this.nudExposure.Location = new System.Drawing.Point(29, 172);
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
            this.nudExposure.Size = new System.Drawing.Size(120, 20);
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
            this.btnRefresh.Location = new System.Drawing.Point(31, 53);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 35);
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
            this.cbCamera.Location = new System.Drawing.Point(9, 15);
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
            this.cbCamera.SelectedIndexChanged += new System.EventHandler(this.cbCamera_SelectedIndexChanged);
            // 
            // uiContextMenuStrip1
            // 
            this.uiContextMenuStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(249)))), ((int)(((byte)(255)))));
            this.uiContextMenuStrip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.uiContextMenuStrip1.Name = "uiContextMenuStrip1";
            this.uiContextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1083, 499);
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
        private Sunny.UI.UIComboBox cbCamera;
        private Sunny.UI.UIButton btnRefresh;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Sunny.UI.UILabel uiLabel1;
        private Sunny.UI.UIButton btnGetExposureRange;
        private System.Windows.Forms.NumericUpDown nudExposure;
        private Sunny.UI.UIButton btnSave;
        private Sunny.UI.UIButton btnDrawRoi;
        private Sunny.UI.UIContextMenuStrip uiContextMenuStrip1;
        private Sunny.UI.UIListBox lbRoi;
        private Sunny.UI.UIButton btnLoadRoi;
        private Sunny.UI.UIButton btnReadQR;
    }
}

