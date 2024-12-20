using De01.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De01
{
    public partial class frmSinhVien : Form
    {
        StudentContextDB context = new StudentContextDB();

        public frmSinhVien()
        {
            InitializeComponent();
        }

        public void SetNut(bool e)
        {
            btnThem.Enabled = e;
            btnSua.Enabled = e;
            btnXoa.Enabled = e;
            btnLuu.Enabled = !e;
            btnKLuu.Enabled = !e;
            btnThoat.Enabled = e;
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            try
            {
                List<Sinhvien> listStudent = context.Sinhviens.ToList();
                List<Lop> listClass = context.Lops.ToList();
                FillFalcultyCombobox(listClass);
                BindGrid(listStudent);
                SetNut(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFalcultyCombobox(List<Lop> listClass)
        {
            cboLop.DataSource = listClass;
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
        }

        private void BindGrid(List<Sinhvien> listStudent)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells[0].Value = item.MaSV;
                dgvSinhVien.Rows[index].Cells[1].Value = item.HoTenSV;
                dgvSinhVien.Rows[index].Cells[2].Value = item.NgaySinh.HasValue ? item.NgaySinh.Value.ToString("dd/MM/yyyy") : "";
                dgvSinhVien.Rows[index].Cells[3].Value = item.Lop.TenLop;
            }
        }

        private void ResetInputs()
        {
            txtMaSV.Clear();
            txtHotenSV.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            if (cboLop.Items.Count > 0)
                cboLop.SelectedIndex = 0;
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrEmpty(txtMaSV.Text))
            {
                MessageBox.Show("Mã số sinh viên không được để trống!");
                txtMaSV.Focus();
                return false;
            }

            if (txtMaSV.Text.Length != 6)
            {
                MessageBox.Show("Mã số sinh viên phải có đúng 6 ký tự!");
                txtMaSV.Focus();
                return false;
            }

            if (string.IsNullOrEmpty(txtHotenSV.Text))
            {
                MessageBox.Show("Họ tên sinh viên không được để trống!");
                txtHotenSV.Focus();
                return false;
            }

            if (cboLop.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn lớp!");
                cboLop.Focus();
                return false;
            }

            return true;
        }

        private string currentAction = "";

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;
            currentAction = "Them";
            SetNut(false); 
            ResetInputs();
            txtMaSV.Focus();

        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSV.Text))
            {
                MessageBox.Show("Hãy chọn một sinh viên để xóa!");
                return;
            }
            currentAction = "Xoa";
            SetNut(false);
   
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSV.Text))
            {
                MessageBox.Show("Hãy chọn một sinh viên để sửa!");
                return;
            }
            currentAction = "Sua";
            SetNut(false); 
            txtHotenSV.Focus();
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                txtMaSV.Text = row.Cells[0].Value?.ToString();
                txtHotenSV.Text = row.Cells[1].Value?.ToString();
                dtpNgaySinh.Value = DateTime.Parse(row.Cells[2].Value.ToString());
                cboLop.Text = row.Cells[3].Value?.ToString();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                switch (currentAction)
                {
                    case "Them":
                        Sinhvien newStudent = new Sinhvien
                        {
                            MaSV = txtMaSV.Text,
                            HoTenSV = txtHotenSV.Text,
                            NgaySinh = dtpNgaySinh.Value,
                            MaLop = cboLop.SelectedValue.ToString()
                        };
                        context.Sinhviens.Add(newStudent);
                        context.SaveChanges();
                        MessageBox.Show("Thêm mới sinh viên thành công!");
                        break;

                    case "Sua":
                        var studentToEdit = context.Sinhviens.FirstOrDefault(s => s.MaSV == txtMaSV.Text);
                        if (studentToEdit == null)
                        {
                            MessageBox.Show("Không tìm thấy sinh viên cần sửa!");
                            return;
                        }
                        studentToEdit.HoTenSV = txtHotenSV.Text;
                        studentToEdit.NgaySinh = dtpNgaySinh.Value;
                        studentToEdit.MaLop = cboLop.SelectedValue.ToString();
                        context.SaveChanges();
                        MessageBox.Show("Sửa thông tin sinh viên thành công!");
                        break;

                    case "Xoa":
                        var studentToDelete = context.Sinhviens.FirstOrDefault(s => s.MaSV == txtMaSV.Text);
                        if (studentToDelete == null)
                        {
                            MessageBox.Show("Không tìm thấy sinh viên cần xóa!");
                            return;
                        }
                        if (MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            context.Sinhviens.Remove(studentToDelete);
                            context.SaveChanges();
                            MessageBox.Show("Xóa sinh viên thành công!");
                        }
                        break;

                    default:
                        MessageBox.Show("Không có hành động nào được chọn!");
                        return;
                }

                BindGrid(context.Sinhviens.ToList());

                ResetInputs();
                SetNut(true);
                currentAction = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}");
            }
        }

        private void btnKLuu_Click(object sender, EventArgs e)
        {
            ResetInputs();
            BindGrid(context.Sinhviens.ToList());
            SetNut(true);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Close();
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();

                var result = context.Sinhviens
                    .Where(s => s.MaSV.ToLower().Contains(keyword) ||
                                s.HoTenSV.ToLower().Contains(keyword) ||
                                s.Lop.TenLop.ToLower().Contains(keyword))
                    .ToList();

                if (result.Count > 0)
                {
                    BindGrid(result);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên nào khớp với từ khóa tìm kiếm!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    BindGrid(context.Sinhviens.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}