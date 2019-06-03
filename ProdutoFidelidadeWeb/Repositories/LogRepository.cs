using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdutoFidelidadeWeb.Repositories
{
    public class LogRepository : IRepository<Model.Log>
    {
        private ProdutoFidelidadeContext _context;

        public LogRepository(ProdutoFidelidadeContext context)
        {
            _context = context;
        }
        public IList<Model.Log> List()
        {
            return _context.Logs.ToList();
        }

        public void Save(Model.Log entity)
        {
            _context.Logs.Add(entity);
            _context.SaveChanges();
        }
    }
}
