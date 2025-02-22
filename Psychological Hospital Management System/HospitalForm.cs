using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Psychological_Hospital_Management_System
{
    public partial class HospitalForm : Form
    {
        public HospitalForm()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(HospitalForm_FormClosing); // Event handler for when the form is closing.
            this.StartPosition = FormStartPosition.CenterScreen; // Centering the form on the screen when it opens.
            this.MaximizeBox = false; // Disabling the maximize button to prevent resizing.
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Preventing the form from being resizable by using a fixed single border style.
        }

        private void HospitalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit(); // Exit the application.
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm(); // Creating a new instance of the Login Form.

            loginForm.Show(); // Displaying the Login Form to the user.

            this.Hide(); // Hide the Hospital form after this process.
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Exit the application.
        }
    }
}
