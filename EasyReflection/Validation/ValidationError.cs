namespace EasyReflection.Validation
{
    public class ValidationError
    {
        public string Message { get; private set; }
        public string Name { get; private set; }

        public ValidationError(string Name, string Message)
        {
            this.Message = Message;
            this.Name = Name;
        }
    }
}