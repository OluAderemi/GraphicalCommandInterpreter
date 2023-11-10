using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicalCommandInterpreter
{
    public partial class Form1 : Form
    {
        public int penX = 0; // Default X position of the 'pen'
        public int penY = 0; // Default Y position of the 'pen'
        public int markerSize = 10; // Marker size

        private CommandParser commandParser; // Instance of the CommandParser

        public Form1()
        {
            InitializeComponent();
            this.Load += Form_Load!;
            MarkerShow();
            pictureBox1.Paint += PictureBox1_Paint!;
            commandParser = new CommandParser(); // Instantiate the CommandParser
        }

        private void Form_Load(object sender, EventArgs e)
        {
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

        public void MovePenMarker()
        {
            pictureBox1.Refresh();
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                Brush markerBrush = Brushes.Blue;
                g.FillEllipse(markerBrush, penX, penY, 10, 10);
            }
        }

        public void MoveToButDrawWithNoPenMarker()
        {
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                Brush markerBrush = Brushes.Transparent;
                g.FillEllipse(markerBrush, penX, penY, markerSize, markerSize);
            }
        }

        public void ClearDrawingArea()
        {
            pictureBox1.Invalidate();
            penX = 0;
            penY = 0;
            markerSize = 0;
        }

        public void DrawCircle(int radius)
        {
            int diameter = radius * 2;
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.DrawEllipse(Pens.Black, penX - radius, penY - radius, diameter, diameter);
            }
        }

        public void DrawRectangle(int width, int height)
        {
            int startX = penX - width / 2;
            int startY = penY - height / 2;

            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.DrawRectangle(Pens.Black, startX, startY, width, height);
            }
        }

        public void DrawTriangle(int adj, int @base, int hyp)
        {
            int x1 = penX;
            int y1 = penY;

            int x2 = penX + adj;
            int y2 = penY;

            int height = (int)Math.Sqrt(hyp * hyp - (@base / 2) * (@base / 2));

            int x3 = penX + @base;
            int y3 = penY - height;

            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.DrawLine(Pens.Black, x1, y1, x2, y2);
                g.DrawLine(Pens.Black, x2, y2, x3, y3);
                g.DrawLine(Pens.Black, x3, y3, x1, y1);
            }
        }

        public void MarkerShow()
        {
            pictureBox1.Refresh();
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                Brush markerBrush = Brushes.Blue;
                g.FillEllipse(markerBrush, 0, 0, 10, 10);
            }
        }

        private void ExecuteSingleCommand(string command)
        {
            commandParser.HandleCommand(this, command); // Pass the Form1 instance to the command parser
            // Update PictureBox1 here if required after handling the command
        }

        private void ExecuteMultiLineCommands(string commands)
        {
            commandParser.HandleCommand(this, commands); // Pass the Form1 instance to the command parser
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
            // Handle PictureBox click event if needed
            // ...
        }
    }
}
