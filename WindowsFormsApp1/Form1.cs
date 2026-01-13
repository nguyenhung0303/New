using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public class GigECamItem
        {
            public string Display { get; set; }
            public string Device { get; set; }
            public override string ToString() => Display;
        }

        private string _iface = "GigEVision2";
        private HTuple _acqHandle = null;
        private bool _isLiveRunning = false;
        private CancellationTokenSource _liveCts = null;

        private void LoadGigECameras()
        {
            cbCamera.Items.Clear();
            try
            {
                HTuple info, devices;
                HOperatorSet.InfoFramegrabber(_iface, "device", out info, out devices);

                if (devices == null || devices.Length == 0)
                {
                    MessageBox.Show("HALCON không thấy GigE camera.\nHãy kiểm tra: camera online, cùng subnet, tắt firewall/antivirus nếu cần, và camera thấy trong tool hãng.");
                    return;
                }

                var list = new List<GigECamItem>();
                for (int i = 0; i < devices.Length; i++)
                {
                    string dev = devices[i].S;
                    list.Add(new GigECamItem
                    {
                        Device = dev,
                        Display = dev
                    });
                }

                cbCamera.Items.AddRange(list.ToArray());
                if (cbCamera.Items.Count > 0)
                    cbCamera.SelectedIndex = 0;
            }
            catch (HalconException hex)
            {
                MessageBox.Show("Lỗi InfoFramegrabber GigEVision2: " + hex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadGigECameras();
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, -1, -1);
        }

        private void OpenSelectedCamera()
        {
            if (!(cbCamera.SelectedItem is GigECamItem cam)) return;

            StopLive();
            CloseCamera();

            try
            {
                HOperatorSet.OpenFramegrabber(
                    _iface,
                    0, 0, 0, 0, 0, 0,
                    "progressive",
                    -1,
                    "default",
                    -1,
                    "false",
                    "default",
                    cam.Device,
                    0,
                    -1,
                    out _acqHandle
                );

                if (nudExposure.Value > 0)
                {
                    SetExposureTime((double)nudExposure.Value);
                }

                StartLive();
            }
            catch (HalconException hex)
            {
                MessageBox.Show($"Lỗi mở camera: {hex.Message}");
            }
        }

        private void CloseCamera()
        {
            if (_acqHandle != null && _acqHandle.Length > 0)
            {
                try
                {
                    HOperatorSet.CloseFramegrabber(_acqHandle);
                }
                catch { }
                finally
                {
                    _acqHandle = null;
                }
            }
        }

        private void StartLive()
        {
            if (_acqHandle == null || _isLiveRunning) return;

            _isLiveRunning = true;
            _liveCts = new CancellationTokenSource();
            var token = _liveCts.Token;

            Task.Run(() =>
            {
                while (_isLiveRunning && !token.IsCancellationRequested)
                {
                    try
                    {
                        HObject image = null;
                        HOperatorSet.GrabImageAsync(out image, _acqHandle, -1);

                        if (image != null && image.IsInitialized())
                        {
                            if (hWindowControl1.InvokeRequired)
                            {
                                hWindowControl1.Invoke(new Action(() =>
                                {
                                    try
                                    {
                                        HOperatorSet.DispObj(image, hWindowControl1.HalconWindow);
                                    }
                                    catch { }
                                }));
                            }
                            else
                            {
                                HOperatorSet.DispObj(image, hWindowControl1.HalconWindow);
                            }

                            image.Dispose();
                        }
                    }
                    catch (HalconException)
                    {
                        Thread.Sleep(10);
                    }
                    catch
                    {
                        break;
                    }
                }
            }, token);
        }

        private void StopLive()
        {
            _isLiveRunning = false;
            if (_liveCts != null)
            {
                _liveCts.Cancel();
                _liveCts = null;
            }
            Thread.Sleep(100);
        }

        private void SetExposureTime(double exposureTimeUs)
        {
            if (_acqHandle == null || _acqHandle.Length == 0) return;

            try
            {
                HOperatorSet.SetFramegrabberParam(_acqHandle, "ExposureTime", exposureTimeUs);
            }
            catch (HalconException hex)
            {
                try
                {
                    HOperatorSet.SetFramegrabberParam(_acqHandle, "ExposureTimeAbs", exposureTimeUs);
                }
                catch
                {
                    MessageBox.Show($"Lỗi set exposure time: {hex.Message}\n\nThử dùng tên parameter khác hoặc kiểm tra range hợp lệ.");
                }
            }
        }

        private void GetExposureTimeRange()
        {
            if (_acqHandle == null || _acqHandle.Length == 0)
            {
                MessageBox.Show("Vui lòng mở camera trước!");
                return;
            }

            try
            {
                HTuple rangeVal;

                try
                {
                    HOperatorSet.GetFramegrabberParam(_acqHandle, "ExposureTime_range", out rangeVal);
                }
                catch
                {
                    HOperatorSet.GetFramegrabberParam(_acqHandle, "ExposureTimeAbs_range", out rangeVal);
                }

                if (rangeVal != null && rangeVal.Length >= 2)
                {
                    double min = rangeVal[0].D;
                    double max = rangeVal[1].D;

                    nudExposure.Minimum = (decimal)min;
                    nudExposure.Maximum = (decimal)max;

                    MessageBox.Show($"Exposure range: {min} - {max} µs\n\nĐã cập nhật range cho NumericUpDown.", "Thông tin");
                }
                else
                {
                    MessageBox.Show("Không lấy được exposure range từ camera.");
                }
            }
            catch (HalconException hex)
            {
                MessageBox.Show($"Không lấy được exposure range: {hex.Message}\n\nCamera có thể không hỗ trợ query range.");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            StopLive();
            CloseCamera();
            LoadGigECameras();
        }

        private void cbCamera_SelectedIndexChanged(object sender, EventArgs e)
        {
            OpenSelectedCamera();
        }

        private void nudExposure_ValueChanged(object sender, EventArgs e)
        {
            SetExposureTime((double)nudExposure.Value);
        }

        private void btnGetExposureRange_Click(object sender, EventArgs e)
        {
            GetExposureTimeRange();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopLive();
            CloseCamera();
        }
    }
}