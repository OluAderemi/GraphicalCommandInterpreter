using System.Runtime.Serialization;

namespace GraphicalCommandInterpreter
{
    [Serializable]
    internal class FileOperationException : Exception
    {
        public FileOperationException()
        {
        }

        public FileOperationException(string? message) : base(message)
        {
        }

        public FileOperationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected FileOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}