namespace SDK_dotnet.Error
{
    public sealed class Result<T, E> where E : IError
    {
        public System.Guid ID { get; } = System.Guid.NewGuid();
        public bool IsSuccess { get; set; }
        public bool IsFailure
        {
            get { return !IsSuccess; }
        }
        public T Data { get; set; }
        public E Error { get; set; }
    }
}
