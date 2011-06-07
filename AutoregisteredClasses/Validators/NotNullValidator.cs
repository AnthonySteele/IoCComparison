namespace AutoregisteredClasses.Validators
{
    using AutoregisteredClasses.Interfaces;

    public class NotNullValidator : IValidator
    {
        public bool IsValid(object value)
        {
            return (value  != null);
        }
    }
}
