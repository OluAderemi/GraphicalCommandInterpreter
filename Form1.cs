using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicalCommandInterpreter
{
    public partial class Form1 : Form
    {
        private int penX = 0; // Default X position of the 'pen'
        private int penY = 0; // Default Y position of the 'pen'
        private int markerSize = 10; // Marker size
        

        public Form1()
        {
            InitializeComponent();
            this.Load += Form_Load!;
            MarkerShow();
            //pictureBox1.Paint += PictureBox1_Paint!; // Subscribe to the PictureBox's Paint event
        }

        private void Form_Load(object sender, EventArgs e)
        {
            // Subscribe to the PictureBox's Paint event
            pictureBox1.Paint += PictureBox1_Paint!;
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            // Create a pen for drawing
            using (SolidBrush brush = new SolidBrush(Color.Blue))
            {
                // Define the position and size of the brush
                int brushSize = markerSize;
                int x = penX; // X coordinate
                int y = penY; // Y coordinate

                // Draw a filled ellipse to represent the brush
                e.Graphics.FillEllipse(brush, x, y, brushSize, brushSize);
            }
        }

        private void HandleCommand(string command)
        {
            string[] parts = command.Split(' ');

            if (parts.Length < 1)
            {
                // Command not recognized or incomplete
                return;
            }
            string commandType = parts[0].ToLower();
            switch (commandType)
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

                    ClearDrawingArea();
                    break;

                case "reset":
                    penX = 0;
                    penY = 0;
                    MarkerShow();
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
                    if (parts.Length >= 4)
                    {
                        int adj, @base, hyp; // using '@base' as 'base' is a keyword
                        if (int.TryParse(parts[1], out adj) && int.TryParse(parts[2], out @base) && int.TryParse(parts[3], out hyp))
                        {
                            DrawTriangle(adj, @base, hyp);
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

                    g.FillEllipse(markerBrush, penX, penY, 10, 10);
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
            }

        }

        private void ClearDrawingArea()
        {
            pictureBox1.Invalidate();
            penX = 0;
            penY = 0;
            markerSize = 0;
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
            // Calculate the starting X and Y coordinates to draw the rectangle from its center
            int startX = penX - width / 2;
            int startY = penY - height / 2;

            // Draw the rectangle using the calculated coordinates
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.DrawRectangle(Pens.Black, startX, startY, width, height);
            }
        }


        private void DrawTriangle(int adj, int @base, int hyp)
        {
            // Calculate the coordinates for the vertices of the triangle
            int x1 = penX;
            int y1 = penY;

            int x2 = penX + adj;
            int y2 = penY;

            // Calculate the height based on the hypotenuse and base
            int height = (int)Math.Sqrt(hyp * hyp - (@base / 2) * (@base / 2));

            int x3 = penX + @base;
            int y3 = penY - height;

            // Draw the triangle using the calculated coordinates
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.DrawLine(Pens.Black, x1, y1, x2, y2);
                g.DrawLine(Pens.Black, x2, y2, x3, y3);
                g.DrawLine(Pens.Black, x3, y3, x1, y1);
            }
        }


        private void MarkerShow()
        {

                pictureBox1.Refresh();
                using (Graphics g = pictureBox1.CreateGraphics())
                {
                    // Change the pen marker color to a contrasting one like blue
                    Brush markerBrush = Brushes.Blue;

                    g.FillEllipse(markerBrush, 0, 0, 10, 10);
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
            /*if (isMove == false)
            {
                pictureBox1.Refresh();
                using (Graphics g = pictureBox1.CreateGraphics())
                {
                    // Change the pen marker color to a contrasting one like blue
                    Brush markerBrush = Brushes.Blue;

                    g.FillEllipse(markerBrush, 0, 0, 10, 10);
                }
            }
            */
            
        }
    }
}
