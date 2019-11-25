using System;

namespace LibraryIS.CommonLayer.Exceptions
{
    public class ResourceConflictException : Exception
    {
        public ResourceConflictException(string message) : base(message) { }
        public ResourceConflictException(string message, Exception ex) : base(message, ex) { }
    }
}
