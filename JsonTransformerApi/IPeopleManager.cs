using JsonTransformerApi.ApiModels;

namespace JsonTransformerApi
{
    public interface IPeopleManager
    {
        /// <summary>
        /// Converts an array of flat <see cref="Person"/> into a hierarchical structure, inserts the data into the database,
        /// and returns the resulting hierarchy
        /// </summary>
        /// <param name="persons">The array of persons to convert to a hierarch.</param>
        /// <returns>List of hierarchies of <see cref="PersonTreeNode"/> objects</returns>
        Task<IEnumerable<PersonTreeNode>> ProcessPeople(Person[] persons);
    }
}