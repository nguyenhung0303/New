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
            public string Display { get; set; }   // chuỗi hiển thị
            public string Device { get; set; }    // chuỗi Device dùng để OpenFramegrabber
            public override string ToString() => Display;

        }
        private string _iface = "GigEVision2";   // GigE dùng GigEVision2 :contentReference[oaicite:2]{index=2}

        private void LoadGigECameras()
        {
            cbCamera.Items.Clear();

            try
            {
                // "device" => list các giá trị hợp lệ cho tham số Device của OpenFramegrabber :contentReference[oaicite:3]{index=3}
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

                    // Hiển thị cơ bản: bạn có thể đổi format sau (model | SN | IP… nếu query thêm)
                    list.Add(new GigECamItem
                    {
                        Device = dev,
                        Display = dev
                    });
                }

                cbCamera.Items.AddRange(list.ToArray());
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
        }

        private HTuple _acqHandle = null;

        //private void OpenSelectedCamera()
        //{
        //    if (cbCamera.SelectedItem is not GigECamItem cam) return;

        //    // đóng handle cũ
        //    if (_acqHandle != null && _acqHandle.Length > 0)
        //    {
        //        try { HOperatorSet.CloseFramegrabber(_acqHandle); } catch { }
        //        _acqHandle = null;
        //    }

        //    // Device truyền đúng chuỗi lấy từ InfoFramegrabber("device") :contentReference[oaicite:4]{index=4}
        //    HOperatorSet.OpenFramegrabber(
        //        _iface,
        //        0, 0, 0, 0, 0, 0,
        //        "progressive",
        //        -1,
        //        "default",
        //        -1,
        //        "false",
        //        "default",
        //        cam.Device,   // <-- quan trọng
        //        0,
        //        -1,
        //        out _acqHandle
        //    ); // :contentReference[oaicite:5]{index=5}
        //}

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadGigECameras();
        }
    }
}
