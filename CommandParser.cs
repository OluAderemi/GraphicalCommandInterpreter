using System;

namespace GraphicalCommandInterpreter
{
    public class CommandParser
    {
        public void HandleCommand(Form1 form, string command)
        {
            string[] commandLines = command.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in commandLines)
            {
                string[] parts = line.Split(' ');

                if (parts.Length < 1)
                {
                    // Command not recognized or incomplete
                    continue;
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
                                form.penX = x; // Update pen's X position
                                form.penY = y; // Update pen's Y position
                                form.MovePenMarker(); // Draw the pen marker after moving
                            }
                        }
                        break;

                    case "drawto":
                        if (parts.Length >= 3)
                        {
                            int x, y;
                            if (int.TryParse(parts[1], out x) && int.TryParse(parts[2], out y))
                            {
                                form.penX = x; // Store X coordinate for drawing
                                form.penY = y; // Store Y coordinate for drawing
                                form.MoveToButDrawWithNoPenMarker();
                            }
                        }
                        break;

                    case "clear":
                        form.ClearDrawingArea();
                        break;

                    case "reset":
                        form.penX = 0;
                        form.penY = 0;
                        form.MarkerShow();
                        break;

                    case "circle":
                        if (parts.Length >= 2)
                        {
                            int radius;
                            if (int.TryParse(parts[1], out radius))
                            {
                                form.DrawCircle(radius);
                            }
                        }
                        break;

                    case "rectangle":
                        if (parts.Length >= 3)
                        {
                            int width, height;
                            if (int.TryParse(parts[1], out width) && int.TryParse(parts[2], out height))
                            {
                                form.DrawRectangle(width, height);
                            }
                        }
                        break;

                    case "triangle":
                        if (parts.Length >= 4)
                        {
                            int adj, @base, hyp; // using '@base' as 'base' is a keyword
                            if (int.TryParse(parts[1], out adj) && int.TryParse(parts[2], out @base) && int.TryParse(parts[3], out hyp))
                            {
                                form.DrawTriangle(adj, @base, hyp);
                            }
                        }
                        break;

                    default:
                        // Command not recognized
                        break;
                }
            }
        }
    }
}
