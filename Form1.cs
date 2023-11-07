using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicalCommandInterpreter
{
    public partial class Form1 : Form
    {
        private int penX = 200; // Default X position of the 'pen'
        private int penY = 200; // Default Y position of the 'pen'
        private const int markerSize = 10; // Marker size

        public Form1()
        {
            InitializeComponent();
            //pictureBox1.Paint += PictureBox1_Paint!; // Subscribe to the PictureBox's Paint event
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Redraw all elements here if needed

        }

        private void HandleCommand(string command)
        {
            string[] parts = command.Split(' ');

            if (parts.Length < 1)
            {
                // Command not recognized or incomplete
                return;
            }

            switch (parts[0].ToLower())
            {
                case "moveto":
                    if (parts.Length >= 3)
                    {
                        int x, y;
                        if (int.TryParse(parts[1], out x) && int.TryParse(parts[2], out y))
                        {
                            penX = x; // Update pen's X position
                            penY = y; // Update pen's Y position
                            DrawPenMarker(); // Draw the pen marker after moving

                        }
                    }
                    break;

                case "circle":
                    if (parts.Length >= 2)
                    {
                        int radius;
                        if (int.TryParse(parts[1], out radius))
                        {
                            DrawCircle(radius);
                        }
                    }
                    break;

                // Add cases for other commands as needed

                default:
                    // Command not recognized
                    break;
            }
        }

        private void DrawPenMarker()
        {
            // Clear the PictureBox before drawing the marker
                pictureBox1.Refresh();

                using (Graphics g = pictureBox1.CreateGraphics())
                {
                    // Change the pen marker color to a contrasting one like blue
                    Brush markerBrush = Brushes.Blue;

                    g.FillEllipse(markerBrush, penX, penY, markerSize, markerSize);
                }
            
        }



        private void DrawCircle(int radius)
        {
            int diameter = radius * 2;
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.DrawEllipse(Pens.Black, penX - radius, penY - radius, diameter, diameter);
            }
        }

        // Other drawing methods (DrawRectangle, DrawTriangle) go here...

        private void ExecuteSingleCommand(string command)
        {
            HandleCommand(command);
            // Update PictureBox1 here if required after handling the command
        }

        private void ExecuteMultiLineCommands(string commands)
        {
            string[] commandLines = commands.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in commandLines)
            {
                ExecuteSingleCommand(line);
            }
            // Update PictureBox1 here if required after handling multi-line commands
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string command = textBox1.Text.Trim();
                ExecuteSingleCommand(command);
                textBox1.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string commands = richTextBox1.Text.Trim();
            ExecuteMultiLineCommands(commands);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            pictureBox1.Refresh();
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                // Change the pen marker color to a contrasting one like blue
                Brush markerBrush = Brushes.Blue;

                g.FillEllipse(markerBrush, 0, 0, 10, 10);
            }
            
        }
    }
}
