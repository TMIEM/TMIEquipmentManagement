using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class CustomerOpsBL
    {
        public static void AddNewCustomer(Customer customer)
        {
            CustomerOpsDAL.AddNewCustomer(customer);
        }

        public static List<Customer> GetAllCustomers()
        {
            return CustomerOpsDAL.GetAllCustomers();
        }

        public static Customer GetCustomerById(int customerId)
        {
            try
            {
                return CustomerOpsDAL.GetCustomerById(customerId);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateCustomer(Customer customer)
        {
            CustomerOpsDAL.UpdateCustomer(customer);
        }
    }
}
