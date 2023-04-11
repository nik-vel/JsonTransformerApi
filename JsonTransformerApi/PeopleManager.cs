using AutoMapper;
using JsonTransformerApi.ApiModels;
using JsonTransformerApi.DataAccess;
using JsonTransformerApi.DataModels;

namespace JsonTransformerApi
{
    public class PeopleManager : IPeopleManager
    {
        private readonly IPersonHierarchyBuilder _hierarchyBuilder;
        private readonly IPeopleRepository _peopleRepository;
        private readonly IMapper _mapper;

        public PeopleManager(IPersonHierarchyBuilder hierarchyBuilder, IPeopleRepository peopleRepository, IMapper mapper)
        {
            _hierarchyBuilder = hierarchyBuilder;
            _peopleRepository = peopleRepository;
            _mapper = mapper;
        }

        ///<inheritdoc />
        public async Task<IEnumerable<PersonTreeNode>> ProcessPeople(Person[] persons)
        {
            var result = _hierarchyBuilder.ConvertToHierarchy(persons);

            var dataModel = _mapper.Map<PersonDataList>(result);

            await _peopleRepository.Insert(dataModel);

            return result;
        }
    }
}
