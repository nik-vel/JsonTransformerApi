using JsonTransformerApi.DataModels;

namespace JsonTransformerApi.DataAccess
{
    public interface IPeopleRepository
    {
        Task Insert(PersonDataList personDataList);
    }
}