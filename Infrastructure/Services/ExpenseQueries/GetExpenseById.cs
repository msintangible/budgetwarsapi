using bugdgetwarsapi.Database;
using bugdgetwarsapi.DTOs;
using bugdgetwarsapi.Models;
using MediatR;

namespace bugdgetwarsapi.Services.ExpenseQueries;

public static class GetExpenseById
{
    public record Query(int ExpenseId) : IRequest<Response>;

    public abstract record Response
    {
        public static Success MkSuccess(Expense expense)
        {
            var mapper = new ExpenseMapper.ExpenseMapper();
            var dto = mapper.ToDto(expense);
            return new Success(dto);
        }

        public static NotFound MkNotFound()
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

            if (entity is null) return Response.MkNotFound();
            return Response.MkSuccess(entity);
        }
    }
}