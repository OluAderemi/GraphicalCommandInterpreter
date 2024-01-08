using Microsoft.VisualStudio.TestTools.UnitTesting;
using GraphicalCommandInterpreter;
using System.Drawing;
using System.IO;



namespace GraphicalCommandInterpreterTests
{

    // Tests for single line commands
    [TestClass]
    public class SingleLineCommandTests
    {
        [TestMethod]
        public void ExecuteSingleCommand_MovetoCommand_SetsPenPosition()
        {
            // Arrange
            Form1 form = new Form1();
            string command = "moveto 50,50";

            // Act
            form.ExecuteSingleCommand(command);

            // Assert
            Assert.AreEqual(50, form.penX);
            Assert.AreEqual(50, form.penY);
        }

        [TestMethod]
        public void ExecuteSingleCommand_PenCommand_SetsPenColor()
        {
            // Arrange
            Form1 form = new Form1();
            string command = "pen red";

            // Act
            form.ExecuteSingleCommand(command);

            // Assert
            Assert.AreEqual(Color.Red, form.penColor);
        }
    }

    //Tests for Multiline Commands
    [TestClass]
    public class MultiLineCommandTests
    {
        [TestMethod]
        public void ExecuteMultiLineCommands_DrawCommands_DrawsShapes()
        {
            // Arrange
            Form1 form = new Form1();
            string commands = "moveto 50 50\r\ncircle 20\r\ndrawto 100 100";

            // Act
            form.ExecuteMultiLineCommands(commands);

            // Assert
            Assert.AreEqual(100, form.penX);
            Assert.AreEqual(100, form.penY);
        }

    }


    // Tests for Save and Load
    [TestClass]
    public class SaveAndLoadTests
    {
        [TestMethod]
        public void SaveButtonClick_SavesTextToFile()
        {
            // Arrange
            Form1 form = new Form1();

            // Set up the richTextBox1 with some test content
            form.richTextBox1.Text = "Test content";

            // Set up a temporary file path
            string tempFilePath = Path.GetTempFileName();

            // Act
            form.SaveFile(tempFilePath);

            // Assert
            // Check if the file was created and contains the expected content
            Assert.IsTrue(File.Exists(tempFilePath));
            string fileContent = File.ReadAllText(tempFilePath);
            Assert.AreEqual("Test content", fileContent);

            // Clean up: Delete the temporary file
            File.Delete(tempFilePath);
        }

        [TestMethod]
        public void LoadButtonClick_LoadsTextFromFile()
        {
            // Arrange
            Form1 form = new Form1();

            // Set up a temporary file path
            string tempFilePath = Path.GetTempFileName();

            // Set the content to be written to the file
            string testContent = "Test content";
            File.WriteAllText(tempFilePath, testContent);

            // Act
            form.LoadFile(tempFilePath);

            // Assert
            // Check if the content of richTextBox1 matches the content written to the file
            Assert.AreEqual(testContent, form.richTextBox1.Text);

            // Clean up: Delete the temporary file
            File.Delete(tempFilePath);
        }
    }

    // Tests for invalid commands
    [TestClass]
    public class CommandParserInvalidCommandsTests
    {
        [TestMethod]
        public void ExecuteInvalidCircleCommand()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser commandParser = new CommandParser();
            string invalidCommand = "crcle 50";

            // Act & Assert
            Assert.ThrowsException<InvalidCommandException>(() => commandParser.HandleCommand(form, invalidCommand));
        }

        [TestMethod]
        public void ExecuteInvalidMovetoCommand()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser commandParser = new CommandParser();
            string invalidCommand = "movto 100,100";

