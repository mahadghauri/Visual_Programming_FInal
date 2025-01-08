using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Question_1
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString = "Data Source=DESKTOP-GHAURI\\SQLEXPRESS;Initial Catalog=StudentTrackerDB;Integrated Security=True";

        public MainWindow()
        {
            InitializeComponent();
            LoadStudents();
            LoadFilters();
        }

        private void LoadStudents()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Students";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                StudentDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }


        private void LoadFilters()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                SqlCommand gradeCmd = new SqlCommand("SELECT DISTINCT Grade FROM Students", connection);
                SqlDataReader gradeReader = gradeCmd.ExecuteReader();
                while (gradeReader.Read())
                {
                    GradeFilter.Items.Add(gradeReader["Grade"].ToString());
                }
                gradeReader.Close();

                
                SqlCommand subjectCmd = new SqlCommand("SELECT DISTINCT Subject FROM Students", connection);
                SqlDataReader subjectReader = subjectCmd.ExecuteReader();
                while (subjectReader.Read())
                {
                    SubjectFilter.Items.Add(subjectReader["Subject"].ToString());
                }
                subjectReader.Close();
            }
        }


        private void ApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string grade = GradeFilter.Text;
                string subject = SubjectFilter.Text;

                string query = "SELECT * FROM Students WHERE 1=1";
                if (!string.IsNullOrEmpty(grade))
                { 
                    query += " AND Grade = @Grade";
                }
                if (!string.IsNullOrEmpty(subject))
                {
                    query += " AND Subject = @Subject";
                }   

                SqlCommand cmd = new SqlCommand(query, connection);
                if (!string.IsNullOrEmpty(grade))
                    cmd.Parameters.AddWithValue("@Grade", grade);
                if (!string.IsNullOrEmpty(subject))
                    cmd.Parameters.AddWithValue("@Subject", subject);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                StudentDataGrid.ItemsSource = dataTable.DefaultView;
            }
        }


        private Student GetStudentById(int studentId)
        {
            Student student = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Students WHERE StudentID = @StudentID";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    student = new Student
                    {
                        StudentID = (int)reader["StudentID"],
                        Name = reader["Name"].ToString(),
                        Grade = reader["Grade"].ToString(),
                        Subject = reader["Subject"].ToString(),
                        Marks = Convert.IsDBNull(reader["Marks"]) ? 0 : Convert.ToInt32(reader["Marks"]), 
                        Attendance = Convert.IsDBNull(reader["Attendance"]) ? 0 : Convert.ToInt32(reader["Attendance"]) 
                    };
                }
            }

            return student;
        }




        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            AddStudentsWindow addWindow = new AddStudentsWindow();
            addWindow.ShowDialog(); 
            LoadStudents(); 
        }



        private void EditStudent_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)StudentDataGrid.SelectedItem;
                int studentId = Convert.ToInt32(selectedRow["StudentID"]);


                Student studentToEdit = GetStudentById(studentId);

                
                AddStudentsWindow addWindow = new AddStudentsWindow(studentToEdit);
                addWindow.ShowDialog(); 


                LoadStudents();
            }
            else
            {
                MessageBox.Show("Please select a student to edit.");
            }
        }



        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            if (StudentDataGrid.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)StudentDataGrid.SelectedItem;
                int studentId = Convert.ToInt32(selectedRow["StudentID"]);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("DELETE FROM Students WHERE StudentID = @StudentID", connection);
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Student deleted successfully.");
                LoadStudents();
            }
            else
            {
                MessageBox.Show("Please select a student to delete.");
            }
        }
    }
}
