using Gotini_sabitiya_Bilyana.Models;
using Gotini_sabitiya_Bilyana.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Reflection.Emit;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;
using System.Text;

namespace Hotel_Bilyana.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ITicketRepository _ticketRepository;

        public DashboardController(DatabaseContext context, ITicketRepository ticketRepository)
        {
            _context = context;
            _ticketRepository = ticketRepository;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Reserve(int eventId, int quantity)
        {

            var ticket = new Ticket
            {
                EventId = eventId,
                UserId = User.Identity.Name,
                Quantity = quantity
            };

            _ticketRepository.Add(ticket);

            return RedirectToAction("MyTickets");
        }

        [HttpGet]
        public IActionResult MyTickets()
        {

            var tickets = _ticketRepository.GetByUserName(User.Identity.Name);

            return View(tickets);
        }
        public IActionResult Users()
        {
            if (User.IsInRole("admin"))
            {
                var users = _context.ApplicationUsers.ToList();
                var userRoles = _context.UserRoles.ToList();
                var roles = _context.Roles.ToList();

                var userRolesDict = userRoles.ToDictionary(ur => ur.UserId, ur => roles.FirstOrDefault(r => r.Id == ur.RoleId)?.Name);

                ViewBag.UserRoles = userRolesDict;


                //    var users = _context.ApplicationUsers.ToList();
                return View(users);
            }
            else
            {
                return View("NotAuthorized");
            }
        }
        public IActionResult User_Create()
        {
            // ViewBag.ReservationId = new SelectList(_context.ReservationModel, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> User_Create([Bind("FirstName,LastName,UserName")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(User));
            }
            //   ViewBag.ReservationId = new SelectList(_context.ReservationModel, "Id", "Id", client.ReservationId);
            return View(user);
        }

        public IActionResult User_Edit(string id)
        {
            if (User.IsInRole("admin"))
            {
                var user = _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);

                if (user == null)
                {
                    return NotFound();
                }

                var roleId = _context.UserRoles.FirstOrDefault(ur => ur.UserId == id)?.RoleId;
                var roleName = _context.Roles.FirstOrDefault(r => r.Id == roleId)?.Name;

                ViewBag.UserRole = roleName;

                return View(user);
            }
            else
            {
                return View("NotAuthorized");
            }
        }
        [HttpPost]
        public async Task<IActionResult> User_Edit([Bind("Id,SecurityStamp,ConcurrencyStamp,FirstName,LastName,PhoneNumber,UserName,NormalizedUserName,Email,NormalizedEmail,ProfilePicture,EmalCobfirmed,PasswordHash,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AcessFailedCount")] ApplicationUser user)
        {


            if (ModelState.IsValid)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(User));
            }
            return View(user);
        }
        public IActionResult User_Details(string id)
        {
            if (User.IsInRole("admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = _context.ApplicationUsers
                  .FirstOrDefault(m => m.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                var roleId = _context.UserRoles.FirstOrDefault(ur => ur.UserId == id)?.RoleId;
                var roleName = _context.Roles.FirstOrDefault(r => r.Id == roleId)?.Name;

                //user.ProfilePicture = roleName;
                ViewBag.UserRole = roleName;

                return View(user);
            }
            else
            {
                return View("NotAuthorized");
            }
        }
        public async Task<IActionResult> User_Delete(string? id)
        {
            if (User.IsInRole("admin"))
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = await _context.ApplicationUsers
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            else
            {
                return View("NotAuthorized");
            }
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("User_Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> User_DeleteConfirmed(string id)
        {
            if (User.IsInRole("admin"))
            {
                var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
                _context.ApplicationUsers.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Users));
            }
            else
            {
                return View("NotAuthorized");
            }
        }
        // GET: Events
        public async Task<IActionResult> Event()
        {
            return View(await _context.Events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Event_Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Event_Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Event_Create([Bind("Id,Name,Description,EventDate,EventImage")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Event_Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Event_Edit(int id, [Bind("Id,Name,Description,EventDate,EventImage")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Event_Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Event_DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}

