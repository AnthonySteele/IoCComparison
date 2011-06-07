namespace AutoregisteredClasses.Services
{
    using AutoregisteredClasses.Interfaces;

    public class OrderService : IOrderService
    {
        public string Orders()
        {
            return "Here are some orders";
        }
    }
}
