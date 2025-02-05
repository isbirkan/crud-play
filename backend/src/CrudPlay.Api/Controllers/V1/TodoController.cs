using Asp.Versioning;

using CrudPlay.Application.Commands;
using CrudPlay.Application.Queries;
using CrudPlay.Core.DTO;
using CrudPlay.Core.Identity;

using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CrudPlay.Api.Controllers.V1;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize(Roles = RoleConstants.User)]
public class TodoController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Route("{id?}")]
    public async Task<IActionResult> GetAsync([FromRoute] string? id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return Ok(await _mediator.Send(new GetTodosQuery(), cancellationToken));
        }

        return Ok(await _mediator.Send(new GetTodoByIdQuery(id), cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetByUserIdAsync([FromQuery] string userId, CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetTodosByUserIdQuery(userId), cancellationToken));

    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] CreateTodoRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new CreateTodoCommand(request), cancellationToken);

        return Ok();
    }

    [HttpPatch]
    [Route("{id}")]
    public async Task<IActionResult> PatchAsync([FromRoute] string id, [FromBody] UpdateTodoRequest request, CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateTodoCommand(id, request), cancellationToken);

        return Ok();
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTodoCommand(id), cancellationToken);

        return Ok();
    }
}
