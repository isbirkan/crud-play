using CrudPlay.Application.Queries;

using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace CrudPlay.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken) =>
        Ok(await _mediator.Send(new GetTodosQuery(), cancellationToken));
}
