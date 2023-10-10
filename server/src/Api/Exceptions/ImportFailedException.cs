using System;

namespace StudyZen.Api.Exceptions
{
    public class ImportFailedException : Exception
    {
        public ImportFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
