using AutoMapper;
using JsonTransformerApi.ApiModels;
using JsonTransformerApi.DataModels;
using JsonTransformerApi.Mappings;

namespace UnitTests
{
    [TestFixture]
    public class PersonProfileTests
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PersonProfile>();
            });

            _mapper = new Mapper(config);
        }

        [Test]
        public void Mapping_PersonTreeNodeToPersonData_ShouldBeSuccessful()
        {
            // Arrange
            var node = new PersonTreeNode(1, "David");
            node.Childs.Add(new PersonTreeNode(2, "Tom"));

            // Act
            var result = _mapper.Map<PersonTreeNode, PersonData>(node);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("David");
            result.Childs.Should().HaveCount(1);
            result.Childs[0].Should().NotBeNull();
            result.Childs[0].Id.Should().Be(2);
            result.Childs[0].Name.Should().Be("Tom");
            result.Childs[0].Childs.Should().BeEmpty();
        }

        [Test]
        public void Mapping_PersonTreeNodeListToPersonDataList_ShouldBeSuccessful()
        {
            // Arrange
            var nodes = new List<PersonTreeNode>
            {
                new PersonTreeNode(1, "David"),
                new PersonTreeNode(2, "Tom")
            };

            // Act
            var result = _mapper.Map<IEnumerable<PersonTreeNode>, PersonDataList>(nodes);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.Data[0].Should().NotBeNull();
            result.Data[0].Id.Should().Be(1);
            result.Data[0].Name.Should().Be("David");
            result.Data[0].Childs.Should().BeEmpty();
            result.Data[1].Should().NotBeNull();
            result.Data[1].Id.Should().Be(2);
            result.Data[1].Name.Should().Be("Tom");
            result.Data[1].Childs.Should().BeEmpty();
        }
    }
}
