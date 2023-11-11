using System;
using System.Drawing;
using System.Windows.Forms;

namespace GraphicalCommandInterpreter
{

    /// <summary>
    /// Represents the main form of the graphical command interpreter.
    /// </summary>
    public partial class Form1 : Form
    {
        public int penX = 0; // Default X position of the 'pen'
        public int penY = 0; // Default Y position of the 'pen'
        public int markerSize = 10; // Marker size
        public Color penColor = Color.Black;
        public bool fillEnabled = false;


        private CommandParser commandParser; // Instance of the CommandParser

        public Form1()
        {
            InitializeComponent();
            this.Load += Form_Load!;
            Reset();
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
        public void SetPenColor(Color color)
        {
            penColor = color;
        }

        public void SetFillStatus(bool status)
        {
            fillEnabled = status;
        }

        /// <summary>
        /// Moves to the specified coordinates on the drawing area.
        /// </summary>
        public void MoveTo()
        {
            pictureBox1.Refresh();
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                Brush markerBrush = Brushes.Blue;
                g.FillEllipse(markerBrush, penX, penY, 10, 10);
            }
        }

        /// <summary>
        /// Sets the coordinates for next location to draw shapes.
        /// </summary>
        public void DrawTo()
        {
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                Brush markerBrush = Brushes.Transparent;
                g.FillEllipse(markerBrush, penX, penY, markerSize, markerSize);
            }
        }

        /// <summary>
        /// Clears the drawing area
        /// </summary>
        public void ClearDrawingArea()
        {
            pictureBox1.Invalidate();
            markerSize = 0;
        }

        public void DrawCircle(int radius)
        {
            int diameter = radius * 2;
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                Pen pen = new Pen(penColor);
                if (fillEnabled)
                {
                    Brush fillBrush = new SolidBrush(penColor);
                    g.FillEllipse(fillBrush, penX - radius, penY - radius, diameter, diameter);
                }
                g.DrawEllipse(pen, penX - radius, penY - radius, diameter, diameter);
            }
        }

        /// <summary>
        /// Draws a rectangle takes two parameters, width and height
        /// </summary>
        public void DrawRectangle(int width, int height)
        {
            int startX = penX - width / 2;
            int startY = penY - height / 2;

            using (Graphics g = pictureBox1.CreateGraphics())
            {
                Pen pen = new Pen(penColor);
                if (fillEnabled)
                {
                    Brush fillBrush = new SolidBrush(penColor);
                    g.FillRectangle(fillBrush, startX, startY, width, height);
                }
                g.DrawRectangle(pen, startX, startY, width, height);
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
                Pen pen = new Pen(penColor);
                if (fillEnabled)
                {
                    Brush fillBrush = new SolidBrush(penColor);
                    Point[] points = { new Point(x1, y1), new Point(x2, y2), new Point(x3, y3) };
                    g.FillPolygon(fillBrush, points);
                }
                g.DrawLine(pen, x1, y1, x2, y2);
                g.DrawLine(pen, x2, y2, x3, y3);
                g.DrawLine(pen, x3, y3, x1, y1);
            }
        }

        /// <summary>
        /// Clears the drawing area and returns the Pen to 0,0
        /// </summary>
        public void Reset()
        {
            penX = 0;
            penY = 0;
            pictureBox1.Refresh();
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                Brush markerBrush = Brushes.Blue;
                g.FillEllipse(markerBrush, 0, 0, 10, 10);
            }
            fillEnabled = false;
            penColor = Color.Black;


        }

        /// <summary>
        /// Executes a single-line command.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        public void ExecuteSingleCommand(string command)
        {
            commandParser.HandleCommand(this, command); // Pass the Form1 instance to the command parser
            // Update PictureBox1 here if required after handling the command
        }

        /// <summary>
        /// Executes a multi-line command.
        /// </summary>
        /// <param name="commands">The multi-line commands to execute.</param>
        public void ExecuteMultiLineCommands(string commands)
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
        /// <summary>
        /// Saves the content of the richTextBox1 to a selected text file.
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            // Show a SaveFileDialog to allow the user to choose where to save the file
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Save Text File";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file name
                    string fileName = saveFileDialog.FileName;

                    try
                    {
                        // Save the content of richTextBox1 to the selected file
                        System.IO.File.WriteAllText(fileName, richTextBox1.Text);
                        MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            richTextBox1.Clear();
        }
        /// <summary>
        /// Loads the content of a selected text file into richTextBox1.
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // Show an OpenFileDialog to allow the user to choose a file to open
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.Title = "Open Text File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Get the selected file name
                    string fileName = openFileDialog.FileName;

                    try
                    {
                        // Load the content of the selected file into richTextBox1
                        richTextBox1.Text = System.IO.File.ReadAllText(fileName);
                        MessageBox.Show("File loaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
