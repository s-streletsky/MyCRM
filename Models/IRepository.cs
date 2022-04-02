using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Models
{
    internal interface IRepository<T>
    {
        T Add(T item);
        T Get(T item);
        T Update(T item);
        void Delete(T item);
    }
}
