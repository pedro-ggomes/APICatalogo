using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ProdutosController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> GetAllProducts()
        {
            var produtos = await _context
                                 .Produtos    
                                 .AsNoTracking()
                                 .ToListAsync();
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados");
            }
            return Ok(produtos);
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]
        public async Task<ActionResult<Produto>> GetById(int id)
        {
            var produto = await _context
                                .Produtos
                                .FirstOrDefaultAsync(x => x.ProdutoId == id);
            if (produto == null)
            {
                return NotFound($"Produto com ID {id} não encontrado ...");
            }
            return Ok(produto);
        }

        [HttpPost]
        public ActionResult CreateProduct(Produto produto)
        {
            if (produto == null)
            {
                return BadRequest("Body da requisição não contém um produto");
            }
            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult UpdateProduct(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
            {
                return BadRequest($"ID fornecido ({id}) é diferente do " +
                    $"ID do Objeto produto inserido ({produto.ProdutoId})");
            }
            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult DeleteProduct(int id) { 
            var produto = _context
                          .Produtos
                          .FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null) {
                return NotFound("Produto não encontrado");
            }
            _context.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
