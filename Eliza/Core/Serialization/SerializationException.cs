using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Eliza.Core.Serialization
{
    class SerializationException : Exception
    {
        public SerializationException() : base() { }

        public SerializationException(String msg) : base(msg) { }

        public SerializationException(String msg, Exception ex) : base(msg, ex) { }
    }

    class UnsupportedAttributeException : SerializationException
    {
        public UnsupportedAttributeException() : base() { }

        public UnsupportedAttributeException(String msg) : base(msg) { }

        public UnsupportedAttributeException(String str, Exception ex) : base(str, ex) { }

        public UnsupportedAttributeException(Attribute attr, FieldInfo fieldInfo) :
            this(String.Format("Unsupported use case of attribute {0} in {1}", attr.GetType().Name, fieldInfo.Name)) { }
    }

}
