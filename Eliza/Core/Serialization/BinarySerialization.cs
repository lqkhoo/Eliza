using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Eliza.Core.Serialization
{

    class BinarySerialization
    {
        public readonly Stream BaseStream;

        protected const int UNKNOWN_LENGTH = 0;
        // In the serialized stream, the default type of the number
        // representing the length of the array that follows.
        protected const TypeCode DEFAULT_LENGTH_TYPECODE = TypeCode.Int32;

        public const int HEADER_LENGTH_NBYTES = 0x20;
        public const int FOOTER_LENGTH_NBYTES = 0x10;


        public BinarySerialization(Stream baseStream)
        {
            this.BaseStream = baseStream;
        }

        protected static bool IsList(Type type)
        {
            return typeof(IList).IsAssignableFrom(type);
        }

        protected static bool IsDictionary(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        // This returns all public fields of a class as a lazy enumerable,
        // starting from the base object. They're not ordered in the alphabetical sense.
        protected static IEnumerable<FieldInfo> GetFieldsOrdered(Type objectType)
        {

            Stack<Type> typeStack = new();

            do {
                typeStack.Push(objectType);
                objectType = objectType.BaseType;
            } while (objectType != null);

            while (typeStack.Count > 0) {
                objectType = typeStack.Pop();
                foreach (FieldInfo fieldInfo in objectType.GetFields()) {
                    // yield return fieldInfo;
                    if (! fieldInfo.IsDefined(typeof(CompilerGeneratedAttribute))) {
                        yield return fieldInfo;
                    }
                    
                }
            }

        }

    }
}
