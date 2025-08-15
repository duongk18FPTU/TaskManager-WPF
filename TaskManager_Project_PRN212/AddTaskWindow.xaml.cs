using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.EntityFrameworkCore;
using TaskManager_Project_PRN212.Models;

namespace TaskManager_Project_PRN212
{
    public partial class AddTaskWindow : Window
    {
        public Models.Task NewTask { get; private set; }
        private TaskManagerDbContext _context;

        public AddTaskWindow()
        {
            InitializeComponent();
            _context = new TaskManagerDbContext();
            LoadCategory(); // Gọi hàm load danh mục
        }

        private void LoadCategory()
        {
            var categories = _context.Categories.ToList();
            cbCategory.ItemsSource = categories;
        }
        

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTaskName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (dpDueDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày hạn chót.");
                return;
            }
            if (cbHour.SelectedItem == null || cbMinute.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn giờ và phút!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            //Lấy mô tả từ TextBox txtDescription
            string description = txtDescription.Text;
            //Lấy doanh mục từ ComboBox cbCategory
            string selectedCategory = (cbCategory.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Lấy ngày từ DatePicker
            DateTime selectedDate = dpDueDate.SelectedDate.Value;

            // Lấy giờ và phút từ ComboBox
            int hour = int.Parse((cbHour.SelectedItem as ComboBoxItem).Content.ToString());
            int minute = int.Parse((cbMinute.SelectedItem as ComboBoxItem).Content.ToString());

            // Kết hợp ngày, giờ, phút thành DateTime
            DateTime dueDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
            string selectedStatus = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Chuyển đổi trạng thái sang giá trị phù hợp với database
            string dbStatus = selectedStatus switch
            {
                "Chưa hoàn thành" => "Pending",
                "Đang thực hiện" => "In Progress",
                "Hoàn thành" => "Completed",
                _ => "Pending"
            };

            NewTask = new Models.Task
            {
                Title = txtTaskName.Text,
                DueDate = dueDateTime,
                Status = dbStatus,
                Description = description,
                CategoryId = (int?)cbCategory.SelectedValue
            };

            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
