using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsAppLab2
{
    public class DatabaseManager
    {
        // Строка подключения к базе данных с таблицами
        private string connectionString = "Data Source=StudentDatabase.db;Version=3;";

        // Создание пустых таблиц Студентов и Оценок
        public void CreateDatabase()
        {
            // В using мы говорим, что будем использовать ресурс базы данных
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // SQL команда создания таблицы
                string query = "CREATE TABLE IF NOT EXISTS Students (StudentID INTEGER PRIMARY KEY, FullName TEXT, PhoneNumber TEXT);";
                // Исполнитель комманды
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();

                query = "CREATE TABLE IF NOT EXISTS Grades (StudentID INTEGER, PhysicsGrade DOUBLE, MathGrade DOUBLE, FOREIGN KEY (StudentID) REFERENCES Students(StudentID));";
                command = new SQLiteCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }

        public void InsertStudent(Student student)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Запрос для получения максимального текущего ID из таблицы Students
                string getMaxIdQuery = "SELECT MAX(StudentID) FROM Students;";
                SQLiteCommand getMaxIdCommand = new SQLiteCommand(getMaxIdQuery, connection);
                object result = getMaxIdCommand.ExecuteScalar();

                int newStudentId = result is DBNull ? 1 : Convert.ToInt32(result) + 1; // Генерация нового ID

                // Записываем студента в таблицу студентов (имя, номер)
                string query = "INSERT INTO Students (StudentID, FullName, PhoneNumber) VALUES (@StudentID, @FullName, @PhoneNumber);";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@StudentID", newStudentId);
                command.Parameters.AddWithValue("@FullName", student.FullName);
                command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                command.ExecuteNonQuery();

                // Записываем оценки студента в таблицу оценок
                query = "INSERT INTO Grades (StudentID, PhysicsGrade, MathGrade) VALUES (@StudentID, @PhysicsGrade, @MathGrade);";
                command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@StudentID", newStudentId);
                command.Parameters.AddWithValue("@PhysicsGrade", student.PhysicsGrade);
                command.Parameters.AddWithValue("@MathGrade", student.MathGrade);
                command.ExecuteNonQuery();
            }
        }

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Получаем список студентов с их оценками, как результат операции слияния таблиц студентов и их оценок по идентификатору студентов
                string query = "SELECT Students.StudentID, Students.FullName, Students.PhoneNumber, Grades.PhysicsGrade, Grades.MathGrade FROM Students JOIN Grades ON Students.StudentID = Grades.StudentID;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                // Конвертируем данные из таблицы, полученные в reader, в объект типа Student
                while (reader.Read())
                {
                    Student student = new Student()
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        FullName = Convert.ToString(reader["FullName"]),
                        PhoneNumber = Convert.ToString(reader["PhoneNumber"]),
                        PhysicsGrade = Convert.ToDouble(reader["PhysicsGrade"]),
                        MathGrade = Convert.ToDouble(reader["MathGrade"])
                    };

                    students.Add(student);
                }
            }

            return students;
        }

        public void UpdateStudent(Student student)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Обновляем данные студента в таблице студентов по идентификатору
                string query = "UPDATE Students SET FullName = @FullName, PhoneNumber = @PhoneNumber WHERE StudentID = @StudentID;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@FullName", student.FullName);
                command.Parameters.AddWithValue("@PhoneNumber", student.PhoneNumber);
                command.Parameters.AddWithValue("@StudentID", student.StudentID);
                command.ExecuteNonQuery();

                // Обновляем оценки студента в таблице оценок по идентификатору
                query = "UPDATE Grades SET PhysicsGrade = @PhysicsGrade, MathGrade = @MathGrade WHERE StudentID = @StudentID;";
                command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@PhysicsGrade", student.PhysicsGrade);
                command.Parameters.AddWithValue("@MathGrade", student.MathGrade);
                command.Parameters.AddWithValue("@StudentID", student.StudentID);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteStudent(int studentID)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "DELETE FROM Students WHERE StudentID = @StudentID;";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@StudentID", studentID);
                command.ExecuteNonQuery();

                query = "DELETE FROM Grades WHERE StudentID = @StudentID;";
                command = new SQLiteCommand(query, connection);
                command.Parameters.AddWithValue("@StudentID", studentID);
                command.ExecuteNonQuery();
            }
        }
    }
}
