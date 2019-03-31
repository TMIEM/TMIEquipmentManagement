using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class RecordNotFoundException : SystemException
    {
        public RecordNotFoundException(string message)
        {
            Message = message;
        }

        public string Message { get;} 

    }
}
