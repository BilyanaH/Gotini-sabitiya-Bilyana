public interface ITicketRepository
{
    void Add(Ticket ticket);
    void Update(Ticket ticket);
    void Remove(int ticketId);
    Ticket GetById(int ticketId);
    IEnumerable<Ticket> GetAll();
    IEnumerable<Ticket> GetByUserName(string userName);
}
