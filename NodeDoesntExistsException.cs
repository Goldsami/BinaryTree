using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClassBinaryTree
{
    public class NodeDoesntExistsException : Exception
    {
        public NodeDoesntExistsException()
        {
        }

        public NodeDoesntExistsException(string message) : base(message)
        {
        }

        public NodeDoesntExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NodeDoesntExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
