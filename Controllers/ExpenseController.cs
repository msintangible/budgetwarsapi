using bugdgetwarsapi.Models;
using bugdgetwarsapi.Services.ExpenseCommands;
using bugdgetwarsapi.Services.ExpenseQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace bugdgetwarsapi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ExpenseController : Controller
{
    private readonly IMediator _mediator;

    public ExpenseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var response = await _mediator.Send(new GetExpenseById.Query(id));

        return response switch
        {
            GetExpenseById.Response.Success s => Ok(s.Expense),
            GetExpenseById.Response.NotFound => NotFound(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateExpense.Command command)
    {
        var response = await _mediator.Send(command);

        return response switch
        {
            CreateExpense.Response.Success { model: var model } 
                => CreatedAtAction(nameof(GetById), new { id = model.Id }, model),

            _ => UnprocessableEntity()
        };
    }
}