using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace GraphicalCommandInterpreter
{
    // Custom exception class for invalid commands
    public class InvalidCommandException : Exception
    {
        // Invalid command that caused the exception
        public string InvalidCommand { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidCommandException"/> class with the specified invalid command and message.
        /// </summary>
        /// <param name="invalidCommand">The invalid command that caused the exception.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
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
        private Dictionary<string, int> variables = new Dictionary<string, int>();

        /// <summary>
        /// Handles the specified graphical command and performs the corresponding action on the given form.
        /// </summary>
        /// <param name="form">The form on which the graphical commands will be executed.</param>
        /// <param name="command">The graphical command to handle.</param>
        /// <exception cref="InvalidCommandException">Thrown when the command is invalid or contains errors.</exception>
        public void HandleCommand(Form1 form, string command)
        {
            bool executeLinesFlag = true;
            try
            {
                //string conditions = string.Empty;
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

                    if (commandType == "if")
                    {
                        if (parts.Length == 4)
                        {
                            int operand1, operand2;

                            // Check if operands are variables
                            bool isOperand1Variable = variables.TryGetValue(parts[1], out operand1);
                            bool isOperand2Variable = variables.TryGetValue(parts[3], out operand2);

                            // If both operands are variables, update their values
                            if (isOperand1Variable)
                                variables[parts[1]] = operand1;
                            if (isOperand2Variable)
                                variables[parts[3]] = operand2;

                            // Evaluate the if condition based on the operator
                            switch (parts[2])
                            {
                                case "<":
                                    executeLinesFlag = isOperand1Variable ? operand1 < operand2 : int.Parse(parts[1]) < operand2;
                                    break;
                                case ">":
                                    executeLinesFlag = isOperand1Variable ? operand1 > operand2 : int.Parse(parts[1]) > operand2;
                                    break;
                                case "=":
                                    executeLinesFlag = isOperand1Variable ? operand1 == operand2 : int.Parse(parts[1]) == operand2;
                                    break;
                                case ">=":
                                    executeLinesFlag = isOperand1Variable ? operand1 >= operand2 : int.Parse(parts[1]) >= operand2;
                                    break;
                                case "<=":
                                    executeLinesFlag = isOperand1Variable ? operand1 <= operand2 : int.Parse(parts[1]) <= operand2;
                                    break;
                                case "!=":
                                    executeLinesFlag = isOperand1Variable ? operand1 != operand2 : int.Parse(parts[1]) != operand2;
                                    break;
                                default:
                                    // Handle invalid operator
                                    throw new InvalidCommandException(line, "Invalid operator in if statement. Supported operators are: <, >, =, >=, <=, !=");
                            }

                            

                            // Display a message if the condition is not met
                            if (!executeLinesFlag)
                            {
                                MessageBox.Show($"Condition '{line.Substring(3)}' is not met. Skipped the following commands.", "Condition Not Met", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            // Handle invalid if statement
                            throw new InvalidCommandException(line, "Invalid if statement. Usage: if variable operator value");
                        }
                    }
                    else if (commandType == "endif")
                    {
                        // Reset the executeLinesFlag when encountering endif
                        executeLinesFlag = true;
                    }


                    // Check if it's a variable assignment
                    if (parts.Length >= 3 && parts[1] == "=")
                    {
                        HandleVariableAssignment(parts);

                        continue;

                    }
                    else
                    {
                        if (executeLinesFlag)
                            switch (commandType)
                        {
                            case "if":
                                break;
                            case "endif":
                                break;
                            case "moveto":
                                if (parts.Length == 3)
                                {
                                    int x, y;
                                    if (int.TryParse(parts[1], out x) && int.TryParse(parts[2], out y))
                                    {
                                        form.penX = x;
                                        form.penY = y;
                                        form.MoveTo();
                                    }
                                    else if (variables.TryGetValue(parts[1], out x) && variables.TryGetValue(parts[2], out y))
                                    {
                                        form.penX = x;
                                        form.penY = y;
                                        form.MoveTo();
                                    }
                                    else
                                    {
                                        throw new InvalidCommandException(line, "Invalid coordinates for moveto command. Moveto takes two integer coordinates or variable references, e.g., moveto 100,100 or moveto num1, num2");
                                    }
                                }
                                else
                                {
                                    throw new InvalidCommandException(line, "Invalid parameters for moveto command. Moveto takes two integer coordinates or variable references, e.g., moveto 100,100 or moveto num1, num2");
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
                                        form.DrawTo();
                                    }
                                    else if (variables.TryGetValue(parts[1], out x) && variables.TryGetValue(parts[2], out y))
                                    {
                                        form.penX = x;
                                        form.penY = y;
                                        form.MoveTo();
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

                            /// <summary>
                            /// Changes the pen colour
                            /// </summary>
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
                            /// <summary>
                            /// Determines if shape drawn is outlined or filled by pen's colour
                            /// </summary>
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
                                form.Reset();
                                break;

                            case "circle":
                                if (parts.Length == 2)
                                {
                                    int radius;
                                    if (int.TryParse(parts[1], out radius))
                                    {
                                            if (executeLinesFlag)
                                            {
                                                form.DrawCircle(radius);
                                            }
                                        }
                                    else if (variables.TryGetValue(parts[1], out radius))
                                    {
                                            if (executeLinesFlag)
                                            {
                                                form.DrawCircle(radius);
                                            }
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
                                            if (executeLinesFlag)
                                            {
                                                form.DrawRectangle(width, height);
                                            }
                                    }
                                    else if (variables.TryGetValue(parts[1], out width) && variables.TryGetValue(parts[2], out height))
                                    {
                                            if (executeLinesFlag)
                                            {
                                                form.DrawRectangle(width, height);
                                            }
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
                                            if (executeLinesFlag)
                                            {
                                                form.DrawTriangle(adj, @base, hyp);
                                            }
                                        }
                                    else if (variables.TryGetValue(parts[1], out adj) && variables.TryGetValue(parts[2], out @base) && variables.TryGetValue(parts[2], out hyp))
                                    {
                                            if (executeLinesFlag)
                                            {
                                                form.DrawTriangle(adj, @base, hyp);
                                            }
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
                                throw new InvalidCommandException(line, "Valid Commands include: moveto, drawto, pen, fill, clear, reset, circle, rectangle, and triangle. Try one");
                        }
                    }
                }
            }
            catch (InvalidCommandException ex)
            {
                // Display a message to the user
                MessageBox.Show($"Error in command '{ex.InvalidCommand}': {ex.Message}", "Invalid Command.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw ex;
            }
            /*catch (Exception ex)
            {
                // Handle other exceptions if needed
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }*/
        }

        public void HandleVariableAssignment(string[] parts)
        {
            string variable = parts[0];
            int value;

            if (int.TryParse(parts[2], out value))
            {
                variables[variable] = value;
            }
            else
            {
                // Check if it's a variable reference
                if (variables.TryGetValue(parts[2], out int referencedValue))
                {
                    variables[variable] = referencedValue;
                }
                else
                {
                    throw new InvalidCommandException(string.Join(" ", parts), $"Invalid value for variable '{variable}' assignment");
                }
            }
        }


        

   




    }
}