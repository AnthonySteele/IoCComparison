namespace AutoregisteredClasses.Services
{
    using AutoregisteredClasses.Interfaces;

    public class CustomerService : ICustomerService
    {
        public string Customers()
        {
            return "some customers";
        }
    }
}
