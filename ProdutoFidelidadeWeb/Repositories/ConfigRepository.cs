using ProdutoFidelidadeWeb.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdutoFidelidadeWeb.Repositories
{
    public class ConfigRepository : IRepository<Model.Config>
    {
        private ProdutoFidelidadeContext _context;

        public ConfigRepository(ProdutoFidelidadeContext context)
        {
            _context = context;
        }
        public IList<Config> List()
        {
            return _context.Configs.ToList();
        }

        public void Save(Config entity)
        {
            var result = _context.Configs.SingleOrDefault(b => b.Name == entity.Name);
            if (result != null)
            {
                result.Value = entity.Value;
                _context.SaveChanges();
            }
        }
    }
}
