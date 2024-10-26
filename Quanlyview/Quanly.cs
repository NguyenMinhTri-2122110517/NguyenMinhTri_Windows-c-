﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing; // Ensure you have this using directive for Image

namespace Quanlyview
{
    public partial class Quanly : Form
    {
        public List<Employee> lstEmp = new List<Employee>();
        private BindingSource bs = new BindingSource();
        public bool isThoat = true;
        public event EventHandler DangXuat;

        private string employeeImagePath = string.Empty; // Store the image path

        public Quanly()
        {
            InitializeComponent();
            SetupImageList();

            //ngay sinh
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd MMMM yyyy";
            // Handle value changes (optional)
            dateTimePicker1.ShowUpDown = true;
        }

        private void Quanly_Load(object sender, EventArgs e)
        {
            lstEmp = GetData();
            bs.DataSource = lstEmp;
            dgvEmployee.DataSource = bs;
            SetupDataGridView(); // Setup DataGridView columns
            dateTimePicker1.Value = DateTime.Now; // Set the default date to now

        }

        public List<Employee> GetData()
        {
            // Sample data can be added here if needed
            return lstEmp;
        }

        private void SetupDataGridView()
        {
            dgvEmployee.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployee.Columns[0].HeaderText = "Mã sv";
            dgvEmployee.Columns[1].HeaderText = "Tên sv";
            dgvEmployee.Columns[2].HeaderText = "Ngày Sinh";
            dgvEmployee.Columns[3].HeaderText = "Giới Tính";
            dgvEmployee.Columns[4].HeaderText = "Địa Chỉ";
            dgvEmployee.Columns[5].HeaderText = "Số Điện Thoại";
            dgvEmployee.Columns[6].HeaderText = "Email";
            dgvEmployee.Columns[7].HeaderText = "Mã Lớp";
            dgvEmployee.Columns[7].HeaderText = "Ngành Học";
            dgvEmployee.Columns[8].HeaderText = "Ảnh"; // Add header for Birth Date
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btDangXuat_Click(object sender, EventArgs e)
        {
            DangXuat?.Invoke(this, EventArgs.Empty);
        }

        private void Quanly_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isThoat) Application.Exit();
        }

        private void btAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                tbId.Enabled = true; // Đảm bảo mở khóa ô ID khi thêm mới

