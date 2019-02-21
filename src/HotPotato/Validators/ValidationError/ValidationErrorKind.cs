
namespace HotPotato.Validators
{
    public enum ValidationErrorKind
    {
        /// <summary>
        /// An unknown error
        /// </summary>
        Unknown,
        /// <summary>
        /// A string is expected
        /// </summary>
        StringExpected,
        /// <summary>
        /// A number is expected
        /// </summary>
        NumberExpected,
        /// <summary>
        /// An integer is expected
        /// </summary>
        IntegerExpected,
        /// <summary>
        /// A boolean is expected
        /// </summary>
        BooleanExpected,
        /// <summary>
        /// An object is expected
        /// </summary>
        ObjectExpected,
        /// <summary>
        /// A property is required but not found
        /// </summary>
        PropertyRequired,
        /// <summary>
        /// An array is expected
        /// </summary>
        ArrayExpected,
        /// <summary>
        /// A null value is expected
        /// </summary>
        NullExpected,
        /// <summary>
        /// The Regex pattern does not match
        /// </summary>
        PatternMismatch,
        /// <summary>
        /// The string is too short
        /// </summary>
        StringTooShort,
        /// <summary>
        /// The string is too long
        /// </summary>
        StringTooLong,
        /// <summary>
        /// The number is too small
        /// </summary>
        NumberTooSmall,
        /// <summary>
        /// The number is too big
        /// </summary>
        NumberTooBig,
        /// <summary>
        /// The integer is too big
        /// </summary>
        IntegerTooBig,
        /// <summary>
        /// The array contains too many items
        /// </summary>
        TooManyItems,
        /// <summary>
        /// The array contains too few items
        /// </summary>
        TooFewItems,
        /// <summary>
        /// The items in the array are not unique
        /// </summary>
        ItemsNotUnique,
        /// <summary>
        /// A date time is expected
        /// </summary>
        DateTimeExpected,
        /// <summary>
        /// A date is expected
        /// </summary>
        DateExpected,
        /// <summary>
        /// A time is expected
        /// </summary>
        TimeExpected,
        /// <summary>
        /// A timespan is expected
        /// </summary>
        TimeSpanExpected,
        /// <summary>
        /// A URI is expected
        /// </summary>
        UriExpected,
        /// <summary>
        /// An IP v4 address is expected
        /// </summary>
        IpV4Expected,
        /// <summary>
        /// An IP v6 address is expected
        /// </summary>
        IpV6Expected,
        /// <summary>
        /// A valid GUID is expected
        /// </summary>
        GuidExpected,
        /// <summary>
        /// The object is not in the given schemas
        /// </summary>
        NotAnyOf,
        /// <summary>
        /// The obeject is not all of the given schemas
        /// </summary>
        NotAllOf,
        /// <summary>
        /// The object is not one of the given schemas
        /// </summary>
        NotOneOf,
        /// <summary>
        /// The object matches the not allowed schema
        /// </summary>
        ExcludedSchemaValidates,
        /// <summary>
        /// The number is not a multiple of the given number
        /// </summary>
        NumberNotMultipleOf,
        /// <summary>
        /// The integer is not a multiple of the given integer
        /// </summary>
        IntegerNotMultipleOf,
        /// <summary>
        /// The vlaue is not one of the allowed enumerations
        /// </summary>
        NotInEnumeration,
        /// <summary>
        /// An Email is expected
        /// </summary>
        EmailExpected,
        /// <summary>
        /// A hostname is expected
        /// </summary>
        HostnameExpected,
        /// <summary>
        /// The array tuple contains too many items
        /// </summary>
        TooManyItemsInTuple,
        /// <summary>
        /// An array item is not valid
        /// </summary>
        ArrayItemNotValid,
        /// <summary>
        /// The item is not valid with the AdditionalItems chema
        /// </summary>
        AdditionalItemNotValid,
        /// <summary>
        /// The additional properties are not valid
        /// </summary>
        AdditionalPropertiesNotValid,
        /// <summary>
        /// Addtional/unspecified properties are not allowed
        /// </summary>
        NoAdditionalPropertiesAllowed,
        /// <summary>
        /// Too many properties exist in the object
        /// </summary>
        TooManyProperties,
        /// <summary>
        /// Too few properties exist in the object.
        /// </summary>
        TooFewProperties,
        /// <summary>
        /// A Base64 string is expected
        /// </summary>
        Base64Expected,
        /// <summary>
        /// No type of the types validates (check error details in <see cref="cref="MultiTypeValidationError"/>).
        /// </summary>
        NoTypeValidates

    }
}
