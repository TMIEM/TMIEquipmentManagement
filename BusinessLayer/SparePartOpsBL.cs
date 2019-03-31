using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class SparePartOpsBL
    {
        public static void AddNewSparePart(SparePart sparePart)
        {
            SparePartOpsDAL.AddNewSparePart(sparePart);
        }

        public static List<SparePart> GetAllSpareParts()
        {
            return SparePartOpsDAL.GetAllSpareParts();
        }

        public static SparePart GetSparePartByModel(string sparePartModel)
        {
            try
            {
                return SparePartOpsDAL.GetSparePartByModel(sparePartModel);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateSparePart(SparePart sparePart)
        {
            SparePartOpsDAL.UpdateSparePart(sparePart);
        }
    }
}
