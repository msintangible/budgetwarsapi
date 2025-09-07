using bugdgetwarsapi.Database;
using bugdgetwarsapi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bugdgetwarsapi.Services.ExpenseCommands;

public  static class DeleteExpense
{
    public record Command : IRequest<Response>
    {
        public int Id { get; set; }
    }
    
    public abstract record Response
    {
        public record Success : Response;

        public static Success MkSuccess() => new();

        public record NotFound : Response;

        public static NotFound MkNotFound() => new();

        public record UnknownError : Response;

        public static UnknownError MkUnknownError() => new();
    }

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;
        
        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity =  await _context.Set<Expense>().FirstOrDefaultAsync(x => request.Id == x.Id,cancellationToken);

            if (entity is null)
            {
                return new Response.NotFound();
            }
            _context.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response.Success();
        }
    }
}