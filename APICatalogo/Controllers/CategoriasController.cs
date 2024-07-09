using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public CategoriasController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetAll()
        {
            var categorias = _context
                             .Categorias
                             .AsNoTracking()
                             .ToList();
            if (categorias is null)
            {
                return NotFound("Categorias não foram encontradas");
            }
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Categoria> GetById(int id)
        {
            var categoria = _context
                            .Categorias
                            .AsNoTracking()
                            .FirstOrDefault(c => c.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound($"Categoria de ID: {id} não encontrada");
            }
            return Ok(categoria);
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            var categoriaProdutos = _context
                                    .Categorias
                                    .Include(p => p.Produtos)
                                    .AsNoTracking()
                                    .ToList();
            if (categoriaProdutos is null)
            {
                return NotFound("Categorias e produtos não encontrados");
            }
            return Ok(categoriaProdutos);
        }

        [HttpPost]
        public ActionResult Create(Categoria categoria)
        {
            if (categoria is null)
            {
                return BadRequest("Objeto Categoria não encontrado no corpo da requisição");
            }

            _context.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
                new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Update(int id, Categoria categoria) {
            if (id != categoria.CategoriaId) {
                return BadRequest("Ids don't match");
            }
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id) { 
            var categoria = _context
                            .Categorias
                            .FirstOrDefault(c => c.CategoriaId == id);
            if (categoria is null) {
                return NotFound($"Categoria com o ID: {id} não encontrada ...");
            }
            _context.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
