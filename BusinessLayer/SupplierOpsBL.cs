using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class SupplierOpsBL
    {
        public static void AddNewSupplier(Supplier supplier)
        {
            SupplierOpsDAL.AddNewSupplier(supplier);
        }

        public static List<Supplier> GetAllSuppliers()
        {
            return SupplierOpsDAL.GetAllSuppliers();
        }

        public static Supplier GetSupplierById(int supplierId)
        {
            try
            {
                return SupplierOpsDAL.GetSupplierById(supplierId);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateSupplier(Supplier supplier)
        {
            SupplierOpsDAL.UpdateSupplier(supplier);
        }
    }
}
