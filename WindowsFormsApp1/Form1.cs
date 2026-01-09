using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private HTuple _acqHandle = null;
        private CancellationTokenSource _cts;
        private volatile bool _running = false;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += (s, e) => StopCam();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_running) return;

            try
            {
                // 1) Open camera (tham số giống y hệt open_framegrabber bạn copy)
                HOperatorSet.OpenFramegrabber(
                    "GigEVision2",
                    0, 0, 0, 0, 0, 0,
                    "progressive",   // <--- như HDevelop
                    -1,
                    "default",
                    -1,
                    "false",
                    "default",
                    "Camera1",
                    0,
                    -1,
                    out _acqHandle
                );

                // 2) Start grabbing
                HOperatorSet.GrabImageStart(_acqHandle, -1);

                _cts = new CancellationTokenSource();
                _running = true;
                Task.Run(() => GrabLoop(_cts.Token));
            }
            catch (Exception ex)
            {
                _running = false;
                MessageBox.Show("Start lỗi: " + ex.Message);
            }
        }

        private void GrabLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                HObject img = null;
                try
                {
                    HOperatorSet.GrabImageAsync(out img, _acqHandle, -1);

                    BeginInvoke(new Action(() =>
                    {
                        try
                        {
                            hWindowControl1.HalconWindow.DispObj(img);
                        }
                        finally
                        {
                            img?.Dispose();
                        }
                    }));
                }
                catch
                {
                    img?.Dispose();
                    Thread.Sleep(5);
                }
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCam();
        }

        private void StopCam()
        {
            try
            {
                _cts?.Cancel();
                _cts = null;

                if (_acqHandle != null)
                {
                    try { HOperatorSet.CloseFramegrabber(_acqHandle); } catch { }
                    _acqHandle = null;
                }
            }
            finally
            {
                _running = false;
            }
        }
    }
}
