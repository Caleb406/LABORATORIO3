using LAB3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LAB3.Controllers
{

    public class LibroController : Controller
    {
        private readonly LibreriaContext _context;

        public LibroController(LibreriaContext context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Libro> lista = _context.Libros.Include(c => c.Autors).ToList();
            return View(lista);
        }

        [HttpGet]
        public IActionResult RegistroLibro()
        {
            return View();
        }

        // Método para procesar el formulario de registro de autor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistroLibro(Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        [HttpGet]
        public async Task<IActionResult> EditarLibro(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro); // Aquí estás pasando un objeto de tipo Libro a la vista EditarLibro
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarLibro(int id, Libro libro)
        {
            if (id != libro.CodigoLibro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Actualizar los datos del libro
                    _context.Update(libro);

                    // Actualizar los datos de los autores
                    foreach (var autor in libro.Autors)
                    {
                        _context.Update(autor);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.CodigoLibro))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.CodigoLibro == id);
        }


        [HttpGet]
        public async Task<IActionResult> EliminarLibro(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        [HttpPost, ActionName("EliminarLibro")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminarLibro(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            _context.Libros.Remove(libro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
