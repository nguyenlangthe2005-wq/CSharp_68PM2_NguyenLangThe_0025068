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
    public partial class frm_main : Form
    {
        public frm_main()
        {
            InitializeComponent();
        }

        private void pnlLopHoc_Paint(object sender, PaintEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void cboGioiTinh_SelectedIndexChanged(object sender, EventArgs e) { }

        private void quảnLýSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContent.Controls.Clear();
            UCQLSV ucSV = new UCQLSV();
            ucSV.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(ucSV);
        }

        private void quảnLýLớpHọcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlContent.Controls.Clear();
            UCQLLH ucLH = new UCQLLH();
            ucLH.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(ucLH);
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?", "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Restart();
            }
        }
    }
}