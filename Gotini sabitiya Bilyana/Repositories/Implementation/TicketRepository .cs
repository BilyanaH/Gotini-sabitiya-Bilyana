using Gotini_sabitiya_Bilyana.Models.Domain;
using Gotini_sabitiya_Bilyana.Models;
using System.Data.Entity;


public class TicketRepository : ITicketRepository
{
    private readonly DatabaseContext _dbContext;

    public TicketRepository(DatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }


    public IEnumerable<Ticket> GetAllByUserId(string userId)
    {
        return _dbContext.Tickets
            .Include(t => t.Event)
            .Where(t => t.UserId == userId)
            .ToList();
    }
    public void Add(Ticket ticket)
    {
        _dbContext.Tickets.Add(ticket);
        _dbContext.SaveChanges();
    }

    public void Update(Ticket ticket)
    {
        _dbContext.Tickets.Update(ticket);
        _dbContext.SaveChanges();
    }

    public void Remove(int ticketId)
    {
        var ticket = GetById(ticketId);
        if (ticket != null)
        {
            _dbContext.Tickets.Remove(ticket);
            _dbContext.SaveChanges();
        }
    }

    public Ticket GetById(int ticketId)
    {
        return _dbContext.Tickets.SingleOrDefault(t => t.Id == ticketId);
    }

    public IEnumerable<Ticket> GetAll()
    {
        return _dbContext.Tickets.Include(t => t.Event);
    }

    public IEnumerable<Ticket> GetByUserName(string username)
    {
        return _dbContext.Tickets.Include(t => t.Event).Where(t => t.User.UserName == username);
    }
}
