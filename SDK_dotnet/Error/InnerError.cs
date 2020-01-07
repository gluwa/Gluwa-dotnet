namespace Gluwa.Error
{
    public sealed class InnerError<E>
    {
        /// <summary>
        /// Error code. Most likely this will be stringfied from service specific error
        /// </summary>
        public E Code { get; set; }
        /// <summary>
        /// path to the object that contains the error
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// descriptive message
        /// </summary>
        public string Message { get; set; }
    }
}
