using System;
using System.Threading;
using System.Windows.Forms;

namespace GraphicalCommandInterpreter
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Subscribe to the ThreadException event
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            // Set the unhandled exception mode to force the application to use the ThreadException event handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }

        // Event handler for ThreadException
        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
        }
    }
}
