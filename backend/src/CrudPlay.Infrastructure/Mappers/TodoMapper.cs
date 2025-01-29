using AutoMapper;

using CrudPlay.Application.Models;
using CrudPlay.Infrastructure.Entities;

namespace CrudPlay.Infrastructure.Mappers;

internal class TodoMapper : Profile
{
    public TodoMapper()
    {
        CreateMap<Todo, TodoItem>();
    }
}
