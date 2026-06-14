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
        string targetSearch = "";
        int pageNumber = 1;
        int pageSize = 2;
        int totalPages = 1;

        public UCQLSV()
        {
            InitializeComponent();
        }

        private void pnlSinhVien_Paint(object sender, PaintEventArgs e) { }
        private void lblPhanTrang_Click(object sender, EventArgs e) { }

        private void btnTrangCuoi_Click(object sender, EventArgs e)
        {
            pageNumber = totalPages;
            LoadData();
        }

        private void btnTrangSau_Click(object sender, EventArgs e)
        {
            if (pageNumber < totalPages)
            {
                pageNumber++;
                LoadData();
            }
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            if (pageNumber > 1)
            {
                pageNumber--;
                LoadData();
            }
        }

        private void btnTrangDau_Click(object sender, EventArgs e)
        {
            pageNumber = 1;
            LoadData();
        }

        private void txtHoTen_TextChanged(object sender, EventArgs e) { }

        private void btnTim_Click(object sender, EventArgs e)
        {
            targetSearch = txtTimKiem.Text.Trim();
            pageNumber = 1;
            LoadData();
        }

        private void txtTimKiem_TextChanged(object sender, EventArgs e) { }
        private void label6_Click(object sender, EventArgs e) { }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMSSV.Text = "";
            txtHoTen.Text = "";
            cboGioiTinh.SelectedIndex = 0;
            dtpNgaySinh.Value = DateTime.Now;
            if (cboMaLop.Items.Count > 0) cboMaLop.SelectedIndex = 0;
            txtMSSV.Enabled = true;
            targetSearch = "";
            txtTimKiem.Text = "";
            pageNumber = 1;
            LoadData();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string maSV = txtMSSV.Text.Trim();

            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Vui lòng chọn một sinh viên từ danh sách để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dialogResult = MessageBox.Show($"Bạn có chắc chắn muốn xóa sinh viên có mã {maSV} không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (dialogResult == DialogResult.Yes)
            {
                tbl_sinhvien sinhvien = db.tbl_sinhviens.SingleOrDefault(sv => sv.id == maSV);

                if (sinhvien != null)
                {
                    try
                    {
                        db.tbl_sinhviens.DeleteOnSubmit(sinhvien);
                        db.SubmitChanges();
                        MessageBox.Show("Xóa sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnLamMoi_Click(sender, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra khi xóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên cần xóa trong hệ thống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            string maSV = txtMSSV.Text.Trim();

            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Vui lòng chọn một sinh viên để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                tbl_sinhvien sinhvien = db.tbl_sinhviens.SingleOrDefault(sv => sv.id == maSV);

                if (sinhvien != null)
                {
                    sinhvien.hoten = txtHoTen.Text.Trim();
                    sinhvien.gioitinh = cboGioiTinh.Text;
                    sinhvien.ngaysinh = dtpNgaySinh.Value.Date;
                    sinhvien.malop = cboMaLop.SelectedValue?.ToString();

                    db.SubmitChanges();
                    MessageBox.Show("Cập nhật thông tin sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên để sửa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMSSV.Text) || string.IsNullOrWhiteSpace(txtHoTen.Text))
            {
                MessageBox.Show("Mã sinh viên và Họ tên không được để trống!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; 
            }

            var checkTonTai = db.tbl_sinhviens.SingleOrDefault(sv => sv.id == txtMSSV.Text.Trim());
            if (checkTonTai != null)
            {
                MessageBox.Show("Mã sinh viên này đã tồn tại! Vui lòng nhập mã khác.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tbl_sinhvien sinhvien = new tbl_sinhvien();
            sinhvien.id = txtMSSV.Text.Trim();
            sinhvien.hoten = txtHoTen.Text.Trim();
            sinhvien.gioitinh = cboGioiTinh.Text;
            sinhvien.ngaysinh = dtpNgaySinh.Value.Date;

            if (cboMaLop.SelectedValue != null)
            {
                sinhvien.malop = cboMaLop.SelectedValue.ToString();
            }

            try
            {
                db.tbl_sinhviens.InsertOnSubmit(sinhvien);
                db.SubmitChanges();
                MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void cboLop_SelectedIndexChanged(object sender, EventArgs e) { }
        private void cboGioiTinh_SelectedIndexChanged(object sender, EventArgs e) { }
        private void dtpNgaySinh_ValueChanged(object sender, EventArgs e) { }
        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void txtMaSV_TextChanged(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }

        private void UCQLSV_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadDSLH4CBX();
        }

        public void LoadData()
        {
            var query = db.tbl_sinhviens.AsQueryable();

            if (!string.IsNullOrEmpty(targetSearch))
            {
                query = query.Where(sv => sv.hoten.Contains(targetSearch) || sv.id.Contains(targetSearch) || sv.malop.Contains(targetSearch));
            }

            int totalRecords = query.Count();
            totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            if (totalPages <= 0) totalPages = 1;

            if (pageNumber > totalPages) pageNumber = totalPages;

            lblPhanTrang.Text = $"Trang {pageNumber}/{totalPages} | {totalRecords} bản ghi";

            List<tbl_sinhvien> dSSV = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            dgv_DSSV.DataSource = dSSV;
        }

        public void LoadDSLH4CBX()
        {
            List<tbl_lophoc> dSLH = db.tbl_lophocs.ToList();
            cboMaLop.DataSource = dSLH;
            cboMaLop.DisplayMember = "tenLop";
            cboMaLop.ValueMember = "malop";
        }

        private void dgv_DSSV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_DSSV.Rows[e.RowIndex];

                txtMSSV.Text = row.Cells["id"].Value?.ToString();
                txtHoTen.Text = row.Cells["hoten"].Value?.ToString();
                cboGioiTinh.Text = row.Cells["gioitinh"].Value?.ToString();

                if (row.Cells["ngaysinh"].Value != null)
                {
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["ngaysinh"].Value);
                }

                if (row.Cells["malop"].Value != null)
                {
                    cboMaLop.SelectedValue = row.Cells["malop"].Value.ToString();
                }

                txtMSSV.Enabled = false;
            }
        }

        private void dgv_DSSV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}