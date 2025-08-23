using bugdgetwarsapi.Database;
using bugdgetwarsapi.Models;
using MediatR;
namespace bugdgetwarsapi.Services.ExpenseCommands;



public  static class CreateExpense
{
    public record Command : IRequest<Response>
    {
        public string Name { get; set; }
        public double Amount { get; set; }
    }

    public abstract record Response
    {
        public record Success( Expense model): Response;
        
        public static Success MkSuccess(Expense model) => new(model); 
        
        public record UnProcessEntity : Response;

        public static UnProcessEntity MkUnProcessEntity() => new();

        public record UnknownError : Response;

        public static UnknownError MkUnknownError() => new();
        
        
    }

    public class Handler :IRequestHandler<Command, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public  async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var entity = new Expense
            {
                Name = request.Name,
                Amount = request.Amount,
                
            };
            _context.Set<Expense>().Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Response.MkSuccess(entity);
            
            
            
        }
    }
}