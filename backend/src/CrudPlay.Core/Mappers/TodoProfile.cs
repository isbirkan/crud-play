using AutoMapper;

using CrudPlay.Core.DTO;

namespace CrudPlay.Core.Mappers;

internal class TodoProfile : Profile
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
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForAllMembers(o => o.Condition((source, destination, sourceMember, destinationMember) =>
                sourceMember != null && !sourceMember.Equals(destinationMember)));
    }
}
