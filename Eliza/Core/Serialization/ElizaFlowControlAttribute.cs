using System;
using Eliza.Model;

namespace Eliza.Core.Serialization
{
    // Seal leaf-level attributes
    // https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/attributes

    public abstract class ElizaAttribute : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class ElizaFlowControlAttribute : ElizaAttribute
    {

        // Inclusive.
        public int FromVersion { get; set; } = 0;

        public SaveData.LOCALE Locale { get; set; } = SaveData.LOCALE.ANY;


        // Unfortunately lambdas are not allowed as Attribute properties.
        /// <summary>
        /// This is for branching on locale / version-specific differences.
        /// Pass in a lambda (locale, version) => bool
        /// </summary>
        // public Func<SaveData.LOCALE, int, bool> Version { get; set; } = null;

    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class ElizaListAttribute : ElizaFlowControlAttribute
    {
        public const int UNKNOWN_SIZE = 0;
        public const TypeCode DEFAULT_LENGTH_TYPECODE = TypeCode.Int32;
        public const bool DEFAULT_ISMESSAGEPACK_LIST = false;

        /// <summary>
        /// Denotes the data type encoding length information in the serialized data.
        /// The (de)serializer defaults to TypeCode.Int32, so specify edge cases only.
        /// </summary>
        public TypeCode LengthType { get; set; } = DEFAULT_LENGTH_TYPECODE;

        /// <summary>
        /// This annotation means the array size is always known and fixed,
        /// and we DON'T read in size information.
        /// </summary>
        public int FixedSize { get; set; } = UNKNOWN_SIZE;

        /// <summary>
        /// This annotation means the array size is always known and fixed,
        /// however, we still ALWAYS read in how many elements are filled.
        /// </summary>
        public int MaxSize { get; set; } = UNKNOWN_SIZE;

        /// <summary>
        /// Denotes that this is a list or array of MessagePack objects.
        /// </summary>
        public bool IsMessagePackList { get; set; } = DEFAULT_ISMESSAGEPACK_LIST;
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class ElizaStringAttribute : ElizaFlowControlAttribute
    {
        public const int UNKNOWN_SIZE = 0;
        public const TypeCode DEFAULT_LENGTH_TYPECODE = TypeCode.Int32;
        public const bool DEFAULT_IS_UTF16_UUID = false;

        public TypeCode LengthType { get; set; } = DEFAULT_LENGTH_TYPECODE;

        /// <summary>
        /// Strings annotated with MaxSize always serialize to number of bytes
        /// equal to MaxSize, but they still have a field indicating the length
        /// of the actual string within.
        /// </summary>
        public int MaxSize { get; set; } = UNKNOWN_SIZE;

        /// <summary>
        /// Annotates string-type fields that are Uuid objects serialized in
        /// UTF-16 encoding. Used for the stringId field in FurnitureSaveData.
        /// </summary>
        public bool IsUtf16Uuid { get; set; } = DEFAULT_IS_UTF16_UUID;

    }

    // Doesn't influence our logic at the moment
    /// <summary>
    /// Annotates that a fields which have non-default values in edge cases.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class ElizaValueAttribute : ElizaAttribute
    {
        /// <summary>
        /// Specifies a non-default value when an empty or otherwise
        /// uninitialized object is serialized into bytes.
        /// Fields annotated with this may be linked to each other. Modifying
        /// just one may have undefined behavior. Maybe use this to show a warning in the UI.
        /// </summary>
        public byte[] Undefined { get; set; }
    }

}
