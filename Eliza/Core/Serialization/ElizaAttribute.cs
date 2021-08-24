using System;

namespace Eliza.Core.Serialization
{
    abstract class ElizaAttribute : Attribute
    {

        // Attributes that influence the behavior of the (de)-serializer
        // should inherit from this class.
        public abstract class ElizaFlowControlAttribute : ElizaAttribute { }


        [AttributeUsage(AttributeTargets.Field)]
        public class ElizaVersionAttribute : ElizaFlowControlAttribute
        {
            // Annotates fields which are only present in a subset of
            // our models. Required for backwards-compatibility.
            public int From { get; set; } // Inclusive
            // public int Until { get; set; }
        }


        [AttributeUsage(AttributeTargets.Field)]
        public class ElizaSizeAttribute : ElizaFlowControlAttribute
        {
            /// <summary>
            /// This annotation means the array size is always known and fixed,
            /// so we don't read in size information.
            /// </summary>
            public int Fixed { get; set; }

            /// <summary>
            /// This denotes the maximum size for string arrays.
            /// There's little reason to use this for reflection logic.
            /// </summary>
            public int Max { get; set; }

            /// <summary>
            /// This denotes the data type encoding length information in the serialized data.
            /// For example, if it's encoded in a single byte, this is TypeCode.Byte.
            /// The (de)serializer uses a default TypeCode.Int32, so this is for specifying
            // edge cases only.
            /// </summary>
            public TypeCode LengthType { get; set; }
        }

        /// <summary>
        /// Annotates string-type fields that are Uuid objects serialized in
        /// UTF-16 encoding. Used for the stringId field in FurnitureSaveData.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class ElizaUtf16Uuid : ElizaFlowControlAttribute { }


        /// <summary>
        /// Annotates that a fields which have non-default values in edge cases.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class ElizaValueAttribute : ElizaFlowControlAttribute
        {
            /// <summary>
            /// Specifies a non-default value when an empty or otherwise
            /// uninitialized object is serialized into bytes.
            /// Fields annotated with this may be linked to each other. Modifying
            /// just one may have undefined behavior. Maybe use this to show a warning in the UI.
            /// </summary>
            public byte[] Undefined { get; set; }
        }

        /// <summary>
        /// Denotes a list of a type that requires MessagePack serialization support,
        /// because we can't annotate individual items in the list itself.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class ElizaMessagePackListAttribute : ElizaFlowControlAttribute { }

        /// <summary>
        /// Annotates a MessagePack object of unknown type,
        /// so we're just coercing into some base type.
        /// </summary>
        [AttributeUsage(AttributeTargets.Field)]
        public class ElizaMessagePackRawAttribute : ElizaFlowControlAttribute { }
    }
}
