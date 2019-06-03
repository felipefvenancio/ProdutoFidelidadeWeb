using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdutoFidelidadeWeb.Repositories
{
    public class ProdutoFidelidadeContext_Init
    {
        private ProdutoFidelidadeContext _context;

        public ProdutoFidelidadeContext_Init(ProdutoFidelidadeContext context)
        {
            _context = context;
        }
        public void SeedData()
        {
            if (_context.Configs.Any())
                return;

            var configs = new List<Model.Config>
            {
                new Model.Config{ Name = "IntegrationKey", Value="4B335B6F-9C4D-47F7-B798-C46FFBC4881A" },
                new Model.Config{ Name = "MallCode", Value="1" }

            };

            configs.ForEach(s => _context.Configs.Add(s));
            _context.SaveChanges();


        }

        private void AddNewConfig(Model.Config config)
        {
            var existingType = _context.Configs.FirstOrDefault(p => p.Name == config.Name);
            if (existingType == null)
            {
                _context.Configs.Add(config);
            }
        }
    }
}
