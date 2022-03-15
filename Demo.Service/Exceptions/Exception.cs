using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Exceptions
{
    public class NotAllowSpecialCharaterException : Exception
    {
        public NotAllowSpecialCharaterException(string message) : base(message)
        {
        }       
    }

    public class RegisterException : Exception
    {
        public RegisterException(string message) : base(message)
        {
        }
    }

    public class LoginException : Exception
    {
        public LoginException(string message) : base(message)
        {
        }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message)
        {
        }
    }public class UploadFileException : Exception
    {
        public UploadFileException(string message) : base(message)
        {
        }
    }
}
