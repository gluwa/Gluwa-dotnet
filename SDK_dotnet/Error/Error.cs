using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Gluwa.Error
{
    /// <summary>
    /// Gluwa standard internal error class with no InnerError
    /// </summary>
    /// <typeparam name="E">error code enum type. Should implement an extension method ToGlobalErrorCode</typeparam>
    public class Error<E> : IError where E : struct, IConvertible
    {
        public int Code { get; set; }
        public string Message { get; set; }

        /// <summary>
        /// Extra data that is needed to process the error. This can be JSON
        /// </summary>
        public string ExtraData { get; set; }

        public Error()
        {
            Debug.Assert(typeof(E).GetTypeInfo().IsEnum);
        }

        public Error(int code)
            : this()
        {
            Code = code;
        }

        public Error(int code, string message)
            : this()
        {
            Code = code;
            Message = message;
        }
    }

    /// <summary>
    /// Gluwa standard internal error class with inner errors
    /// </summary>
    /// <typeparam name="IE">error code enum type for inner errors</typeparam>
    /// <typeparam name="E">error code enum type. Should implement an extension method ToGlobalErrorCode</typeparam>
    public class Error<E, IE> : Error<E> where E : struct, IConvertible //where IE : struct, IConvertible
    {
        public List<InnerError<IE>> InnerErrors { get; set; }

        public Error()
            : base()
        {
            Debug.Assert(typeof(IE).GetTypeInfo().IsEnum);
        }

        public Error(int code, string message, List<InnerError<IE>> innerErrors)
            : base(code, message)
        {
            InnerErrors = innerErrors;
        }

        public Error(int code, string message, InnerError<IE> innerError)
            : this(code, message, new List<InnerError<IE>>() { innerError })
        {
        }
    }
}
