using System;

namespace Docunet
{
    public class NonExistingFieldException : Exception
    {
        public NonExistingFieldException(string message) : base(message)
        {
        }
    }
}
