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

        public class CameraItem
        {
            public string Display { get; set; }
            public string Device { get; set; }
            public string Interface { get; set; }
            public CameraType Type { get; set; }
            public override string ToString() => Display;
        }

        public enum CameraType
        {
            GigE,
            USB
        }

        private HTuple _acqHandle = null;
        private bool _isLiveRunning = false;
        private CancellationTokenSource _liveCts = null;
        private CameraType _currentCameraType;

        private void LoadAllCameras()
        {
            cbCamera.Items.Clear();
            var allCameras = new List<CameraItem>();

            // Thêm GigE cameras
            try
            {
                HTuple info, devices;
                HOperatorSet.InfoFramegrabber("GigEVision2", "device", out info, out devices);

                if (devices != null && devices.Length > 0)
                {
                    for (int i = 0; i < devices.Length; i++)
                    {
                        string dev = devices[i].S;
                        allCameras.Add(new CameraItem
                        {
                            Device = dev,
                            Display = $"[GigE] {dev}",
                            Interface = "GigEVision2",
                            Type = CameraType.GigE
                        });
                    }
                }
            }
            catch (HalconException hex)
            {
                // GigE không khả dụng
                Console.WriteLine("GigE không khả dụng: " + hex.Message);
            }

            // Thêm USB cameras (DirectShow)
            try
            {
                HTuple info, devices;
                HOperatorSet.InfoFramegrabber("DirectShow", "device", out info, out devices);

                if (devices != null && devices.Length > 0)
                {
                    for (int i = 0; i < devices.Length; i++)
                    {
                        string dev = devices[i].S;
                        allCameras.Add(new CameraItem
                        {
                            Device = dev,
                            Display = $"[USB] {dev}",
                            Interface = "DirectShow",
                            Type = CameraType.USB
                        });
                    }
                }
            }
            catch (HalconException hex)
            {
                Console.WriteLine("USB DirectShow không khả dụng: " + hex.Message);
            }

            // Thêm vào ComboBox
            if (allCameras.Count > 0)
            {
                cbCamera.Items.AddRange(allCameras.ToArray());
                cbCamera.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Không tìm thấy camera nào (GigE hoặc USB).\n\n" +
                    "Kiểm tra:\n" +
                    "- GigE: camera online, cùng subnet, tắt firewall\n" +
                    "- USB: camera đã cắm và driver đã cài",
                    "Không tìm thấy camera");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadAllCameras();
            HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, -1, -1);

            // Đăng ký event để tự động fit khi resize
            hWindowControl1.SizeChanged += HWindowControl1_SizeChanged;
        }

        private void HWindowControl1_SizeChanged(object sender, EventArgs e)
        {
            // Reset part để fit lại khi resize control
            try
            {
                HOperatorSet.SetPart(hWindowControl1.HalconWindow, 0, 0, -1, -1);
            }
            catch { }
        }

        private void OpenSelectedCamera()
        {
            if (!(cbCamera.SelectedItem is CameraItem cam)) return;

            StopLive();
            CloseCamera();

            try
            {
                _currentCameraType = cam.Type;

                if (cam.Type == CameraType.GigE)
                {
                    // Mở GigE camera
                    HOperatorSet.OpenFramegrabber(
                        cam.Interface,
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
                }
                else // USB camera
                {
                    // Mở USB camera với DirectShow
                    HOperatorSet.OpenFramegrabber(
                        cam.Interface,
                        1, 1, 0, 0, 0, 0,
                        "default",
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
                }

                if (nudExposure.Value > 0)
                {
                    SetExposureTime((double)nudExposure.Value);
                }

                StartLive();
            }
            catch (HalconException hex)
            {
                MessageBox.Show($"Lỗi mở camera: {hex.Message}\n\n" +
                    $"Camera type: {cam.Type}\n" +
                    $"Interface: {cam.Interface}",
                    "Lỗi");
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
                                        DisplayImageFit(image);
                                    }
                                    catch { }
                                }));
                            }
                            else
                            {
                                DisplayImageFit(image);
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

        private void DisplayImageFit(HObject image)
        {
            try
            {
                HTuple imgWidth, imgHeight;
                HOperatorSet.GetImageSize(image, out imgWidth, out imgHeight);

                int winWidth = hWindowControl1.Width;
                int winHeight = hWindowControl1.Height;

                // Tính toán tỷ lệ để fit ảnh vào control mà không bị méo
                double scaleWidth = (double)winWidth / imgWidth.I;
                double scaleHeight = (double)winHeight / imgHeight.I;

                // Chọn tỷ lệ nhỏ hơn để đảm bảo ảnh vừa khít mà không bị cắt
                double scale = Math.Min(scaleWidth, scaleHeight);

                // Tính toán kích thước hiển thị thực tế
                int displayWidth = (int)(imgWidth.I * scale);
                int displayHeight = (int)(imgHeight.I * scale);

                // Tính toán offset để căn giữa ảnh
                int offsetX = (winWidth - displayWidth) / 2;
                int offsetY = (winHeight - displayHeight) / 2;

                // Clear window và set màu nền
                HOperatorSet.ClearWindow(hWindowControl1.HalconWindow);

                // Set part để hiển thị toàn bộ ảnh (từ 0,0 đến kích thước ảnh - 1)
                HOperatorSet.SetPart(hWindowControl1.HalconWindow,
                    -offsetY / scale,
                    -offsetX / scale,
                    (winHeight - offsetY) / scale - 1,
                    (winWidth - offsetX) / scale - 1);

                // Hiển thị ảnh
                HOperatorSet.DispObj(image, hWindowControl1.HalconWindow);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error displaying image: " + ex.Message);
            }
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
                if (_currentCameraType == CameraType.GigE)
                {
                    // GigE camera - thử các tên parameter phổ biến
                    try
                    {
                        HOperatorSet.SetFramegrabberParam(_acqHandle, "ExposureTime", exposureTimeUs);
                    }
                    catch
                    {
                        HOperatorSet.SetFramegrabberParam(_acqHandle, "ExposureTimeAbs", exposureTimeUs);
                    }
                }
                else // USB camera
                {
                    // USB camera thường không hỗ trợ exposure time chính xác như GigE
                    // Có thể thử các parameter khác như "brightness", "exposure" tùy camera
                    MessageBox.Show("Camera USB thường không hỗ trợ điều chỉnh exposure time như GigE camera.\n\n" +
                        "Bạn có thể điều chỉnh trong phần mềm của camera hoặc qua properties.",
                        "Thông tin");
                }
            }
            catch (HalconException hex)
            {
                MessageBox.Show($"Lỗi set exposure time: {hex.Message}\n\n" +
                    $"Camera type: {_currentCameraType}",
                    "Lỗi");
            }
        }

        private void GetExposureTimeRange()
        {
            if (_acqHandle == null || _acqHandle.Length == 0)
            {
                MessageBox.Show("Vui lòng mở camera trước!");
                return;
            }

            if (_currentCameraType == CameraType.USB)
            {
                MessageBox.Show("Camera USB thường không hỗ trợ query exposure range qua HALCON.\n\n" +
                    "Sử dụng phần mềm của camera để điều chỉnh.",
                    "Thông tin");
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

                    MessageBox.Show($"Exposure range: {min} - {max} µs\n\nĐã cập nhật range cho NumericUpDown.",
                        "Thông tin");
                }
                else
                {
                    MessageBox.Show("Không lấy được exposure range từ camera.");
                }
            }
            catch (HalconException hex)
            {
                MessageBox.Show($"Không lấy được exposure range: {hex.Message}\n\n" +
                    "Camera có thể không hỗ trợ query range.",
                    "Lỗi");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            StopLive();
            CloseCamera();
            LoadAllCameras();
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