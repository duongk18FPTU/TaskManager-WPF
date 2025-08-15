using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TaskManager_Project_PRN212.Models;

namespace TaskManager_Project_PRN212
{
    public partial class EditWindow : Window
    {
        private TaskManagerDbContext _context;
        public Models.Task TaskToEdit { get; private set; }

        public EditWindow(Models.Task task)
        {
            InitializeComponent();
            _context = new TaskManagerDbContext();
            TaskToEdit = task;
            LoadTaskDetails();
            LoadCategory();
        }

        private void LoadTaskDetails()
        {
            txtTaskName.Text = TaskToEdit.Title;
            txtDescription.Text = TaskToEdit.Description;
            dpDueDate.SelectedDate = TaskToEdit.DueDate;
            cbHour.SelectedItem = TaskToEdit.DueDate.Hour.ToString("D2");
            cbMinute.SelectedItem = TaskToEdit.DueDate.Minute.ToString("D2");
            cbStatus.SelectedItem = TaskToEdit.Status switch
            {
                "Pending" => "Chưa bắt đầu",
                "Completed" => "Hoàn thành",
                "In Progress" => "Đang thực hiện",
                _ => "Chưa bắt đầu"
            };
            cbCategory.SelectedValue = TaskToEdit.CategoryId;
        }

        private void LoadCategory()
        {
            var categories = _context.Categories.ToList();
            cbCategory.ItemsSource = categories;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTaskName.Text) || dpDueDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DateTime selectedDate = dpDueDate.SelectedDate.Value;
            int hour = int.Parse((cbHour.SelectedItem as ComboBoxItem).Content.ToString());
            int minute = int.Parse((cbMinute.SelectedItem as ComboBoxItem).Content.ToString());
            DateTime dueDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);

            string selectedStatus = (cbStatus.SelectedItem as ComboBoxItem)?.Content.ToString();
            string statusDbValue = selectedStatus switch
            {
                "Chưa bắt đầu" => "Pending",
                "Hoàn thành" => "Completed",
                "Đang thực hiện" => "In Progress",
                _ => "Pending"
            };

            TaskToEdit.Title = txtTaskName.Text;
            TaskToEdit.Description = txtDescription.Text;
            TaskToEdit.DueDate = dueDateTime;
            TaskToEdit.Status = statusDbValue;
            TaskToEdit.CategoryId = (int?)cbCategory.SelectedValue;

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


