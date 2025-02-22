using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Psychological_Hospital_Management_System
{
    public partial class TestForm : Form
    {
        // Declaring private fields to store user information and a SQL helper object.
        private string userId;
        private string userName;
        private string userSurname;
        private SQL sqlHelper = new SQL(); // SQL helper for database operations.

        private string[] questions = new string[] // Array containing all the questions to be displayed in the test form.
        {
            "1. I notice that I am drawn to things that are not difficult or interesting at school, at home, or at work.",
            "2. I find it difficult to read written materials unless they are very interesting or very easy.",
            "3. I struggle to focus on what is said, especially in group settings.",
            "4. I get excited quickly and calm down just as quickly.",
            "5. I generally feel restless, and little things irritate me.",
            "6. I often deny things I said without thinking.",
            "7. I make quick decisions without considering the negative outcomes.",
            "8. My relationships with others suffer because of my tendency to speak first and think later.",
            "9. My mood is filled with ups and downs.",
            "10. I find it difficult to plan to achieve multiple tasks or goals.",
            "11. I get disappointed easily.",
            "12. I am a fragile person, and many things easily hurt me.",
            "13. I always feel like I am about to go somewhere.",
            "14. I feel more comfortable moving than sitting still.",
            "15. I often start answering questions before the entire question is asked.",
            "16. I usually work on multiple projects at the same time and struggle to complete many of them.",
            "17. I have a lot of obsessions in my mind.",
            "18. Even when I sit quietly, I move my feet or hands.",
            "19. I have trouble waiting for my turn in group activities.",
            "20. My mind fills up with too many unnecessary things, causing it to lose functionality.",
            "21. My thoughts clash in my mind like balls bouncing off walls.",
            "22. My brain is like a TV screen showing multiple channels at once.",
            "23. I can't prevent daydreaming during the day.",
            "24. I experience stress due to the disorganized way my brain works."
        };

        public TestForm(string userId, string userName, string userSurname) // Constructor for the `TestForm` class, initializing the form with the user's ID, name, and surname.
        {
            InitializeComponent();
            // Assigning the constructor parameters to the private fields.
            this.userId = userId;
            this.userName = userName;
            this.userSurname = userSurname;

            this.ControlBox = false; // Hide the control box.
            this.StartPosition = FormStartPosition.CenterScreen; // Centering the form on the screen when it opens.
            this.MaximizeBox = false; // Disabling the maximize button to prevent resizing.
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Preventing the form from being resizable by using a fixed single border style.

            LoadQuestions();
        }

        private void LoadQuestions() // Method to dynamically load and display the questions with answer options.
        {
            Panel questionPanel = new Panel(); // Creating a panel to contain the questions and radio buttons.
            questionPanel.Size = new System.Drawing.Size(660, 600); // Setting panel size.
            questionPanel.Location = new System.Drawing.Point(10, 10); // Positioning the panel.
            questionPanel.AutoScroll = true; // Enabling scrolling to handle multiple questions

            for (int i = 0; i < questions.Length; i++) // Looping through each question in the array.
            {
                GroupBox questionGroup = new GroupBox(); // Creating a group box for each question to hold the answer options.
                questionGroup.Text = questions[i]; // Setting the question text as the title.
                questionGroup.Size = new System.Drawing.Size(630, 100); // Setting the size of each group box.
                questionGroup.Location = new System.Drawing.Point(10, i * 130); // Positioning the group boxes vertically.

                // Creating radio buttons for each answer option.
                RadioButton option1 = new RadioButton() { Text = "Never", Location = new System.Drawing.Point(10, 20) };
                RadioButton option2 = new RadioButton() { Text = "Rarely", Location = new System.Drawing.Point(10, 40) };
                RadioButton option3 = new RadioButton() { Text = "Sometimes", Location = new System.Drawing.Point(150, 20) };
                RadioButton option4 = new RadioButton() { Text = "Usually", Location = new System.Drawing.Point(150, 40) };
                RadioButton option5 = new RadioButton() { Text = "Frequently", Location = new System.Drawing.Point(290, 20) };
                RadioButton option6 = new RadioButton() { Text = "Always", Location = new System.Drawing.Point(290, 40) };

                // Adding the radio buttons to the group box.
                questionGroup.Controls.Add(option1);
                questionGroup.Controls.Add(option2);
                questionGroup.Controls.Add(option3);
                questionGroup.Controls.Add(option4);
                questionGroup.Controls.Add(option5);
                questionGroup.Controls.Add(option6);

                questionPanel.Controls.Add(questionGroup); // Adding the group box to the question panel.
            }

            this.Controls.Add(questionPanel); // Adding the panel to the form's controls.

            Button submitButton = new Button(); // Creating a submit button for the form.
            submitButton.Text = "Submit"; // Setting the button text.
            submitButton.Location = new System.Drawing.Point(300, 630); // Positioning the button.
            submitButton.Click += SubmitButton_Click; // Attaching the click event handler to the submit button.
            this.Controls.Add(submitButton); // Adding the button to the form.
        }

        private void SubmitButton_Click(object sender, EventArgs e) // Event handler for when the Submit button is clicked.
        {
            DialogResult confirmResult = MessageBox.Show("Are you sure you want to submit the test?", "Confirm Submission", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Displaying a confirmation dialog to ensure the user wants to submit the test.

            if (confirmResult == DialogResult.No) // If the user chooses not to submit, continue the test.
            {
                return;
            }

            int totalScore = 0; // Variable to store the total score.

            foreach (Control control in this.Controls) // Looping through all the controls in the form.
            {
                if (control is Panel panel) // If the control is a panel (contains questions).
                {
                    foreach (Control questionGroup in panel.Controls) // Looping through each question group inside the panel.
                    {
                        if (questionGroup is GroupBox groupBox) // If the control is a group box (contains options).
                        {
                            RadioButton selectedOption = groupBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked); // Finding the selected radio button (answer) in the group box.
                            if (selectedOption == null) // If no option is selected, show a warning message.
                            {
                                MessageBox.Show($"Please select an option for the question '{groupBox.Text}'.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            int score = CalculateScoreForAnswer(selectedOption.Text); // Calculating the score based on the selected answer.
                            totalScore += score; // Adding the score to the total score.
                        }
                    }
                }
            }

            bool isSuccess = sqlHelper.SaveTestResults(userName, userSurname, totalScore); // Saving the test results to the database.

            string scoreMessage = GetScoreMessage(totalScore); // Retrieving a message based on the total score.

            if (isSuccess) // If the results were successfully saved.
            {
                DialogResult result = MessageBox.Show($"Test results were successfully submitted.\n\nTotal Score: {totalScore}\n\n{scoreMessage}\n\nClick OK to exit.", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information); // Displaying the results and score to the user.

                if (result == DialogResult.OK) // If the user clicks OK, exit the application.
                {
                    Application.Exit();
                }
            }
            else // If there was an error saving the results.
            {
                DialogResult result = MessageBox.Show("An error occurred while submitting test results. Would you like to try again?", "Critical Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error); // Prompting the user to retry or cancel.

                if (result == DialogResult.Cancel) // If the user chooses to cancel, exit the application.
                {
                    Application.Exit();
                }
            }
        }

        private string GetScoreMessage(int totalScore) // Method to return a message based on the total score.
        {
            if (totalScore >= 0 && totalScore <= 24) // Defining score ranges and corresponding messages.
            {
                return "No Symptoms.";
            }
            else if (totalScore >= 25 && totalScore <= 60)
            {
                return "Low Level Symptoms of Attention Deficit and Hyperactivity Disorder.";
            }
            else if (totalScore >= 61 && totalScore <= 96)
            {
                return "Moderate Level Symptoms of Attention Deficit and Hyperactivity Disorder.";
            }
            else if (totalScore >= 97 && totalScore <= 120)
            {
                return "High Level Symptoms of Attention Deficit and Hyperactivity Disorder.";
            }
            else
            {
                return "Invalid score."; // Default case for invalid scores.
            }
        }

        private int CalculateScoreForAnswer(string answer) // Method to calculate the score for a given answer.
        {
            switch (answer) // Assigning scores based on the selected answer.
            {
                case "Never": return 0;
                case "Rarely": return 1;
                case "Sometimes": return 2;
                case "Usually": return 3;
                case "Frequently": return 4;
                case "Always": return 5;
                default: return 0;
            }
        }

        private void ExitButton_Click(object sender, EventArgs e) // Event handler for the Exit button (if it exists).
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit without saving?", "Exit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Displaying a confirmation dialog to ensure the user wants to exit without saving.

            if (result == DialogResult.Yes) // If the user confirms, exit the application.
            {
                Application.Exit();
            }
        }
    }
}