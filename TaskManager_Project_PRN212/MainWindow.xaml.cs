using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using TaskManager_Project_PRN212.Models;

namespace TaskManager_Project_PRN212
{
    public partial class MainWindow : Window
    {
        private TaskManagerDbContext _context;

        private User _currentUser; 

        private ObservableCollection<Models.Task> _tasks;

        public MainWindow(User user)
        {
            InitializeComponent();
            _context = new TaskManagerDbContext();
            _currentUser = user; 
            txtWelcome.Text = $"Xin chào, {_currentUser.FullName}";
            LoadTasks();
        }

        private void LoadTasks()
        {
            _tasks = new ObservableCollection<Models.Task>(_context.Tasks
                .Where(t => t.UserId == _currentUser.Id)
                .Include(t => t.Category)
                .ToList());
            dgTasks.ItemsSource = _tasks;
        }


        private void FilterTask_Click(object sender, RoutedEventArgs e)
        {
            var selectedStatus = (cbStatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();

            // Chuyển đổi trạng thái để so sánh với dữ liệu trong DB
            string statusDbValue = selectedStatus switch
            {
                "Chưa bắt đầu" => "Pending",
                "Đang thực hiện" => "In Progress",
                "Hoàn thành" => "Completed",
                _ => null 
            };

            var tasks = _context.Tasks
                .Where(t => t.UserId == _currentUser.Id)
                .Where(t => statusDbValue == null || t.Status == statusDbValue)
                .Include(t => t.Category)
                .ToList();

            dgTasks.ItemsSource = tasks;
        }

        private void SearchTasks_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();

            var filteredTasks = _context.Tasks
                .Where(t => t.UserId == _currentUser.Id)
                .Include(t => t.Category)
                .AsQueryable();

            // Tìm kiếm theo Title, Description, và Category.Name, xử lí null
            filteredTasks = filteredTasks.Where(t =>
                (t.Title != null && t.Title.ToLower().Contains(searchText)) ||
                (t.Description != null && t.Description.ToLower().Contains(searchText)) ||
                (t.Category != null && t.Category.Name != null && t.Category.Name.ToLower().Contains(searchText)));

            // Kết hợp với bộ lọc trạng thái
            string selectedStatus = (cbStatusFilter.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedStatus != "Tất cả")
            {
                string dbStatus = selectedStatus switch
                {
                    "Chưa bắt đầu" => "Pending",
                    "Đang thực hiện" => "In Progress",
                    "Hoàn thành" => "Completed",
                    _ => "Pending"
                };
                filteredTasks = filteredTasks.Where(t => t.Status == dbStatus);
            }


            _tasks.Clear();
            foreach (var task in filteredTasks.ToList())
            {
                _tasks.Add(task);
            }
            dgTasks.ItemsSource = _tasks;
        }


        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            var addTaskWindow = new AddTaskWindow();
            if (addTaskWindow.ShowDialog() == true)
            {
                var newTask = addTaskWindow.NewTask;
                newTask.UserId = _currentUser.Id; 
                _context.Tasks.Add(newTask);
                _context.SaveChanges();
                LoadTasks();
            }
        }
        

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is Models.Task selectedTask)
            {
                EditWindow editWindow = new EditWindow(selectedTask);
                if (editWindow.ShowDialog() == true)
                {
                    _context.Tasks.Update(editWindow.TaskToEdit);
                    _context.SaveChanges();
                    LoadTasks();
                }
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedItem is Models.Task selectedTask &&
                MessageBox.Show("Bạn có chắc muốn xóa công việc này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _context.Tasks.Remove(selectedTask);
                _context.SaveChanges();
                LoadTasks();
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Logged out successfully!");
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            Close();

        }

    }
}
