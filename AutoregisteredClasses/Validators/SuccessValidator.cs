namespace AutoregisteredClasses.Validators
{
    using AutoregisteredClasses.Interfaces;

    public class SuccessValidator : IValidator
    {
        public bool IsValid(object value)
        {
            return true;
        }
    }
}
