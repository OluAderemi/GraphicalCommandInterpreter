using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicalCommandInterpreter
{
    public partial class Form1 : Form
    {
        private int penX = 0; // Default X position of the 'pen'
        private int penY = 0; // Default Y position of the 'pen'
        private const int markerSize = 10; // Marker size
        private bool isMove = false; // Flag to determine if the user has moved the marker

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
                            MovePenMarker(); // Draw the pen marker after moving

                        }
                    }
                    break;

                case "drawto":
                    if (parts.Length >= 3)
                    {
                        int x, y;
                        if (int.TryParse(parts[1], out x) && int.TryParse(parts[2], out y))
                        {
                            penX = x; // Store X coordinate for drawing
                            penY = y; // Store Y coordinate for drawing
                            MoveToButDrawWithNoPenMarker();
                        }
                    }
                    break;

                case "clear":
                    pictureBox1.Invalidate();
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

                case "rectangle":
                    if (parts.Length >= 3)
                    {
                        int width, height;
                        if (int.TryParse(parts[1], out width) && int.TryParse(parts[2], out height))
                        {
                            DrawRectangle(width, height);
                        }
                    }
                    break;

                case "triangle":
                    if (parts.Length >= 7) // Three points (x, y) required to draw a triangle
                    {
                        int x1, y1, x2, y2, x3, y3;
                        if (int.TryParse(parts[1], out x1) && int.TryParse(parts[2], out y1) &&
                            int.TryParse(parts[3], out x2) && int.TryParse(parts[4], out y2) &&
                            int.TryParse(parts[5], out x3) && int.TryParse(parts[6], out y3))
                        {
                            DrawTriangle(x1, y1, x2, y2, x3, y3);
                        }
                    }
                    break;


                // Add cases for other commands as needed

                default:
                    // Command not recognized
                    break;
            }
        }

        private void MovePenMarker() // Used for moveto command, the marker will be shown
        {
            // Clear the PictureBox before drawing the marker
                pictureBox1.Refresh();

                using (Graphics g = pictureBox1.CreateGraphics())
                {
                    // Change the pen marker color to a contrasting one like blue
                    Brush markerBrush = Brushes.Blue;

                    g.FillEllipse(markerBrush, penX, penY, markerSize, markerSize);
                isMove = true;
            }
            
        }

        private void MoveToButDrawWithNoPenMarker() // Used for drawto command, the marker will not be shown
        {
            // Clear the PictureBox before drawing the marker
            //pictureBox1.Refresh();

            using (Graphics g = pictureBox1.CreateGraphics())
            {
                // Change the pen marker color to a contrasting one like blue
                Brush markerBrush = Brushes.Transparent;

                g.FillEllipse(markerBrush, penX, penY, markerSize, markerSize);
                //isMove = true;
            }

        }

        private void ClearDrawingArea()
        {
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                // Clear the PictureBox with the form's background color
                g.Clear(this.BackColor);
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

        private void DrawRectangle(int width, int height)
        {
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.DrawRectangle(Pens.Black, penX, penY, width, height);
            }
        }

        private void DrawTriangle(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            Point[] points = { new Point(x1, y1), new Point(x2, y2), new Point(x3, y3) };

            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.DrawPolygon(Pens.Black, points);
            }
        }


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
            if (isMove == false)
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
}
