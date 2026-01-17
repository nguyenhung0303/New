using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
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

        // ROI Model class
        public class RoiModel
        {
            public string Name { get; set; }
            public double Row1 { get; set; }
            public double Col1 { get; set; }
            public double Row2 { get; set; }
            public double Col2 { get; set; }
            public string Color { get; set; } = "green";

            public override string ToString() => Name;
        }

        private HTuple _acqHandle = null;
        private bool _isLiveRunning = false;
        private CancellationTokenSource _liveCts = null;
        private CameraType _currentCameraType;
        private List<RoiModel> _roiList = new List<RoiModel>();
        private const string ROI_FILE = "rois.txt";
        private HTuple _dataCodeHandle = null; // Handle cho Data Code reader

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

            // Load ROI nếu có file
            LoadRoisFromFile();

            // Khởi tạo Data Code Reader cho QR
            InitializeDataCodeReader();
        }

        private void HWindowControl1_SizeChanged(object sender, EventArgs e)
        {
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
                else
                {
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
                                        DisplayAllRois();
                                    }
                                    catch { }
                                }));
                            }
                            else
                            {
                                DisplayImageFit(image);
                                DisplayAllRois();
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

                double scaleWidth = (double)winWidth / imgWidth.I;
                double scaleHeight = (double)winHeight / imgHeight.I;
                double scale = Math.Min(scaleWidth, scaleHeight);

                int displayWidth = (int)(imgWidth.I * scale);
                int displayHeight = (int)(imgHeight.I * scale);

                int offsetX = (winWidth - displayWidth) / 2;
                int offsetY = (winHeight - displayHeight) / 2;

                HOperatorSet.ClearWindow(hWindowControl1.HalconWindow);

                HOperatorSet.SetPart(hWindowControl1.HalconWindow,
                    -offsetY / scale,
                    -offsetX / scale,
                    (winHeight - offsetY) / scale - 1,
                    (winWidth - offsetX) / scale - 1);

                HOperatorSet.DispObj(image, hWindowControl1.HalconWindow);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error displaying image: " + ex.Message);
            }
        }

        private void DisplayAllRois()
        {
            try
            {
                foreach (var roi in _roiList)
                {
                    HOperatorSet.SetColor(hWindowControl1.HalconWindow, roi.Color);
                    HOperatorSet.SetDraw(hWindowControl1.HalconWindow, "margin");
                    HOperatorSet.SetLineWidth(hWindowControl1.HalconWindow, 2);
                    HOperatorSet.DispRectangle1(hWindowControl1.HalconWindow,
                        roi.Row1, roi.Col1, roi.Row2, roi.Col2);

                    // Hiển thị tên ROI
                    HOperatorSet.SetTposition(hWindowControl1.HalconWindow, roi.Row1 - 20, roi.Col1);
                    HOperatorSet.WriteString(hWindowControl1.HalconWindow, roi.Name);
                }
            }
            catch { }
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
                    try
                    {
                        HOperatorSet.SetFramegrabberParam(_acqHandle, "ExposureTime", exposureTimeUs);
                    }
                    catch
                    {
                        HOperatorSet.SetFramegrabberParam(_acqHandle, "ExposureTimeAbs", exposureTimeUs);
                    }
                }
                else
                {
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

        // ========== ROI FUNCTIONS ==========

        private void btnDrawRoi_Click(object sender, EventArgs e)
        {
            try
            {
                // Tạm dừng live để vẽ ROI
                bool wasLive = _isLiveRunning;
                if (wasLive)
                    StopLive();

                // Vẽ ROI bằng HALCON interactive drawing
                HTuple row1, col1, row2, col2;

                // Hiển thị message trên window
                HOperatorSet.SetColor(hWindowControl1.HalconWindow, "yellow");
                HOperatorSet.SetTposition(hWindowControl1.HalconWindow, 20, 20);
                HOperatorSet.WriteString(hWindowControl1.HalconWindow, "Vẽ hình chữ nhật ROI...");

                // Vẽ ROI - người dùng click và kéo để tạo hình chữ nhật
                HOperatorSet.DrawRectangle1(hWindowControl1.HalconWindow,
                    out row1, out col1, out row2, out col2);

                // Tự động đặt tên ROI hoặc dùng dialog đơn giản
                string roiName = GetRoiName();

                if (string.IsNullOrWhiteSpace(roiName))
                {
                    if (wasLive) StartLive();
                    return;
                }

                // Lưu ROI
                var roi = new RoiModel
                {
                    Name = roiName,
                    Row1 = row1.D,
                    Col1 = col1.D,
                    Row2 = row2.D,
                    Col2 = col2.D,
                    Color = "green"
                };

                _roiList.Add(roi);
                UpdateRoiListBox();

                MessageBox.Show($"Đã thêm ROI: {roiName}\n" +
                    $"Tọa độ: ({row1.D:F1}, {col1.D:F1}) -> ({row2.D:F1}, {col2.D:F1})",
                    "Thành công");

                // Tiếp tục live nếu đang chạy
                if (wasLive)
                    StartLive();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi vẽ ROI: {ex.Message}", "Lỗi");
            }
        }

        private string GetRoiName()
        {
            // Dialog đơn giản để nhập tên ROI
            using (Form prompt = new Form())
            {
                prompt.Width = 400;
                prompt.Height = 150;
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.Text = "Nhập tên ROI";
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;

                Label textLabel = new Label() { Left = 20, Top = 20, Text = "Tên ROI:", Width = 100 };
                TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 340, Text = $"ROI_{_roiList.Count + 1}" };
                Button confirmation = new Button() { Text = "OK", Left = 200, Width = 80, Top = 80, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Hủy", Left = 290, Width = 70, Top = 80, DialogResult = DialogResult.Cancel };

                confirmation.Click += (sender, e) => { prompt.Close(); };
                cancel.Click += (sender, e) => { prompt.Close(); };

                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.AcceptButton = confirmation;
                prompt.CancelButton = cancel;

                textBox.SelectAll();
                textBox.Focus();

                return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(ROI_FILE))
                {
                    foreach (var roi in _roiList)
                    {
                        sw.WriteLine($"{roi.Name}|{roi.Row1}|{roi.Col1}|{roi.Row2}|{roi.Col2}|{roi.Color}");
                    }
                }

                MessageBox.Show($"Đã lưu {_roiList.Count} ROI vào file {ROI_FILE}", "Thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lưu file: {ex.Message}", "Lỗi");
            }
        }

        private void btnLoadRoi_Click(object sender, EventArgs e)
        {
            LoadRoisFromFile();
        }

        private void LoadRoisFromFile()
        {
            if (!File.Exists(ROI_FILE))
            {
                return;
            }

            try
            {
                _roiList.Clear();

                string[] lines = File.ReadAllLines(ROI_FILE);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = line.Split('|');
                    if (parts.Length >= 6)
                    {
                        var roi = new RoiModel
                        {
                            Name = parts[0],
                            Row1 = double.Parse(parts[1]),
                            Col1 = double.Parse(parts[2]),
                            Row2 = double.Parse(parts[3]),
                            Col2 = double.Parse(parts[4]),
                            Color = parts[5]
                        };
                        _roiList.Add(roi);
                    }
                }

                UpdateRoiListBox();

                if (_roiList.Count > 0)
                {
                    MessageBox.Show($"Đã load {_roiList.Count} ROI từ file {ROI_FILE}", "Thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi load file: {ex.Message}", "Lỗi");
            }
        }

        private void UpdateRoiListBox()
        {
            lbRoi.Items.Clear();
            foreach (var roi in _roiList)
            {
                lbRoi.Items.Add(roi);
            }
        }

        private void lbRoi_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Có thể thêm chức năng highlight ROI được chọn
            if (lbRoi.SelectedItem is RoiModel selectedRoi)
            {
                // Hiển thị thông tin ROI
                Console.WriteLine($"Selected ROI: {selectedRoi.Name}");
            }
        }

        // Optional: Xóa ROI
        private void lbRoi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && lbRoi.SelectedIndex >= 0)
            {
                if (MessageBox.Show("Xóa ROI này?", "Xác nhận",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    _roiList.RemoveAt(lbRoi.SelectedIndex);
                    UpdateRoiListBox();
                }
            }
        }

        // ========== EXISTING FUNCTIONS ==========

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

            // Giải phóng Data Code Handle
            if (_dataCodeHandle != null && _dataCodeHandle.Length > 0)
            {
                try
                {
                    HOperatorSet.ClearDataCode2dModel(_dataCodeHandle);
                }
                catch { }
            }
        }

        // ========== QR CODE FUNCTIONS ==========

        private void InitializeDataCodeReader()
        {
            try
            {
                // Tạo model cho QR Code
                HOperatorSet.CreateDataCode2dModel("QR Code", new HTuple(), new HTuple(), out _dataCodeHandle);

                // Thiết lập các tham số (tùy chỉnh theo nhu cầu)
                HOperatorSet.SetDataCode2dParam(_dataCodeHandle, "default_parameters", "enhanced_recognition");
                HOperatorSet.SetDataCode2dParam(_dataCodeHandle, "polarity", "auto");
            }
            catch (HalconException hex)
            {
                MessageBox.Show($"Lỗi khởi tạo QR reader: {hex.Message}", "Lỗi");
            }
        }

        private void btnReadQR_Click(object sender, EventArgs e)
        {
            if (_acqHandle == null)
            {
                MessageBox.Show("Vui lòng mở camera trước!", "Thông báo");
                return;
            }

            if (_roiList.Count == 0)
            {
                MessageBox.Show("Chưa có ROI nào! Vui lòng vẽ ROI trước.", "Thông báo");
                return;
            }

            try
            {
                // Tạm dừng live
                bool wasLive = _isLiveRunning;
                if (wasLive)
                    StopLive();

                // Chụp 1 frame
                HObject image;
                HOperatorSet.GrabImage(out image, _acqHandle);

                // Hiển thị ảnh gốc
                DisplayImageFit(image);

                // Lưu tất cả kết quả
                List<string> allResults = new List<string>();

                // Đọc QR trong TỪNG ROI
                foreach (var roi in _roiList)
                {
                    try
                    {
                        // Cắt ảnh theo ROI
                        HObject imageROI;
                        HOperatorSet.CropRectangle1(image, out imageROI,
                            roi.Row1, roi.Col1, roi.Row2, roi.Col2);

                        // Đọc QR code trong vùng ROI
                        HObject symbolXLDs;
                        HTuple resultHandles, decodedDataStrings;

                        HOperatorSet.FindDataCode2d(imageROI, out symbolXLDs, _dataCodeHandle,
                            new HTuple(), new HTuple(), out resultHandles, out decodedDataStrings);

                        if (decodedDataStrings.Length > 0)
                        {
                            for (int i = 0; i < decodedDataStrings.Length; i++)
                            {
                                allResults.Add($"[{roi.Name}] QR {i + 1}: {decodedDataStrings[i].S}");
                            }

                            // Vẽ contour QR lên ảnh (chuyển tọa độ về ảnh gốc)
                            HObject symbolXLDsMoved;
                            HOperatorSet.MoveRegion(symbolXLDs, out symbolXLDsMoved, roi.Row1, roi.Col1);

                            HOperatorSet.SetColor(hWindowControl1.HalconWindow, "green");
                            HOperatorSet.SetLineWidth(hWindowControl1.HalconWindow, 3);
                            HOperatorSet.DispObj(symbolXLDsMoved, hWindowControl1.HalconWindow);

                            symbolXLDsMoved.Dispose();
                        }

                        symbolXLDs.Dispose();
                        imageROI.Dispose();
                    }
                    catch (HalconException hex)
                    {
                        allResults.Add($"[{roi.Name}] Lỗi: {hex.Message}");
                    }
                }

                // Vẽ lại tất cả ROI
                DisplayAllRois();

                // Hiển thị kết quả
                if (allResults.Count > 0)
                {
                    string result = string.Join("\n", allResults);
                    MessageBox.Show(result, $"Kết quả đọc QR ({allResults.Count} kết quả)");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy QR code nào trong các ROI!", "Thông báo");
                }

                image.Dispose();

                // Tiếp tục live
                if (wasLive)
                    StartLive();
            }
            catch (HalconException hex)
            {
                MessageBox.Show($"Lỗi đọc QR: {hex.Message}", "Lỗi");
            }
        }

        // Đọc QR trong ROI cụ thể

    }
}