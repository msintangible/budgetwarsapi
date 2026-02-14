using bugdgetwarsapi.Database;
using bugdgetwarsapi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bugdgetwarsapi.Services.ExpenseCommands;

public static class DeleteExpense
{
    public record Command : IRequest<Response>
    {
        public int Id { get; set; }
    }

    public abstract record Response
    {
        public static Success MkSuccess()
        {
            return new Success();
        }

        public static NotFound MkNotFound()
        {
            return new NotFound();
        }

        public static UnknownError MkUnknownError()
        {
            return new UnknownError();
        }

        public record Success : Response;

        public record NotFound : Response;

        public record UnknownError : Response;
    }

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;

        public CommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<Expense>().FirstOrDefaultAsync(x => request.Id == x.Id, cancellationToken);

            if (entity is null) return new Response.NotFound();
            _context.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response.Success();
        }
    }
}