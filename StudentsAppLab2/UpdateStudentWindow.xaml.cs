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
    /// Логика взаимодействия для UpdateStudentWindow.xaml
    /// </summary>
    public partial class UpdateStudentWindow : Window
    {
        private DatabaseManager dbManager;
        private Student studentToEdit;

        public UpdateStudentWindow(Student student, DatabaseManager manager)
        {
            InitializeComponent();

            dbManager = manager;
            studentToEdit = student;

            // Заполнение полей данными студента
            txtStudentID.Text = studentToEdit.StudentID.ToString();
            txtFullName.Text = studentToEdit.FullName;
            txtPhoneNumber.Text = studentToEdit.PhoneNumber;
            txtPhysicsGrade.Text = studentToEdit.PhysicsGrade.ToString();
            txtMathGrade.Text = studentToEdit.MathGrade.ToString();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Обновление данных студента
            studentToEdit.FullName = txtFullName.Text;
            studentToEdit.PhoneNumber = txtPhoneNumber.Text;
            studentToEdit.PhysicsGrade = Convert.ToDouble(txtPhysicsGrade.Text);
            studentToEdit.MathGrade = Convert.ToDouble(txtMathGrade.Text);

            // Вызов метода обновления студента из базы данных
            dbManager.UpdateStudent(studentToEdit);

            MessageBox.Show("Student updated successfully.");

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
