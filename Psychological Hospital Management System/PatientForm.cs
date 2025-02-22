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
    public partial class PatientForm : Form
    {
        // Declaring private fields to store user information.
        private string userId;
        private string userName;
        private string userSurname;

        // Constructor for the `PatientForm` class, initializing form with the user's ID, name, and surname.
        public PatientForm(string userId, string userName, string userSurname)
        {
            InitializeComponent();
            // Assigning the constructor parameters to the private fields.
            this.userId = userId;
            this.userName = userName;
            this.userSurname = userSurname;

            this.Text = $"{userName} {userSurname}"; // Setting the form's title to the patient's full name.
            this.ControlBox = false; // Hide the control box.
            this.StartPosition = FormStartPosition.CenterScreen; // Centering the form on the screen when it opens.
            this.MaximizeBox = false; // Disabling the maximize button to prevent resizing.
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Preventing the form from being resizable by using a fixed single border style.
        }

        private void StartButton_Click(object sender, EventArgs e) // Event handler for the Start Button click event to initiate the test form.
        {
            TestForm testForm = new TestForm(userId, userName, userSurname); // Creating a new instance of the Test Form, passing the user ID, name, and surname.
            testForm.Show(); // Showing the Test Form to the user.
            this.Hide(); // Hide the Patient Form.
        }
        
        private void ExitButton_Click(object sender, EventArgs e) // Event handler for the ExitButton click event to close the application.
        {
            Application.Exit(); // Exit the application.
        }
    }
}