                // Kiểm tra từng trường dữ liệu có hợp lệ không
                if (string.IsNullOrWhiteSpace(tbId.Text))
                {
                    MessageBox.Show("Lỗi: Vui lòng nhập mã sinh viên.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbName.Text))
                {
                    MessageBox.Show("Lỗi: Vui lòng nhập tên sinh viên.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbAddress.Text))
                {
                    MessageBox.Show("Lỗi: Vui lòng nhập địa chỉ.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbPhone.Text))
                {
                    MessageBox.Show("Lỗi: Vui lòng nhập số điện thoại.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbEmail.Text))
                {
                    MessageBox.Show("Lỗi: Vui lòng nhập email.");
                    return;
                }

                if (string.IsNullOrWhiteSpace(cbMalop.Text))
                {
                    MessageBox.Show("Lỗi: Vui lòng chọn mã lớp.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(cbNganhhoc.Text))
                {
                    MessageBox.Show("Lỗi: Vui lòng chọn mã lớp.");
                    return;
                }

                if (string.IsNullOrEmpty(employeeImagePath))
                {
                    MessageBox.Show("Lỗi: Vui lòng chọn một ảnh.");
                    return;
                }

                // Kiểm tra ID có trùng lặp không
                int newId = int.Parse(tbId.Text);
                if (lstEmp.Any(emp => emp.Id == newId))
                {
                    MessageBox.Show("Lỗi: ID đã tồn tại. Vui lòng nhập ID khác.");
                    return;
                }

                // Tạo đối tượng Employee mới
                Employee newEmp = new Employee
                {
                    Id = newId,
                    Name = tbName.Text,
                    Gender = ckGender.Checked,
                    Address = tbAddress.Text,
                    Sodienthoai = tbPhone.Text,
                    Email = tbEmail.Text,
                    Malop = cbMalop.Text,
                    Nganhhoc = cbNganhhoc.Text,
                    ImagePath = employeeImagePath,
                    BirthDate = dateTimePicker1.Value.Date
                };

                // Thêm vào danh sách và cập nhật DataGridView
                lstEmp.Add(newEmp);
                bs.ResetBindings(false);
                ClearInputFields(); // Xóa dữ liệu các ô nhập sau khi thêm xong

                tbId.Enabled = true; // Đảm bảo mở lại ô ID cho lần thêm tiếp theo
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi: Vui lòng nhập số nguyên hợp lệ cho ID.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }





        private void btEdit_Click(object sender, EventArgs e)
        {
            if (dgvEmployee.CurrentRow == null) return;

            int idx = dgvEmployee.CurrentRow.Index;
            Employee em = lstEmp[idx];

            try
            {
                em.Id = int.Parse(tbId.Text);
                em.Name = tbName.Text;
                em.Gender = ckGender.Checked;
                em.Address = tbAddress.Text;
                em.Sodienthoai = tbPhone.Text;
                em.Email = tbPhone.Text;
                em.Malop = cbMalop.Text;
                em.Nganhhoc = cbNganhhoc.Text;
                em.ImagePath = employeeImagePath; // Save the image path
                em.BirthDate = dateTimePicker1.Value.Date; // Update the BirthDate from DateTimePicker
                bs.ResetBindings(false);
                ClearInputFields();
            }
            catch (FormatException)
            {
                MessageBox.Show("Lỗi: Vui lòng nhập số nguyên hợp lệ cho ID.");
            }
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if (dgvEmployee.CurrentRow == null) return;

            int idx = dgvEmployee.CurrentRow.Index;
            lstEmp.RemoveAt(idx);
            bs.ResetBindings(false);
        }

        private void dgvEmployee_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= lstEmp.Count) return;

            Employee em = lstEmp[e.RowIndex];

            tbId.Text = em.Id.ToString();
            tbName.Text = em.Name;
            ckGender.Checked = em.Gender;
            tbAddress.Text = em.Address;
            tbPhone.Text = em.Sodienthoai;
            tbEmail.Text = em.Email;
            cbMalop.Text = em.Malop;
            cbNganhhoc.Text = em.Nganhhoc;

            // Kiểm tra ngày sinh có hợp lệ không
            dateTimePicker1.Value = (em.BirthDate != DateTime.MinValue) ? em.BirthDate : DateTime.Now;

            // Load employee image if exists
            if (!string.IsNullOrEmpty(em.ImagePath) && System.IO.File.Exists(em.ImagePath))
            {
                pbEmployeeImage.Image = Image.FromFile(em.ImagePath);
                pbEmployeeImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            else
            {
                pbEmployeeImage.Image = null;
            }

            tbId.Enabled = false;
        }



        private void ClearInputFields()
        {
            tbId.Text = "";
            tbName.Text = "";
            tbAddress.Text = "";
            tbPhone.Text = "";
            tbEmail.Text = "";
            cbMalop.Text = "";
            cbNganhhoc.Text = "";
            ckGender.Checked = false;
            pbEmployeeImage.Image = null; // Clear image display
            dateTimePicker1.Value = DateTime.Now; // Reset DateTimePicker to current date

            tbId.Enabled = true;
        }

        private void SetupImageList()
        {
            ImageList imageList = new ImageList();
            imageList.ImageSize = new Size(24, 24);

            // Add images to ImageList (make sure paths are correct)
            imageList.Images.Add(Image.FromFile("Images/add.png"));    // Index 0
            imageList.Images.Add(Image.FromFile("Images/edit.png"));   // Index 1
            imageList.Images.Add(Image.FromFile("Images/delete.png")); // Index 2

            // Assign ImageList to buttons
            btAddNew.ImageList = imageList;
            btAddNew.ImageIndex = 0;

            btEdit.ImageList = imageList;
            btEdit.ImageIndex = 1;

            btDelete.ImageList = imageList;
            btDelete.ImageIndex = 2;
        }

        private void btSelectImage_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem tên đã được nhập hay chưa
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Lỗi: Vui lòng nhập tên trước khi chọn ảnh.");
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    employeeImagePath = ofd.FileName; // Lưu đường dẫn ảnh

                    // Kiểm tra đường dẫn trước khi hiển thị
                    if (System.IO.File.Exists(employeeImagePath))
                    {
                        pbEmployeeImage.Image = Image.FromFile(employeeImagePath);
                        pbEmployeeImage.SizeMode = PictureBoxSizeMode.StretchImage; // Chỉnh lại kích thước ảnh
                    }
                    else
                    {
                        MessageBox.Show("Không thể tải hình ảnh.");
                    }
                }
            }
        }


        // Method to set a specific date for the DateTimePicker (if needed)
        private void SetDateForDateTimePicker(DateTime date)
        {
            dateTimePicker1.Value = date; // Set a specific date, e.g. new DateTime(2024, 10, 17)
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = dateTimePicker1.Value;
            // Do something with the selected date
            this.Text = dateTimePicker1.Value.ToString("dd MMMM yyyy");
        }

        private void dgvEmployee_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void tbPhone_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbAddress_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
