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

        // lock để tránh GrabLoop và btnReadQR grab cùng lúc
        private readonly object _camLock = new object();

        // QR model handle (tạo 1 lần)
        private HTuple _qrHandle = null;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += (s, e) => StopCam();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // tạo model QR Code
                HOperatorSet.CreateDataCode2dModel("QR Code", new HTuple(), new HTuple(), out _qrHandle);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không tạo được QR model (có thể thiếu license DataCode): " + ex.Message);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_running) return;

            try
            {
                lock (_camLock)
                {
                    // 1) Open camera (đúng tham số bạn copy từ HDevelop)
                    HOperatorSet.OpenFramegrabber(
                        "GigEVision2",
                        0, 0, 0, 0, 0, 0,
                        "progressive",
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
                    
                    TrySetFgParam("ExposureTime", 20000);
                    // 2) Start grabbing
                    HOperatorSet.GrabImageStart(_acqHandle, -1);
                }

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
                    lock (_camLock)
                    {
                        if (_acqHandle == null) continue;
                        HOperatorSet.GrabImageAsync(out img, _acqHandle, -1);
                    }

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
        private void TrySetFgParam(string name, object value)
        {
            try
            {
                if (_acqHandle == null) return;

                HTuple hvName = new HTuple(name);
                HTuple hvValue;

                if (value is string s) hvValue = new HTuple(s);
                else if (value is int i) hvValue = new HTuple(i);
                else if (value is double d) hvValue = new HTuple(d);
                else hvValue = new HTuple(value.ToString());

                HOperatorSet.SetFramegrabberParam(_acqHandle, hvName, hvValue);
            }
            catch
            {
                // ignore nếu camera/interface không có param đó
            }
        }
        // ======= NÚT ĐỌC QR TRỰC TIẾP TỪ CAMERA =======
        private void btnReadQR_Click(object sender, EventArgs e)
        {
            if (_qrHandle == null)
            {
                MessageBox.Show("QR model chưa sẵn sàng (có thể thiếu license DataCode).");
                return;
            }

            if (_acqHandle == null)
            {
                MessageBox.Show("Bạn chưa Start camera.");
                return;
            }

            HObject img = null;
            HObject symbolXLDs = null;

            try
            {
                // 1) Grab 1 frame ngay lúc bấm nút (có lock để không đụng GrabLoop)
                lock (_camLock)
                {
                    if (_acqHandle == null) return;
                    HOperatorSet.GrabImageAsync(out img, _acqHandle, -1);
                }

                // 2) Đọc QR
                HTuple resultHandles, decodedStrings;
                HOperatorSet.FindDataCode2d(
                    img,
                    out symbolXLDs,
                    _qrHandle,
                    new HTuple(), new HTuple(),
                    out resultHandles,
                    out decodedStrings
                );

                // 3) Hiển thị ảnh + vùng QR + text
                hWindowControl1.HalconWindow.DispObj(img);

                if (decodedStrings != null && decodedStrings.Length > 0)
                {
                    string qr = decodedStrings[0].S;

                    hWindowControl1.HalconWindow.SetColor("yellow");
                    hWindowControl1.HalconWindow.DispObj(symbolXLDs);

                    HOperatorSet.DispText(
                        hWindowControl1.HalconWindow,
                        "QR: " + qr,
                        "window",
                        20, 20,
                        "green",
                        "box",
                        "true"
                    );

                    MessageBox.Show("Đọc QR: " + qr);
                }
                else
                {
                    HOperatorSet.DispText(
                        hWindowControl1.HalconWindow,
                        "Không tìm thấy QR",
                        "window",
                        20, 20,
                        "red",
                        "box",
                        "true"
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ReadQR lỗi: " + ex.Message);
            }
            finally
            {
                symbolXLDs?.Dispose();
                img?.Dispose();
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

                lock (_camLock)
                {
                    if (_acqHandle != null)
                    {
                        try { HOperatorSet.CloseFramegrabber(_acqHandle); } catch { }
                        _acqHandle = null;
                    }
                }

                if (_qrHandle != null)
                {
                    try { HOperatorSet.ClearDataCode2dModel(_qrHandle); } catch { }
                    _qrHandle = null;
                }
            }
            finally
            {
                _running = false;
            }
        }
    }
}
