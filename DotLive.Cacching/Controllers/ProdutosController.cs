using DotLive.Cacching.Contexto;
using DotLive.Cacching.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace DotLive.Cacching.Controllers
{
    [ApiController]
    [Route("api/produtos/")]
    public class ProdutosController : ControllerBase
    {
        private const string KEY_CHACHE_PROODUTOS = "produtos";
        private  string KEY_CHACHE_PTODUTO_ID = "produto_id_";

        private readonly DotLiveContexto _contexto;
        private readonly IMemoryCache _cache;

        
        public ProdutosController(DotLiveContexto contexto, IMemoryCache cache)
        {
            _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        [HttpPost]
        public async Task<IActionResult> PostAsync(Produto produto)
        {
            try
            {
                if (produto == null)
                    return BadRequest();

                await _contexto.Produtos.AddAsync(produto);
                await _contexto.SaveChangesAsync();

                return CreatedAtAction(nameof(GetByIdAsync), new { id = produto.Id }, produto);
            }
            catch
            {
                return BadRequest("Erro inesperado.");
            }
          
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            #region SEM CACHE
            //var produtos = await _contexto.Produtos.ToListAsync();

            //return Ok(produtos);
            #endregion

            #region COM CACHE

            List<Produto>? produtos = new List<Produto>();

            //verifica se não exite no cache, antes de consultar no banco de dados
            if (!_cache.TryGetValue(KEY_CHACHE_PROODUTOS, out produtos))
            {
                //Consulta no banco de dados
                produtos = await _contexto.Produtos.ToListAsync();

                 //Salva no cache.
                _cache.Set(KEY_CHACHE_PROODUTOS, produtos);
            }

            //Retorna .
            return Ok(produtos);

            #endregion
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            #region SEM CACHE
            //if (id == 0)
            //    return BadRequest();

            //var produto = await   _contexto.Produtos.SingleOrDefaultAsync(p => p.Id == id);

            //if (produto == null)
            //    return NotFound();


            //return Ok(produto);

            #endregion


            #region COM CACHE

            try
            {
                // Constrói a chave de cache para o ID específico
                KEY_CHACHE_PTODUTO_ID += id;

                if (id == 0)
                    return BadRequest();

                // Pega no cache caso exista ou cria uma key caso ela nao exista e salva no cache
                var produto = await _cache.GetOrCreateAsync(KEY_CHACHE_PTODUTO_ID,
                    async p =>
                    {
                        return await _contexto.Produtos.SingleOrDefaultAsync(p => p.Id == id);
                    });


                if (produto == null)
                    return NotFound();


                return Ok(produto);
            }
            catch
            {
                return BadRequest("Erro inesperado.");
            }
            
            #endregion
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(int id, Produto produto)
        {
            #region SEM CACHE
            //if (id == 0 || produto == null)
            //    return BadRequest();

            //var update = await _contexto.Produtos.AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);

            //if (update == null)
            //    return NotFound();


            // _contexto.Produtos.Update(produto);
            //await _contexto.SaveChangesAsync();

            //return NoContent();
            #endregion

            #region COM CACHE
            try
            {
                if (id == 0 || produto == null)
                    return BadRequest();

                var update = await _contexto.Produtos.AsNoTracking().SingleOrDefaultAsync(p => p.Id == id);

                if (update == null)
                    return NotFound();


                _contexto.Produtos.Update(produto);
                await _contexto.SaveChangesAsync();

                //Toda vez que tiver uma atualização, atualizar a cache
                KEY_CHACHE_PTODUTO_ID += id;
                _cache.Set(KEY_CHACHE_PTODUTO_ID, produto);

                return NoContent();
            }
            catch 
            {
                return BadRequest("Erro inesperado.");
            }
           

            #endregion
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();

                var produto = await _contexto.Produtos.SingleOrDefaultAsync(p => p.Id == id);

                if (produto == null)
                    return NotFound();

                _contexto.Remove(produto);
                _contexto.SaveChanges();

                return NoContent();
            }
            catch
            {
                return BadRequest("Erro inesperado.");
            }
           
        }

    }
}
