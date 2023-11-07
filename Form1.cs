namespace GraphicalCommandInterpreter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string commands = richTextBox1.Text.Trim();
            ExecuteMultiLineCommands(commands);
        }

        private void ExecuteSingleCommand(string command)
        {
            // Process the single command entered in the TextBox
            // Example: Check command and perform actions based on it
            if (command.StartsWith("pen"))
            {
                // Extract parameters, validate, and perform pen action
                // Example: pen red, pen green, etc.
            }
            else if (command.StartsWith("draw"))
            {
                // Extract parameters, validate, and perform draw action
                // Example: draw rectangle, draw circle, draw triangle
            }
            // Implement other commands similarly
        }

        private void ExecuteMultiLineCommands(string commands)
        {
            string[] commandLines = commands.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            // Process each line separately using ExecuteSingleCommand or handle multi-line logic
            foreach (string line in commandLines)
            {
                ExecuteSingleCommand(line);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string command = textBox1.Text.Trim();
                ExecuteSingleCommand(command); // Call a method to handle the single line command
            }
        }
    }
}