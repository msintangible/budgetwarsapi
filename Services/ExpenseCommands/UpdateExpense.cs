using bugdgetwarsapi.Database;
using bugdgetwarsapi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bugdgetwarsapi.Services.ExpenseCommands;

public  static class UpdateExpense
{
    public record Command : IRequest<Response>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
       
    }
    
    public abstract record Response
    {
        public record Success( Expense model): Response;
        
        public static Success MkSuccess(Expense model) => new(model); 
        
        public record NotFound : Response;

        public static NotFound MkUnProcessEntity() => new();

        public record UnknownError : Response;

        public static UnknownError MkUnknownError() => new();
    }

    public class CommandHandler : IRequestHandler<Command, Response>
    {
        private  readonly ApplicationDbContext _context;

        public CommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async  Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
           var entity =  await _context.Set<Expense>().FirstOrDefaultAsync(x => request.Id == x.Id,cancellationToken);

           if (entity == null)
           {
               return new Response.NotFound();
           }
           entity.Amount = request.Amount;
           entity.Name = request.Name;
           
           await _context.SaveChangesAsync(cancellationToken);
           
           return Response.MkSuccess(entity);
           

        }
    }
}