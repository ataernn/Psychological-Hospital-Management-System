using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Psychological_Hospital_Management_System
{
    public partial class DoctorForm : Form
    {
        private SQL sqlHelper = new SQL();
        private string userId;
        private string userName;
        private string userSurname;

        public DoctorForm(string userId, string userName, string userSurname)
        {
            InitializeComponent();
            LoadTestResults(); // Calls the method to load patient test results

            // Assigning the constructor parameters to the private fields.
            this.userId = userId;
            this.userName = userName;
            this.userSurname = userSurname;

            this.Text = $"Dr: {userName} {userSurname}"; // Sets the title of the form to display the doctor's name.
            this.FormClosing += new FormClosingEventHandler(DoctorForm_FormClosing); // Event handler for when the form is closing.
            this.StartPosition = FormStartPosition.CenterScreen; // Centering the form on the screen.
            this.MaximizeBox = false; // Disabling the maximize button to prevent resizing.
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Preventing the form from being resizable by using a fixed single border style.
        }

        private void DoctorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Exit the application.
        }

        private void LoadTestResults() // Method to load patient test results into a DataGridView.
        {
            DataTable resultsTable = sqlHelper.GetPatientResults(); // Gets patient test results from the SQL helper class.
            if (resultsTable != null) // If the result is not null, continue processing the table.
            {
                DataGridView.DataSource = resultsTable; // Binds the data table to the DataGridView.

                // Renaming columns for better readability in the DataGridView.
                resultsTable.Columns["name"].ColumnName = "Name";
                resultsTable.Columns["surname"].ColumnName = "Surname";
                resultsTable.Columns["total_score"].ColumnName = "Total Score";
                resultsTable.Columns["created_at"].ColumnName = "Created At";

                if (resultsTable.Columns.Contains("id")) // Hides the id column if it exists, as it is not needed in the view.
                {
                    DataGridView.Columns["id"].Visible = false;
                }
            }
            else
            {
                MessageBox.Show("Test results could not be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // If the result is null, display an error message.
            }
        }


        private void ExitButton_Click(object sender, EventArgs e) // Event handler for the exit button click event.
        {
            Application.Exit(); // Exit the application.
        }

        private void LogOutButton_Click(object sender, EventArgs e) // Event handler for the logout button click event.
        {
            MessageBox.Show("Successfully logged out. Redirecting to login...", "Logged Out", MessageBoxButtons.OK, MessageBoxIcon.Information); // Shows a message indicating a successful logout.
            LoginForm loginForm = new LoginForm(); // Creates and displays a new instance of the LoginForm.
            loginForm.Show();
            this.Hide(); // Hides the current form.
        }
    }
}
