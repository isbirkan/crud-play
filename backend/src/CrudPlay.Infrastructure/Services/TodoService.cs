
using AutoMapper;

using CrudPlay.Application.Interfaces;
using CrudPlay.Application.Models;
using CrudPlay.Infrastructure.Entities;
using CrudPlay.Infrastructure.Interfaces;

namespace CrudPlay.Infrastructure.Services;

internal class TodoService : ITodoService
{
    private readonly IRepository<Todo> _repository;
    private readonly IMapper _mapper;

    public TodoService(IRepository<Todo> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        var todos = await _repository.GetAllAsync(cancellationToken);

        return _mapper.Map<IEnumerable<TodoItem>>(todos);
    }
}
