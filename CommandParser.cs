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


        private bool inIfBlock = false;
        private bool ifConditionMet = false;
        private bool skipCommands = false;


        private bool inLoopBlock = false;
        private int loopIterations = 0;
        /// <summary>
        /// Handles the specified graphical command and performs the corresponding action on the given form.
        /// </summary>
        /// <param name="form">The form on which the graphical commands will be executed.</param>
        /// <param name="command">The graphical command to handle.</param>
        /// <exception cref="InvalidCommandException">Thrown when the command is invalid or contains errors.</exception>
        public void HandleCommand(Form1 form, string command)
        {
            try
            {
                if (inIfBlock)
                {
                    if (command.Trim().ToLower() == "endif")
                    {
                        inIfBlock = false;
                    }
                    else if (!ifConditionMet)
                    {
                        return; // Skip commands within a false if block
                    }
                }
                else if (inLoopBlock)
                {
                    // Store commands in the loop block
                    loopBlockCommands.Add(command);

                    if (command.Trim().ToLower() == "endloop")
                    {
                        // Execute the loop block
                        for (int i = 0; i < loopIterations; i++)
                        {
                            foreach (string loopCommand in loopBlockCommands)
                            {
                                HandleCommand(form, loopCommand);
                            }
                        }

                        // Reset loop-related variables
                        inLoopBlock = false;
                        loopIterations = 0;
                        loopBlockCommands.Clear();
                    }
                    return;
                }
                else if (inLoopBlock)
                {
                    if (command.Trim().ToLower() == "endloop")
                    {
                        inLoopBlock = false;
                    }
                    else
                    {
                        // Execute the loop block
                        if (loopIterations > 0)
                        {
                            for (int i = 0; i < loopIterations; i++)
                            {
                                ExecuteCommandBlock(form, command);
                            }
                        }
                        return;
                    }
                }
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

                    // Check if it's a variable assignment
                    if (parts.Length >= 3 && parts[1] == "=")
                    {
                        HandleVariableAssignment(parts);
      
                        continue;

                    }
                    else
                    {
                        switch (commandType)
                        {

                            case "if":
                                if (parts.Length == 2)
                                {
                                    string condition = parts[1];
                                    ifConditionMet = EvaluateCondition(condition);
                                    inIfBlock = true;
                                    skipCommands = !ifConditionMet;  // Set skipCommands based on ifConditionMet
                                }
                                else
                                {
                                    throw new InvalidCommandException(line, "Invalid parameter for if command. if takes a condition, e.g., if count > size");
                                }
                                break;

                            case "loop":
                                if (parts.Length == 2)
                                {
                                    if (int.TryParse(parts[1], out loopIterations))
                                    {
                                        inLoopBlock = true;
                                    }
                                    else
                                    {
                                        throw new InvalidCommandException(line, "Invalid parameter for loop command. loop takes an integer, e.g., loop 5");
                                    }
                                }
                                else
                                {
                                    throw new InvalidCommandException(line, "Invalid parameter for loop command. loop takes an integer, e.g., loop 5");
                                }
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
                                    else if (variables.TryGetValue(parts[1], out width) && variables.TryGetValue(parts[2], out height))
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
                                if (!skipCommands)  // Only execute the command if skipCommands is false
                                {
                                    HandleCommand(form, command);
                                }
                                break;
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


        private List<string> loopBlockCommands = new List<string>();

        private bool EvaluateCondition(string condition)
        {
            // Implement a simple condition parser
            var tokens = condition.Split(' ');

            if (tokens.Length != 3)
            {
                throw new InvalidCommandException(condition, "Invalid condition format");
            }

            string variable = tokens[0];
            string comparisonOperator = tokens[1];
            string valueStr = tokens[2];

            if (!variables.TryGetValue(variable, out int variableValue))
            {
                throw new InvalidCommandException(condition, $"Variable '{variable}' not found");
            }

            if (!int.TryParse(valueStr, out int comparisonValue))
            {
                throw new InvalidCommandException(condition, $"Invalid value '{valueStr}' in the condition");
            }

            switch (comparisonOperator)
            {
                case "==":
                    return variableValue == comparisonValue;
                case "!=":
                    return variableValue != comparisonValue;
                case ">":
                    return variableValue > comparisonValue;
                case "<":
                    return variableValue < comparisonValue;
                case ">=":
                    return variableValue >= comparisonValue;
                case "<=":
                    return variableValue <= comparisonValue;
                default:
                    throw new InvalidCommandException(condition, $"Invalid comparison operator '{comparisonOperator}'");
            }
        }


        private void ExecuteCommandBlock(Form1 form, string commandBlock)
        {
            // Execute the commands in the block (e.g., for loop iterations)
            string[] commands = commandBlock.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string command in commands)
            {
                HandleCommand(form, command);
            }
        }
    }
}
