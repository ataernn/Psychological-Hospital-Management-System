using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Psychological_Hospital_Management_System
{
    public class SQL
    {
        private string connectionString = "Server=localhost;Database=phmsystem;Uid=root;Pwd=201079;"; // Connection string to connect to the MySQL database.

        private MySqlConnection GetConnection() // Method to create and return a new MySQL connection.
        {
            return new MySqlConnection(connectionString);
        }


        public bool CheckUserLogin(string id, string password, out string userType, out string userName, out string userSurname) // Method to check user login credentials.
        {
            userType = ""; // Initialize userType.
            userName = ""; // Initialize Name.
            userSurname = ""; // Initialize Surname.

            try
            {
                using (var connection = GetConnection()) // For connect to the database.
                {
                    connection.Open(); // Open the connection.
                    string query = "SELECT user_type, name, surname FROM users WHERE id = @id AND password = @password"; // SQL query to retrieve user details.
                    MySqlCommand cmd = new MySqlCommand(query, connection); // Create a command with the query.
                    // Adding parameters to prevent SQL injection.
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@password", password);

                    // Execute the command and read the results.
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())  // If a record is found.
                        {
                            userType = reader["user_type"].ToString();  // If a record is found.
                            userName = reader["name"].ToString(); // Get the name.
                            userSurname = reader["surname"].ToString(); // Get the surname
                            return true; // Continue if login is successful.
                        }
                        else
                        {
                            return false; // No record found. Go back.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Critical Error: " + ex.Message, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message in case of an connection failure or something error.
                return false; // Go back if error appears.
            }
        }

        public bool AddUser(string id, string name, string surname, string password, string userType) // Method to add a new user to the database.
        {
            try
            {
                using (var connection = GetConnection()) // For connect to the database.
                {
                    connection.Open(); // Open the connection.

                    string userTypeInDb; // Variable to store user type for database.
                    // Determine user type based on input.
                    if (userType == "Doctor")
                    {
                        userTypeInDb = "doctor"; // Convert to lowercase for saving in the database.
                    }
                    else if (userType == "Patient")
                    {
                        userTypeInDb = "patient";  // Convert to lowercase for saving in the database.
                    }
                    else
                    {
                        MessageBox.Show("Invalid user type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error for invalid user type.
                        return false; // Go back if error appears.
                    }

                    string query = "INSERT INTO users (id, name, surname, password, user_type) VALUES (@id, @name, @surname, @password, @user_type)"; // SQL query to insert new user into the users table.
                    MySqlCommand cmd = new MySqlCommand(query, connection); // Create a command with the query.
                    // Adding parameters to prevent SQL injection.
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@surname", surname);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@user_type", userTypeInDb);

                    int rowsAffected = cmd.ExecuteNonQuery(); // Execute the command and get the number of rows affected.

                    return rowsAffected > 0; // Continue if one or more rows were affected.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Critical Error: " + ex.Message, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message in case of an connection failure or something error.
                return false; // Go back if error appears.
            }
        }

        public bool SaveTestResults(string userName, string userSurname, int totalScore) // Method to save test results into the database.
        {
            try
            {
                using (var connection = GetConnection()) // For connect to the database.
                {
                    connection.Open(); // Open the connection.
                    string query = "INSERT INTO test_results (name, surname, total_score) VALUES (@name, @surname, @total_score)";
                    MySqlCommand cmd = new MySqlCommand(query, connection); // Create a command with the query.
                    // Adding parameters to prevent SQL injection.
                    cmd.Parameters.AddWithValue("@name", userName);
                    cmd.Parameters.AddWithValue("@surname", userSurname);
                    cmd.Parameters.AddWithValue("@total_score", totalScore);

                    int rowsAffected = cmd.ExecuteNonQuery(); // Execute the command and get the number of rows affected.

                    return rowsAffected > 0; // Continue if one or more rows were affected.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Critical Error: " + ex.Message, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message in case of an connection failure or something error.
                return false; // Go back if error appears.
            }
        }

        public DataTable GetPatientResults() // Method to retrieve all patient test results from the database.
        {
            DataTable dataTable = new DataTable(); // Create a DataTable to store results.
            try
            {
                using (var connection = GetConnection()) // Establish a connection to the database.
                {
                    connection.Open(); // Open the database connection.
                    string query = "SELECT name, surname, total_score, created_at FROM test_results"; // SQL query to select relevant columns from the test_results table.
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection); // Create an adapter to fill the DataTable.
                    adapter.Fill(dataTable); // Fill the DataTable with the results.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Critical Error: " + ex.Message, "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message in case of an connection failure or something error.
            }
            return dataTable; // Return the DataTable containing the results.
        }
    }
}