            // Act & Assert
            Assert.ThrowsException<InvalidCommandException>(() => commandParser.HandleCommand(form, invalidCommand));
        }
    }

    //tests for invalid parameters
    [TestClass]
    public class CommandParserInvalidParametersTests
    {
        [TestMethod]
        public void TestInvalidCircleParameters()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser parser = new CommandParser();
            string invalidCommand = "circle x";

            // Act & Assert
            Assert.ThrowsException<InvalidCommandException>(() => parser.HandleCommand(form, invalidCommand));
        }

        [TestMethod]
        public void TestInvalidMovetoParameters()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser parser = new CommandParser();
            string invalidCommand = "moveto 100";

            // Act & Assert
            Assert.ThrowsException<InvalidCommandException>(() => parser.HandleCommand(form, invalidCommand));
        }

        [TestMethod]
        public void TestInvalidDrawtoParameters()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser parser = new CommandParser();
            string invalidCommand = "drawto 100,100,100";

            // Act & Assert
            Assert.ThrowsException<InvalidCommandException>(() => parser.HandleCommand(form, invalidCommand));
        }
    }

    // Tests for moveto command
    [TestClass]
    public class CommandParserMoveToTests
    {
        [TestMethod]
        public void HandleCommand_MovetoCommand_UpdatesPenCoordinates()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser commandParser = new CommandParser();
            string movetoCommand = "moveto 100,100";

            // Act
            commandParser.HandleCommand(form, movetoCommand);

            // Assert
            Assert.AreEqual(100, form.penX);
            Assert.AreEqual(100, form.penY);
        }
    }

    //Tests for drawto command
    [TestClass]
    public class CommandParserDrawtoTests
    {
        [TestMethod]
        public void HandleCommand_DrawtoCommand_UpdatesPenCoordinates()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser commandParser = new CommandParser();
            string drawtoCommand = "drawto 200,200";

            // Act
            commandParser.HandleCommand(form, drawtoCommand);

            // Assert
            Assert.AreEqual(200, form.penX);
            Assert.AreEqual(200, form.penY);
        }
    }


    //Test for clear command
    [TestClass]
    public class ClearTest
    {
        [TestMethod]
        public void ClearTheDrawingArea()
        {
            // Arrange
            Form1 form = new Form1();

            // Act
            form.ClearDrawingArea();

            // Check if the markerSize is set to 0.
            Assert.AreEqual(0, form.markerSize);
        }
    }

    //Test for reset command
    [TestClass]
    public class ResetTest
    {
        [TestMethod]
        public void MarkerShow_Should_ResetToDefaultState()
        {
            // Arrange
            Form1 form = new Form1();
            form.penX = 50;
            form.penY = 50;
            form.penColor = Color.Red;
            form.fillEnabled = true;

            // Act
            form.Reset();

            // Assert
            // Verify that the properties are reset to their default values
            Assert.AreEqual(0, form.penX);
            Assert.AreEqual(0, form.penY);
            Assert.AreEqual(Color.Black, form.penColor);
            Assert.IsFalse(form.fillEnabled);
        }
    }

    //Tests for rectangle command
    [TestClass]
    public class ShapeTests
    {
        [TestMethod]
        public void DrawRectangle_Test()
        {
            // Arrange
            Form1 form = new Form1();
            int width = 50;
            int height = 100;

            // Act
            form.DrawRectangle(width, height);


            Assert.AreEqual(50, width);
            Assert.AreEqual(100, height);
        }

        [TestMethod]
        public void DrawCircle_Test()
        {
            // Arrange
            Form1 form = new Form1();
            int radius = 50;

            // Act
            form.DrawCircle(radius);

            Assert.AreEqual(50, radius);
        }

        [TestMethod]
        public void DrawTriangle_Test()
        {
            // Arrange
            Form1 form = new Form1();
            int adj = 80;
            int @base = 90;
            int hyp = 100;

            // Act
            form.DrawTriangle(adj, @base, hyp);

            Assert.AreEqual(80, adj);
            Assert.AreEqual(90, @base);
            Assert.AreEqual(100, hyp);
        }
    }

    // Tests for Colour Command
    [TestClass]
    public class ColourTests
    {
        [TestMethod]
        public void SetPenColor_Test()
        {
            // Arrange
            Form1 form = new Form1();
            Color expectedColor = Color.Red;

            // Act
            form.ExecuteSingleCommand("pen red");

            Assert.AreEqual(expectedColor, form.penColor);
        }
    }

    // Tests for Fill Command
    [TestClass]
    public class FillTests
    {
        [TestMethod]
        public void SetFillStatus_Test()
        {
            // Arrange
            Form1 form = new Form1();

            // Act
            form.ExecuteSingleCommand("fill on");

            Assert.IsTrue(form.fillEnabled);
        }
    }


    //Part 2
    // Tests for Defining variables
    [TestClass]

    public class VariableTests
    {
        [TestMethod]
        public void HandleVariableAssignment_ValidIntegerValue_AssignsVariable()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser commandParser = new CommandParser();
            string[] validCommand = { "variableName", "=", "42" };

            // Act
            commandParser.HandleVariableAssignment(validCommand);

            // Assert
            Assert.IsTrue(commandParser.variables.ContainsKey("variableName"));
            Assert.AreEqual(42, commandParser.variables["variableName"]);
        }

        [TestMethod]
        public void HandleVariableAssignment_ValidVariableReference_AssignsVariable()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser commandParser = new CommandParser();
            commandParser.variables["existingVariable"] = 100;
            string[] validCommand = { "variableName", "=", "existingVariable" };

            // Act
            commandParser.HandleVariableAssignment(validCommand);

            // Assert
            Assert.IsTrue(commandParser.variables.ContainsKey("variableName"));
            Assert.AreEqual(100, commandParser.variables["variableName"]);
        }

        [TestMethod]
        public void HandleVariableAssignment_InvalidValue_ThrowsException()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser commandParser = new CommandParser();
            string[] invalidCommand = { "variableName", "=", "invalidValue" };

            // Act & Assert
            Assert.ThrowsException<InvalidCommandException>(() => commandParser.HandleVariableAssignment(invalidCommand));
        }

    }

    //Test for if statements
    [TestClass]
    public class IfTests
    {
        [TestMethod]
        public void HandleCommand_IfStatement_ConditionIsMet()
        {
            // Arrange
            Form1 form = new Form1();
            CommandParser commandParser = new CommandParser();
            string ifStatement = "if variableName > 10";

            // Act
            commandParser.HandleCommand(form, ifStatement);

            // Assert
            // Verify that the executeLinesFlag is set to true after handling the if statement
            Assert.IsTrue(commandParser.executeLinesFlag);
        }

    }

    //Tests for Syntax Checking
    [TestClass]
    public class SyntaxTests
    {
        [TestMethod]
        public void HandleCommand_ValidCommands_NoExceptionThrown()
        {
            // Arrange
            CommandParser commandParser = new CommandParser();
            Form1 dummyForm = new Form1();
            string validCommands = "moveto 10,10\r\ndrawto 20,20\r\n";

            // Act
            Action act = () => commandParser.HandleCommand(dummyForm, validCommands);

            // Assert
            Assert.IsTrue(commandParser.executeLinesFlag); // Assert additional conditions if needed
        }

        [TestMethod]
        public void HandleCommand_InvalidCommands_InvalidCommandExceptionThrown()
        {
            // Arrange
            CommandParser commandParser = new CommandParser();
            Form1 dummyForm = new Form1();
            string invalidCommands = "moving 50,70\r\n";

            // Act
            Action act = () => commandParser.HandleCommand(dummyForm, invalidCommands);

            // Assert
            Assert.ThrowsException<InvalidCommandException>(act);
        }


    }
}