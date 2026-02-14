using bugdgetwarsapi.DTOs;
using bugdgetwarsapi.Models;

namespace bugdgetwarsapi.Services.ExpenseMapper;

public class ExpenseMapper
{
    public ExpenseDto ToDto(Expense expense)
    {
        return new ExpenseDto
        {
            Id = expense.Id,
            Name = expense.Name,
            Amount = expense.Amount
        };
    }
}