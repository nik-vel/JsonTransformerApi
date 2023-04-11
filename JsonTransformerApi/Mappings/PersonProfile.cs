using AutoMapper;
using JsonTransformerApi.ApiModels;
using JsonTransformerApi.DataModels;

namespace JsonTransformerApi.Mappings
{
    /// <summary>
    /// A mapping profile for converting between <see cref="PersonTreeNode"/> and <see cref="PersonData"/> objects.
    /// </summary>
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<PersonTreeNode, PersonData>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Childs, opt => opt.MapFrom(src => src.Childs));

            CreateMap<IEnumerable<PersonTreeNode>, PersonDataList>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.ToArray()));
        }
    }
}
