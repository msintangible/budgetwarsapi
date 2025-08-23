using bugdgetwarsapi.Database;
using bugdgetwarsapi.Models;
using MediatR;

namespace bugdgetwarsapi.Services.ExpenseQueries;

public  static class GetExpenseById
{
    public record Query(int ExpenseId)  : IRequest<Response>;
    
    public abstract record Response {
        
        public record Success(Expense Expense) : Response;
        public static Success MkSuccess(Expense expense) => new(expense);
        
        public record NotFound : Response;

        public static NotFound MkNotFound() => new();

        public record UnknownError : Response;

        public static UnknownError MkUnknownError() => new();
        
        
    }
    
    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;

        public Handler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = _context.Set<Expense>().FirstOrDefault(e => e.Id == request.ExpenseId);

            if (entity is null)
            {
                return Response.MkNotFound();
            }
            return Response.MkSuccess(entity);
        }
    }
}