using MySql.Data.MySqlClient;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Psychological_Hospital_Management_System
{
    public partial class AdminForm : Form
    {
        private string userId;
        private string userName;
        private string userSurname;

        private SQL sqlHelper = new SQL();

        public AdminForm(string userId, string userName, string userSurname)
        {
            InitializeComponent();
            this.userId = userId;
            this.userName = userName;
            this.userSurname = userSurname;
            this.Text = $"Admin: {userName} {userSurname}";
            this.FormClosing += new FormClosingEventHandler(AdminForm_FormClosing);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void ValidatePasswords()
        {
            if (PasswordTextBox.Text != RepeatPasswordTextBox.Text)
            {
                ErrorProvider.SetError(RepeatPasswordTextBox, "Passwords do not match.");
            }
            else
            {
                ErrorProvider.Clear();
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            string id = IDTextBox.Text;
            string name = NameTextBox.Text;
            string surname = SurnameTextBox.Text;
            string password = PasswordTextBox.Text;
            string repeatPassword = RepeatPasswordTextBox.Text;
            string userType = UserTypeComboBox.SelectedItem.ToString();

            if (password != repeatPassword)
            {
                ErrorProvider.SetError(RepeatPasswordTextBox, "Passwords do not match.");
                return;
            }
            else
            {
                ErrorProvider.Clear();
            }

            if (string.IsNullOrWhiteSpace(IDTextBox.Text) ||
            string.IsNullOrWhiteSpace(NameTextBox.Text) ||
            string.IsNullOrWhiteSpace(SurnameTextBox.Text) ||
            string.IsNullOrWhiteSpace(PasswordTextBox.Text) ||
            string.IsNullOrWhiteSpace(RepeatPasswordTextBox.Text))
            {
                MessageBox.Show("All fields must be filled.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (UserTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Select User Type", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != repeatPassword)
            {
                MessageBox.Show("Passwords do not match.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (id.Length != 11)
            {
                MessageBox.Show("ID must have at least 11 digits.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool isSuccess = sqlHelper.AddUser(id, name, surname, password, userType);

            if (isSuccess)
            {
                MessageBox.Show("User registration succeeded.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                IDTextBox.Clear();
                NameTextBox.Clear();
                SurnameTextBox.Clear();
                PasswordTextBox.Clear();
                RepeatPasswordTextBox.Clear();
                UserTypeComboBox.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("User registration failed.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void IDTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void IDTextBox_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(IDTextBox.Text))
            {
                ErrorProvider.SetError(IDTextBox, "ID cannot be empty.");
                return;
            }
            else
            {
                ErrorProvider.Clear();
            }

            if (IDTextBox.Text.Length != 11)
            {
                ErrorProvider.SetError(IDTextBox, "ID must have at least 11 digits");
            }
            else
            {
                ErrorProvider.Clear();
            }
        }

        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidatePasswords();
        }

        private void RepeatPasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidatePasswords();
        }

        private void LogOutButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Successfully logged out. Redirecting to login...", "Logged Out", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Hide();
        }
    }
}
