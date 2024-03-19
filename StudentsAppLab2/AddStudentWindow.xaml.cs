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

namespace StudentsAppLab2
{
    /// <summary>
    /// Логика взаимодействия для AddStudentWindow.xaml
    /// </summary>
    public partial class AddStudentWindow : Window
    {
        private readonly DatabaseManager dbManager;

        public AddStudentWindow(DatabaseManager dbManager)
        {
            InitializeComponent();
            this.dbManager = dbManager;
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text;
            double physicsGrade = Convert.ToDouble(txtPhysicsGrade.Text);
            double mathGrade = Convert.ToDouble(txtMathGrade.Text);
            string phoneNumber = txtPhoneNumber.Text;

            Student student = new Student()
            {
                FullName = fullName,
                PhysicsGrade = physicsGrade,
                MathGrade = mathGrade,
                PhoneNumber = phoneNumber
            };

            dbManager.InsertStudent(student);
            MessageBox.Show("Student added successfully!");
            this.Close();
        }
    }
}
