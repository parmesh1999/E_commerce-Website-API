using SSECSAPI.Models;

namespace SSECSAPI.Data.Interface
{
    public interface ICustomer
    {
        public List<Customer> GetAllCustomer();
        public Customer GetCustomerById(int id);
        public string AddCustomer(Customer customer);
        public string UpdateCustomer(Customer customer);
        public string DeleteCustomer(int id);
    }
}
