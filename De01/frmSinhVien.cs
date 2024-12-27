using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using De01.DAL.Entities;
using De01_BLL;

namespace De01
{
    public partial class frmSinhVien : Form
    {
        private readonly StudentService studentService;
        private string currentAction = "";

        public frmSinhVien()
        {
            InitializeComponent();
            studentService = new StudentService();
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            var students = studentService.GetAllStudents();
            var classes = studentService.GetAllClasses();
            FillFacultyCombobox(classes);
            BindGrid(students);
            SetButtonState(true);
        }

        private void FillFacultyCombobox(List<Lop> listClasses)
        {
            cboLop.DataSource = listClasses;
            cboLop.DisplayMember = "TenLop";
            cboLop.ValueMember = "MaLop";
        }

        private void BindGrid(List<Sinhvien> listStudents)
        {
            dgvSinhVien.Rows.Clear();
            foreach (var student in listStudents)
            {
                int index = dgvSinhVien.Rows.Add();
                dgvSinhVien.Rows[index].Cells[0].Value = student.MaSV;
                dgvSinhVien.Rows[index].Cells[1].Value = student.HoTenSV;
                dgvSinhVien.Rows[index].Cells[2].Value = student.NgaySinh.HasValue ? student.NgaySinh.Value.ToString("dd/MM/yyyy") : "";
                dgvSinhVien.Rows[index].Cells[3].Value = student.Lop?.TenLop;
            }
        }

        public void SetButtonState(bool e)
        {
            btnThem.Enabled = e;
            btnSua.Enabled = e;
            btnXoa.Enabled = e;
            btnLuu.Enabled = !e;
            btnKLuu.Enabled = !e;
            btnThoat.Enabled = e;
        }

        private void ResetInputs()
        {
            txtMaSV.Clear();
            txtHotenSV.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            if (cboLop.Items.Count > 0)
                cboLop.SelectedIndex = 0;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            currentAction = "Add";
            SetButtonState(false);
            ResetInputs();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSV.Text))
            {
                MessageBox.Show("Hãy chọn sinh viên cần sửa!");
                return;
            }
            currentAction = "Edit";
            SetButtonState(false);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaSV.Text))
            {
                MessageBox.Show("Hãy chọn sinh viên cần xóa!");
                return;
            }

            studentService.DeleteStudent(txtMaSV.Text);
            MessageBox.Show("Xóa thành công!");
            BindGrid(studentService.GetAllStudents());
            ResetInputs();
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
            var student = new Sinhvien
            {
                MaSV = txtMaSV.Text,
                HoTenSV = txtHotenSV.Text,
                NgaySinh = dtpNgaySinh.Value,
                MaLop = cboLop.SelectedValue.ToString()
            };

            if (currentAction == "Add")
            {
                studentService.AddStudent(student);
                MessageBox.Show("Thêm thành công!");
            }
            else if (currentAction == "Edit")
            {
                studentService.UpdateStudent(student);
                MessageBox.Show("Sửa thành công!");
            }

            BindGrid(studentService.GetAllStudents());
            ResetInputs();
            SetButtonState(true);
        }

        private void btnKLuu_Click(object sender, EventArgs e)
        {
            ResetInputs();
            SetButtonState(true);
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtTimKiem.Text.Trim().ToLower();

                var result = studentService.GetAllStudents()
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
                    BindGrid(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }    
        }
    }
}