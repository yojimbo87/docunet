using System;

namespace Docunet
{
    public class NullFieldException : Exception
    {
        public NullFieldException(string message) : base(message)
        {
        }
    }
}
