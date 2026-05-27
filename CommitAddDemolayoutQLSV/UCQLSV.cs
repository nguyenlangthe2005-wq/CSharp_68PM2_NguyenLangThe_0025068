using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommitAddDemolayoutQLSV
{
    public partial class UCQLSV : UserControl
    {
        databaseDataContext db = new databaseDataContext();
        public UCQLSV()
        {
            InitializeComponent();
        }

        private void pnlSinhVien_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblPhanTrang_Click(object sender, EventArgs e)
        {

        }

        private void btnTrangCuoi_Click(object sender, EventArgs e)
        {

        }

        private void btnTrangSau_Click(object sender, EventArgs e)
        {

        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {

        }

        private void btnTrangDau_Click(object sender, EventArgs e)
        {

        }

        private void txtHoTen_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTim_Click(object sender, EventArgs e)
        {

        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {

        }

        private void btnSua_Click(object sender, EventArgs e)
        {

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            tbl_sinhvien sinhvien = new tbl_sinhvien();
            sinhvien.id = txtMSSV.Text;
            sinhvien.hoten = txtHoTen.Text;
            sinhvien.gioitinh = cboGioiTinh.Text;
            sinhvien.ngaysinh = DateTime.Parse(dtpNgaySinh.Text);

            try
            {
                db.tbl_sinhviens.InsertOnSubmit(sinhvien);
                db.SubmitChanges();
                MessageBox.Show("Thêm sinh viên thành công!");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //db.tbl_sinhviens.InsertOnSubmit(sinhvien);
            //db.SubmitChanges();
            //LoadData();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cboLop_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboGioiTinh_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtpNgaySinh_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtMaSV_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void UCQLSV_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadDSLH4CBX();
        }
        public void LoadData()
        {
            List<tbl_sinhvien> dSSV = db.tbl_sinhviens.ToList();
            dgv_DSSV.DataSource = dSSV;
        }

        public void LoadDSLH4CBX() // Load dữ liệu cho comboBox
        {
            List<tbl_lophoc> dSLH = db.tbl_lophocs.ToList();
            //cboMaLop.Text = "68PM12";
            cboMaLop.DataSource = dSLH;
            cboMaLop.DisplayMember = "tenLop"; 
            cboMaLop.ValueMember = "malop"; 
        }
    }
}
