using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace CASSharp.Core.Sintaxis
{
    class STException : Exception
    {
        public int Position { get; set; }

        public STException(int? argPosition = null)
        {
            if (argPosition.HasValue)
                Position = argPosition.Value;
        }

        public STException(string message, int? argPosition = null) : base(message)
        {
            if (argPosition.HasValue)
                Position = argPosition.Value;
        }

        public STException(string message, Exception innerException, int? argPosition = null) : base(message, innerException)
        {
            if (argPosition.HasValue)
                Position = argPosition.Value;
        }

        protected STException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
