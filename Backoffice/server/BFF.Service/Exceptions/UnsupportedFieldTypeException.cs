using System;

namespace BFF.Service.Exceptions
{
    internal class UnsupportedFieldTypeException : Exception
    {
        public UnsupportedFieldTypeException(string msg): base(msg)
        {
        }
    }
}