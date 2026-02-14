using bugdgetwarsapi.Database;
using bugdgetwarsapi.DTOs;
using bugdgetwarsapi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bugdgetwarsapi.Services.ExpenseCommands;

public static class UpdateExpense
{
    public record Command : IRequest<Response>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Amount { get; set; }
    }

    public abstract record Response
    {
        public static Success MkSuccess(Expense expense)
        {
            var mapper = new ExpenseMapper.ExpenseMapper();
            var dto = mapper.ToDto(expense);
            return new Success(dto);
        }

        public static NotFound MkUnProcessEntity()
        {
            return new NotFound();
        }

        public static UnknownError MkUnknownError()
        {
            return new UnknownError();
        }

        public record Success(ExpenseDto model) : Response;

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

            if (entity == null) return new Response.NotFound();
            entity.Amount = request.Amount;
            entity.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return Response.MkSuccess(entity);
        }
    }
}