using bugdgetwarsapi.Database;
using bugdgetwarsapi.DTOs;
using bugdgetwarsapi.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace bugdgetwarsapi.Services.ExpenseQueries;

public static class GetExpense
{
    public record Query() : IRequest<Response>;

    public abstract record Response
    {
        public record Success(List<ExpenseDto> Expenses) : Response;

        public static Success MkSuccess(List<Expense> expenses)
        {
            var mapper = new ExpenseMapper.ExpenseMapper();
            var dtoList = expenses.Select(exp => mapper.ToDto(exp)).ToList();
            return new Success(dtoList);
        }
        
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

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            // Query the entity
            var entities = await _context.Set<Expense>().ToListAsync(cancellationToken);

            if (!entities.Any())
                return Response.MkNotFound(); // or Response.NotFound()

            // Map to DTO
            var dtoList = entities.Select(e => new ExpenseMapper.ExpenseMapper().ToDto(e)).ToList();

            return new Response.Success(dtoList);
        }
    }



}