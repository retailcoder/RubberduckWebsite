using System;

namespace Rubberduck.Client
{
    public class ApiException : Exception
    {
        public ApiException(Exception inner) 
            : base("An unexpected error has occurred while processing an API response, see inner exception for details.", inner) { }
    }
}
