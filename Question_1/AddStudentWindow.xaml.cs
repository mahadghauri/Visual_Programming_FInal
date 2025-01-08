using System;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Question_1;  

namespace Question_1
{
    
        public class Student
        {
            public int StudentID { get; set; } 
            public string Name { get; set; }
            public string Grade { get; set; }
            public string Subject { get; set; }
            public int Marks { get; set; }
            public int Attendance { get; set; }
    }
    
        public partial class AddStudentsWindow : Window
        {
            private string connectionString = "Data Source=DESKTOP-GHAURI\\SQLEXPRESS;Initial Catalog=StudentTrackerDB;Integrated Security=True";
            private Student studentToEdit;  

            
            public AddStudentsWindow()
            {
                InitializeComponent();
            }


            public AddStudentsWindow(Student student)
            {
                InitializeComponent();
                studentToEdit = student;


                if (student != null)
                {
                    NameTextBox.Text = student.Name;
                    GradeComboBox.SelectedItem = GradeComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == student.Grade);
                    SubjectComboBox.SelectedItem = SubjectComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == student.Subject);
                    MarksTextBox.Text = student.Marks.ToString();
                    AttendanceTextBox.Text = student.Attendance.ToString();
                }
            }

            private void SaveButton_Click(object sender, RoutedEventArgs e)
            {
                // Validate inputs
                if (string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                    GradeComboBox.SelectedItem == null ||
                    SubjectComboBox.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(MarksTextBox.Text) ||
                    string.IsNullOrWhiteSpace(AttendanceTextBox.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
 
                    Student newStudent = new Student
                    {
                        Name = NameTextBox.Text,
                        Grade = (GradeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                        Subject = (SubjectComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                        Marks = int.Parse(MarksTextBox.Text),
                        Attendance = int.Parse(AttendanceTextBox.Text)
                    };


                    if (studentToEdit != null)
                    {
                        newStudent.StudentID = studentToEdit.StudentID;
                        UpdateStudentInDatabase(newStudent);
                    }
                    else
                    {
                        AddStudentToDatabase(newStudent);
                    }

                    MessageBox.Show("Student saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


            private void AddStudentToDatabase(Student student)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Students (Name, Grade, Subject, Marks, Attendance) VALUES (@Name, @Grade, @Subject, @Marks, @Attendance)";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@Grade", student.Grade);
                    cmd.Parameters.AddWithValue("@Subject", student.Subject);
                    cmd.Parameters.AddWithValue("@Marks", student.Marks);
                    cmd.Parameters.AddWithValue("@Attendance", student.Attendance);
                    cmd.ExecuteNonQuery();
                }
            }


            private void UpdateStudentInDatabase(Student student)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Students SET Name = @Name, Grade = @Grade, Subject = @Subject, Marks = @Marks, Attendance = @Attendance WHERE StudentID = @StudentID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@Grade", student.Grade);
                    cmd.Parameters.AddWithValue("@Subject", student.Subject);
                    cmd.Parameters.AddWithValue("@Marks", student.Marks);
                    cmd.Parameters.AddWithValue("@Attendance", student.Attendance);
                    cmd.Parameters.AddWithValue("@StudentID", student.StudentID);  
                    cmd.ExecuteNonQuery();
                }
            }

            private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                this.Close();
            }
        }
    }
