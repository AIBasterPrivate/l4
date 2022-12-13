using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using l4.Data;
using l4.Models;

namespace l4.Controllers
{
    public class BookOutOfStoragesController : Controller
    {
        private readonly DbConnection _context;

        public BookOutOfStoragesController(DbConnection context)
        {
            _context = context;
        }

        // GET: BookOutOfStorages
        public async Task<IActionResult> Index()
        {
            var dbConnection = _context.BookOutOfStorages.Include(b => b.Book).Include(b => b.Person);
            return View(await dbConnection.ToListAsync());
        }

        // GET: BookOutOfStorages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BookOutOfStorages == null)
            {
                return NotFound();
            }

            var bookOutOfStorage = await _context.BookOutOfStorages
                .Include(b => b.Book)
                .Include(b => b.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookOutOfStorage == null)
            {
                return NotFound();
            }

            return View(bookOutOfStorage);
        }

        // GET: BookOutOfStorages/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id");
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id");
            return View();
        }

        // POST: BookOutOfStorages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*[Bind("PersonId,BookId,BookTakeDate,BookReturnDate")]*/ BookOutOfStorage bookOutOfStorage)
        {
            bookOutOfStorage.Person = await _context.People.FindAsync(bookOutOfStorage.PersonId);
            bookOutOfStorage.Book = await _context.Books.FindAsync(bookOutOfStorage.BookId);

            _context.Add(bookOutOfStorage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", bookOutOfStorage.BookId);
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id", bookOutOfStorage.PersonId);
            return View(bookOutOfStorage);
        }

        // GET: BookOutOfStorages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BookOutOfStorages == null)
            {
                return NotFound();
            }

            var bookOutOfStorage = await _context.BookOutOfStorages.FindAsync(id);
            if (bookOutOfStorage == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", bookOutOfStorage.BookId);
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id", bookOutOfStorage.PersonId);
            return View(bookOutOfStorage);
        }

        // POST: BookOutOfStorages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BookOutOfStorage bookOutOfStorage)
        {
            if (id != bookOutOfStorage.Id)
            {
                return NotFound();
            }

            bookOutOfStorage.Person = await _context.People.FindAsync(bookOutOfStorage.PersonId);
            bookOutOfStorage.Book = await _context.Books.FindAsync(bookOutOfStorage.BookId);
            try
            {
                _context.Update(bookOutOfStorage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookOutOfStorageExists(bookOutOfStorage.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Id", bookOutOfStorage.BookId);
            ViewData["PersonId"] = new SelectList(_context.People, "Id", "Id", bookOutOfStorage.PersonId);
            return View(bookOutOfStorage);
        }

        // GET: BookOutOfStorages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BookOutOfStorages == null)
            {
                return NotFound();
            }

            var bookOutOfStorage = await _context.BookOutOfStorages
                .Include(b => b.Book)
                .Include(b => b.Person)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookOutOfStorage == null)
            {
                return NotFound();
            }

            return View(bookOutOfStorage);
        }

        // POST: BookOutOfStorages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BookOutOfStorages == null)
            {
                return Problem("Entity set 'DbConnection.BookOutOfStorages'  is null.");
            }
            var bookOutOfStorage = await _context.BookOutOfStorages.FindAsync(id);
            if (bookOutOfStorage != null)
            {
                _context.BookOutOfStorages.Remove(bookOutOfStorage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookOutOfStorageExists(int id)
        {
            return _context.BookOutOfStorages.Any(e => e.Id == id);
        }
    }
}
