using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class TestTransaction
    {
        public static void Test()
        {
            try
            {
                using (TransactionScope transactionScope = new TransactionScope())
                {
                    CustomerOpsBL.AddNewCustomer(new Customer()
                    {
                        Name = "cock"
                    });

                    EquipmentOpsBL.AddNewEquipment(new Equipment()
                    {
                    });

                    transactionScope.Complete();
                }
            }
            catch
            {
                throw;
            }
        }

        public static int TestSearch()
        {
            var equipmentItem = new EquipmentItem()
            {
                SerialNumber = "S0001",
                Price = 200
            };

            var equipmentItem2 = new EquipmentItem()
            {
                SerialNumber = "S0002",
                Price = 200
            };

            var equipmentItem3 = new EquipmentItem()
            {
                SerialNumber = "S0003",
                Price = 200
            };

            var equipmentItems = new List<EquipmentItem>();
            equipmentItems.Add(equipmentItem3);
            equipmentItems.Add(equipmentItem);
            equipmentItems.Add(equipmentItem2);

            var binarySearch = equipmentItems.BinarySearch(new EquipmentItem(){SerialNumber = "S0005", Price = 100});

            return binarySearch;
        }
    }
}