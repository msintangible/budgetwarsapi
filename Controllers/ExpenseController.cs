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
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _mediator.Send(new GetExpense.Query());

        return response switch
        {
            GetExpense.Response.Success { Expenses: var models } => Ok(models),
            GetExpense.Response.NotFound => NotFound(),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var response = await _mediator.Send(new GetExpenseById.Query(id));

        return response switch
        {
            GetExpenseById.Response.Success  { Expense: var model }  => Ok(model),
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
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateExpense.Command command)
    {
     

        var response = await _mediator.Send(command);

        return response switch
        {
            UpdateExpense.Response.Success { model: var model } => Ok(model),
            UpdateExpense.Response.NotFound => NotFound(),
            _ => UnprocessableEntity()
        };
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _mediator.Send(new DeleteExpense.Command{ Id = id});

        return response switch
        {
            DeleteExpense.Response.Success => NoContent(),
            DeleteExpense.Response.NotFound => NotFound(),
            _ => UnprocessableEntity()
        };
    }
    
    
}