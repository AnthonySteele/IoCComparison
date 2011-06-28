namespace AutoregisteredClasses.Services
{
    using AutoregisteredClasses.Interfaces;

    public class BusinessProcess
    {
        private readonly ICustomerService customerService;
        private readonly IOrderService orderService;

        public BusinessProcess(ICustomerService customerService, IOrderService orderService)
        {
            this.customerService = customerService;
            this.orderService = orderService;
        }

        public IOrderService OrderService
        {
            get { return this.orderService; }
        }

        public ICustomerService CustomerService
        {
            get { return this.customerService; }
        }
    }
}
