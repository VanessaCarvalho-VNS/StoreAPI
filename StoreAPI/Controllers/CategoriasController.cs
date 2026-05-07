using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreAPI.Context;
using StoreAPI.Models;

namespace StoreAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("produtos")] // rota para obter categorias com seus produtos

        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos() // IEnumerable lista de categorias
        {
            return _context.Categorias.Include(p => p.Produtos).ToList();
            
        }

        [HttpGet] // rota para obter somente as categorias
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            return _context.Categorias.ToList();
            
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")] // rota para obter categoria por id

        public ActionResult<Categoria> Get(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);
            if (categoria is null)
            {
                return NotFound("Categoria não encontrada...");
            }
            return categoria;
        }

        [HttpPost] // rota para criar nova categoria
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return new CreatedAtRouteResult("ObterCategoria",
            new { id = categoria.CategoriaId }, categoria);
        }

       [HttpPut("{id:int}")] // rota para atualizar categoria por id
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return BadRequest();
            }


            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return Ok(categoria);
        }

        [HttpDelete("{id:int}")] // rota para deletar categoria por id

        public ActionResult Delete(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categoria is null)
            {
                return NotFound("Categoria não encontrada...");
            }

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
            return Ok(categoria);
        }
    }
}
