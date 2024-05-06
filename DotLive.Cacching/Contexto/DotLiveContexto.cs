using DotLive.Cacching.Models;
using Microsoft.EntityFrameworkCore;

namespace DotLive.Cacching.Contexto
{
    public class DotLiveContexto : DbContext 
    {
        public DotLiveContexto(DbContextOptions<DotLiveContexto> options) :base(options)
        {}

        public DbSet<Produto> Produtos{ get; set; }
    }
}
