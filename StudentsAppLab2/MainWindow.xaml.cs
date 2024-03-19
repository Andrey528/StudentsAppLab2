using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StudentsAppLab2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseManager dbManager;

        public MainWindow()
        {
            InitializeComponent();
            dbManager = new DatabaseManager();
            dbManager.CreateDatabase();
            LoadStudentsData();
        }

        private void LoadStudentsData()
        {
            List<Student> students = dbManager.GetAllStudents();

            studentsDataGrid.ItemsSource = students;
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentWindow addStudentWindow = new AddStudentWindow(dbManager);
            addStudentWindow.ShowDialog();
            LoadStudentsData();
        }

        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (studentsDataGrid.SelectedItem != null)
            {
                Student selectedStudent = (Student)studentsDataGrid.SelectedItem;
                dbManager.DeleteStudent(selectedStudent.StudentID);
                LoadStudentsData();
            }
        }

        private void UpdateStudent_Click(object sender, RoutedEventArgs e)
        {
            if (studentsDataGrid.SelectedItem != null)
            {
                Student selectedStudent = (Student)studentsDataGrid.SelectedItem;
                UpdateStudentWindow updateStudentWindow = new UpdateStudentWindow(selectedStudent, dbManager);
                updateStudentWindow.ShowDialog();
                LoadStudentsData();
            }
        }
    }
}