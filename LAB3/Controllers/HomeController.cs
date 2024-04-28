using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LAB3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LAB3.Controllers
{
    public class HomeController : Controller
    {
        private readonly LibreriaContext _context;

        public HomeController(LibreriaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Autor> lista = _context.Autors.Include(c => c.oLibro).ToList();
            return View(lista);
        }

        // Método para mostrar el formulario de registro de autor
        [HttpGet]
        public IActionResult RegistroAutor()
        {
            return View();
        }

        // Método para procesar el formulario de registro de autor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistroAutor(Autor autor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        [HttpGet]
        public async Task<IActionResult> EditarAutor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // Método para procesar el formulario de edición de autor
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarAutor(int id, Autor autor)
        {
            if (id != autor.IdAutor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.IdAutor))
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
            return View(autor);
        }

        private bool AutorExists(int id)
        {
            return _context.Autors.Any(e => e.IdAutor == id);
        }


        [HttpGet]
        public async Task<IActionResult> EliminarAutor(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.Autors.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminarAutor(int id)
        {
            try
            {
                // Verificar si el autor con el ID proporcionado existe en la base de datos
                var autor = await _context.Autors.FindAsync(id);
                if (autor == null)
                {
                    return NotFound();
                }

                // Eliminar el autor de la base de datos
                _context.Autors.Remove(autor);
                await _context.SaveChangesAsync();

                // Redirigir a la acción Index después de eliminar exitosamente al autor
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción que pueda ocurrir durante el proceso de eliminación
                // Aquí puedes agregar el código para registrar el error o manejarlo de alguna otra manera
                Console.WriteLine($"Error al eliminar el autor: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }

        }
    }
}
