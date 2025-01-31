using AutoMapper;

using CrudPlay.Core.DTO;

namespace CrudPlay.Core.Mappers;

public class TodoProfile : Profile
{
    public TodoProfile()
    {
        CreateMap<Entities.Todo, Domain.Todo>();

        CreateMap<CreateTodoRequest, Entities.Todo>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore());

        CreateMap<UpdateTodoRequest, Entities.Todo>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Priority, o => o.Ignore())
                .AfterMap((src, dest) =>
                {
                    if (src.Priority.HasValue) dest.Priority = src.Priority.Value;
                })
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForAllMembers(o => o.Condition((_, _, sourceMember, _) => sourceMember is not null));
    }
}
