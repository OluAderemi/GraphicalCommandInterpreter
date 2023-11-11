using System;
using System.IO;

namespace GraphicalCommandInterpreter
{
    // Custom exception class for invalid commands
    public class InvalidCommandException : Exception
    {
        // Invalid command that caused the exception
        public string InvalidCommand { get; }

        // Constructor that takes the invalid command and a message
        public InvalidCommandException(string invalidCommand, string message) : base(message)
        {
            InvalidCommand = invalidCommand;
        }
    }

    /// <summary>
    /// Parses and handles graphical commands for the drawing application.
    /// </summary>
    public class CommandParser
    {
        public void HandleCommand(Form1 form, string command)
        {
            try
            {
                string[] commandLines = command.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in commandLines)
                {
                    // Split the line using both spaces and commas
                    string[] parts = line.Split(new[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);


                    if (parts.Length < 1)
                    {
                        // Command not recognized or incomplete
                        continue;
                    }

                    string commandType = parts[0].ToLower();
                    switch (commandType)
                    {
                        case "moveto":
                            if (parts.Length == 3)
                            {
                                int x, y;
                                if (int.TryParse(parts[1], out x) && int.TryParse(parts[2], out y))
                                {
                                    form.penX = x;
                                    form.penY = y;
                                    form.MovePenMarker();
                                }
                                else
                                {
                                    throw new InvalidCommandException(line, "Invalid coordinates for moveto command. Moveto takes two integer coordinates, e.g drawto 100,100");
                                }
                            }
                            else
                            {
                                throw new InvalidCommandException(line, "Invalid parameters for moveto command. Moveto takes two integer coordinates, e.g drawto 100,100");
                            }
                            break;

                        case "drawto":
                            if (parts.Length == 3)
                            {
                                int x, y;
                                if (int.TryParse(parts[1], out x) && int.TryParse(parts[2], out y))
                                {
                                    form.penX = x;
                                    form.penY = y;
                                    form.MoveToButDrawWithNoPenMarker();
                                }
                                else
                                {
                                    throw new InvalidCommandException(line, "Invalid coordinates for drawto command. Drawto takes two integer coordinates, e.g drawto 100,100");
                                }
                            }
                            else
                            {
                                throw new InvalidCommandException(line, "Invalid parameters for drawto command. Drawto takes two integer coordinates, e.g drawto 100,100");
                            }
                            break;

                        case "pen":
                            if (parts.Length == 2)
                            {
                                string color = parts[1].ToLower();
                                switch (color)
                                {
                                    case "red":
                                        form.SetPenColor(Color.Red);
                                        break;
                                    case "green":
                                        form.SetPenColor(Color.Green);
                                        break;
                                    case "black":
                                        form.SetPenColor(Color.Black);
                                        break;
                                    // Add more color options as needed
                                    default:
                                        throw new InvalidCommandException(line, "Invalid colour specified for pen command. try red, green or black");
                                }
                            }
                            else
                            {
                                throw new InvalidCommandException(line, "Invalid parameter for pen command. pen takes a colour, e.g. pen red");
                            }
                            break;

                        case "fill":
                            if (parts.Length == 2)
                            {
                                string fillStatus = parts[1].ToLower();
                                switch (fillStatus)
                                {
                                    case "on":
                                        form.SetFillStatus(true);
                                        break;
                                    case "off":
                                        form.SetFillStatus(false);
                                        break;
                                    // Add more options as needed
                                    default:
                                        throw new InvalidCommandException(line, "Invalid fill status specified for fill command. Fill is either on or off, e.g fill on");
                                }
                            }
                            else
                            {
                                throw new InvalidCommandException(line, "Invalid parameter for fill command. Fill is either on or off, e.g fill on");
                            }
                            break;

                        case "clear":
                            form.ClearDrawingArea();
                            break;

                        case "reset":
                            form.MarkerShow();
                            break;

                        case "circle":
                            if (parts.Length == 2)
                            {
                                int radius;
                                if (int.TryParse(parts[1], out radius))
                                {
                                    form.DrawCircle(radius);
                                }
                                else
                                {
                                    throw new InvalidCommandException(line, "Invalid radius specified for circle command. Circle takes a radius e.g. circle 40");
                                }
                            }
                            else
                            {
                                throw new InvalidCommandException(line, "Invalid parameter for circle command. Circle takes a radius e.g. circle 40");
                            }
                            break;

                        case "rectangle":
                            if (parts.Length == 3)
                            {
                                int width, height;
                                if (int.TryParse(parts[1], out width) && int.TryParse(parts[2], out height))
                                {
                                    form.DrawRectangle(width, height);
                                }
                                else
                                {
                                    throw new InvalidCommandException(line, "Invalid dimensions specified for rectangle command. Rectangle takes width and height, e.g. rectangle 80,95");
                                }
                            }
                            else
                            {
                                throw new InvalidCommandException(line, "Invalid parameters for rectangle command. Rectangle takes width and height, e.g. rectangle 80,95");
                            }
                            break;

                        case "triangle":
                            if (parts.Length == 4)
                            {
                                int adj, @base, hyp;
                                if (int.TryParse(parts[1], out adj) && int.TryParse(parts[2], out @base) && int.TryParse(parts[3], out hyp))
                                {
                                    form.DrawTriangle(adj, @base, hyp);
                                }
                                else
                                {
                                    throw new InvalidCommandException(line, "Invalid dimensions specified for triangle command. Triangle takes adjacent, base and hypotenuse, e.g. triangle 80,90,100");
                                }
                            }
                            else
                            {
                                throw new InvalidCommandException(line, "Invalid parameters for triangle command. Triangle takes adjacent, base and hypotenuse, e.g. triangle 80,90,100");
                            }
                            break;

                        default:
                            throw new InvalidCommandException(line, "Valid Commands include: moveto, drawto, pen, fill, clear, reset, circle, rectangle and triangle. Try one");
                    }
                }
            }
            catch (InvalidCommandException ex)
            {
                // Handle the exception here, e.g., display a message to the user
                MessageBox.Show($"Error in command '{ex.InvalidCommand}': {ex.Message}", "Invalid Command.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle other exceptions if needed
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        }
}
