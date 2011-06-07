namespace AutoregisteredClasses.Validators
{
    using AutoregisteredClasses.Interfaces;

    public class FailValidator : IValidator
    {
        public bool IsValid(object value)
        {
            return false;
        }
    }
}
