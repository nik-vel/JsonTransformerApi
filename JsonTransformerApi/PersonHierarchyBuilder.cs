using JsonTransformerApi.ApiModels;

namespace JsonTransformerApi
{
    public class PersonHierarchyBuilder : IPersonHierarchyBuilder
    {
        /// <inheritdoc />
        public List<PersonTreeNode> ConvertToHierarchy(Person[] persons)
        {
            if (persons == null || persons.Length == 0)
                return new List<PersonTreeNode>();

            var rootPersons = new List<Person>();

            var personsByParent = new Dictionary<int, List<Person>>();

            foreach (var person in persons)
            {
                //if person doesnt have parent or patent for himself it;s a root element
                if (person.Parent != null && person.Id != person.Parent)
                {
                    if (!personsByParent.ContainsKey(person.Parent.Value))
                    {
                        personsByParent[person.Parent.Value] = new List<Person>();
                    }
                    personsByParent[person.Parent.Value].Add(person);
                }
                else
                {
                    rootPersons.Add(person);
                }
            }

            var result = new List<PersonTreeNode>();
            foreach (var root in rootPersons)
            {
                result.Add(BuildNode(root, personsByParent));
            }

            //If there are any lost children (children who have a parent id, but the parent wasn't in the list),
            //  each of them can become a root element.
            if (personsByParent.Count != 0)
            {
                foreach (var group in personsByParent)
                {
                    result.AddRange(group.Value.Select(p => BuildNode(p, personsByParent)));
                }
            }

            return result;
        }

        /// <summary>
        /// Recursively build PersonTreeNode object for the Person object using dictionary of Persons grouped by parent Id
        /// </summary>
        /// <param name="node">The Person object to build a PersonTreeNode</param>
        /// <param name="personsByParent">A dictionary of Persons grouped by their parent Id</param>
        /// <returns>Builded PersonTreeNode object</returns>
        private static PersonTreeNode BuildNode(Person node, Dictionary<int, List<Person>> personsByParent)
        {
            var childNodes = personsByParent.GetValueOrDefault(node.Id);

            if (childNodes == null)
            {
                return new PersonTreeNode(node.Id, node.Name) { Childs = new() };
            }

            var outputChildNodes = new List<PersonTreeNode>();

            foreach (var childNode in childNodes)
            {
                outputChildNodes.Add(BuildNode(childNode, personsByParent));
            }

            personsByParent.Remove(node.Id); // Remove processed parents to detect whether there are any children left
                                             //    without a parent after all processing is completed.

            return new PersonTreeNode(node.Id, node.Name) { Childs = outputChildNodes };
        }
    }
}
