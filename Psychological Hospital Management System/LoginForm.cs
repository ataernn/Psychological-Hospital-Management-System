using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Psychological_Hospital_Management_System
{
    public partial class LoginForm : Form
    {
        SQL sqlHelper = new SQL(); // Connect to the sql with SQL.cs class

        public LoginForm() // Constructor method for initializing the form.
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(LoginForm_FormClosing); // Event handler for when the form is closing.
            this.StartPosition = FormStartPosition.CenterScreen; // Center the form on the screen when opened.
            this.MaximizeBox = false; // Disable the maximize button.
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Prevent the form from being resized.
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e) // Event handler to close the application when the form is closed.
        {
            Application.Exit(); // Exit the application
        }

        private void ExitButton_Click(object sender, EventArgs e) // Event handler for the "Exit" button to close the application.
        {
            Application.Exit(); // Exit the application
        }

        private void IDTextBox_KeyPress(object sender, KeyPressEventArgs e) // Event handler to ensure only numeric characters are entered into the ID field.
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) // If the character is not a digit or control (e.g., backspace).
            {
                e.Handled = true; // Prevent the character from being entered.
            }
        }

        private void IDTextBox_TextChanged(object sender, EventArgs e)  // Event handler to validate the ID as the user types.
        {
            if (string.IsNullOrWhiteSpace(IDTextBox.Text)) // Check if the ID field is empty.
            {
                ErrorProvider.SetError(IDTextBox, "ID cannot be empty."); // Show error if the ID is empty.
                return;
            }
            else
            {
                ErrorProvider.Clear(); // Don't show the error if the field is not empty.
            }

            if (IDTextBox.Text.Length != 11)
            {
                ErrorProvider.SetError(IDTextBox, "ID must have at least 11 digits"); // Show error if the ID is not 11 digits long.
            }
            else
            {
                ErrorProvider.Clear(); // Don't show the error if the ID is valid.
            }
        }

        private void LoginButton_Click(object sender, EventArgs e) // Event handler for the "Login" button click.
        {
            if (string.IsNullOrWhiteSpace(IDTextBox.Text)) // Check if the ID field is empty.
            {
                MessageBox.Show("ID cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Show error message.
                IDTextBox.Focus(); // Set focus back to the ID field.
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordTextBox.Text)) // Check if the Password field is empty.
            {
                MessageBox.Show("Password cannot be empty.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Show error message.
                PasswordTextBox.Focus(); // Set focus back to the Password field.
                return;
            }

            string userType, userName, userSurname; // Initialize variables to hold user information.

            if (sqlHelper.CheckUserLogin(IDTextBox.Text, PasswordTextBox.Text, out userType, out userName, out userSurname)) // Call the `CheckUserLogin` method from the SQL class to validate login
            {
                // If the login is successful, check the user type and open the appropriate form.
                if (userType == "doctor") // If the user is a doctor.
                {
                    MessageBox.Show($"Successfully logged in. Welcome Dr. {userName} {userSurname}!", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information);  // Show success message.
                    DoctorForm doctorForm = new DoctorForm(IDTextBox.Text, userName, userSurname); // Create a new instance of the Doctor Form.
                    doctorForm.Show(); // Show Doctor Form.
                }
                else if (userType == "patient") // If the user is a patient.
                {
                    MessageBox.Show($"Successfully logged in. Welcome {userName} {userSurname}!", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information); // Show success message.
                    PatientForm patientForm = new PatientForm(IDTextBox.Text, userName, userSurname); // Create a new instance of the Patient Form.
                    patientForm.Show(); // Show Patient Form.
                }
                else if (userType == "admin") // If the user is an administrator.
                {
                    MessageBox.Show($"Successfully logged in. Welcome Admin {userName} {userSurname}!", "Login Success", MessageBoxButtons.OK, MessageBoxIcon.Information); // Show success message.
                    AdminForm adminForm = new AdminForm(IDTextBox.Text, userName, userSurname); // Create a new instance of the Admin Form.
                    adminForm.Show(); // Show Admin Form.
                }
                this.Hide(); // Hide the login form after this process.
            }
            else
            {
                MessageBox.Show("Invalid Credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Show error message for invalid credentials.
            }
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e) // Event handler to validate the password as the user types.
        {
            if (string.IsNullOrWhiteSpace(PasswordTextBox.Text)) // Check if the password field is empty.
            {
                ErrorProvider.SetError(PasswordTextBox, "Password cannot be empty."); // Show error if the password is empty.
                return;
            }
            else
            {
                ErrorProvider.Clear(); // Don't show the error if the password is not empty.
            }
        }
    }
}