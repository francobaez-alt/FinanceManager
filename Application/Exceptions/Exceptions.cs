using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string email)
            : base($"User with email '{email}' already exists.")
        {
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message)
            : base(message)
        {
        }
    }

    public class BusinessException : Exception
    {
        public BusinessException(string message)
            : base(message)
        {
        }
    }

}
