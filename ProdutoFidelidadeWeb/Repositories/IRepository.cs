using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdutoFidelidadeWeb.Repositories
{
    interface IRepository<T>
    {
        IList<T> List();
        void Save(T entity);
    }
}
