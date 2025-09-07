using bugdgetwarsapi.Database;
using bugdgetwarsapi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bugdgetwarsapi.Services.ExpenseQueries;

public static class GetExpense
{
    public record Query() : IRequest<Response>;

    public abstract record Response
    {
        public record Success(List<Expense> Expenses): Response;
        public static Success MkSuccess(List<Expense> Expenses) => new(Expenses);
        
        public record NotFound(): Response;
        
        public static NotFound MkNotFound() => new();
        
    }
    
    public class GetExpenseHandler : IRequestHandler<Query, Response>
    {
        private readonly ApplicationDbContext _context;

        public GetExpenseHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async  Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var entities = await _context.Set<Expense>().ToListAsync(cancellationToken);

            if (!entities.Any())
            {
                return new Response.NotFound();
            }

          
            return new Response.Success(entities);
        }
    }
    
    
    
    
}