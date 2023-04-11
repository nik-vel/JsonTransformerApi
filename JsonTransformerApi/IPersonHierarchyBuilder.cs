using JsonTransformerApi.ApiModels;

namespace JsonTransformerApi
{
    public interface IPersonHierarchyBuilder
    {
        /// <summary>
        /// Transforms an array of flat persons into a hierarchical structure
        /// </summary>
        List<PersonTreeNode> ConvertToHierarchy(Person[] persons);
    }
}