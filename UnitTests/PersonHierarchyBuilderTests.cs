using JsonTransformerApi;
using JsonTransformerApi.ApiModels;
using System.Text.Json;

namespace UnitTests
{
    [TestFixture]
    public class PersonHierarchyBuilderTests
    {
        private IPersonHierarchyBuilder _hierarchyBuilder;

        private JsonSerializerOptions _serializerOptions;

        [SetUp]
        public void SetUp()
        {
            _hierarchyBuilder = new PersonHierarchyBuilder();

            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        [Test]
        public void ConvertToHierarchy_WithEmptyInput_ReturnsEmptyList()
        {
            // Arrange
            var input = Array.Empty<Person>();
            var expected = new List<PersonTreeNode>();

            // Act
            var result = _hierarchyBuilder.ConvertToHierarchy(input);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ConvertToHierarchy_WithFlatList_ReturnsTree()
        {
            // Arrange
            var inputJson = @"[
                            {""id"":1,""name"":""david"",""parent"":null},
                            {""id"":2,""name"":""tom"",""parent"":1},
                            {""id"":3,""name"":""tami"",""parent"":1},
                            {""id"":4,""name"":""gal"",""parent"":1},
                            {""id"":5,""name"":""uri"",""parent"":4},
                            {""id"":6,""name"":""uri"",""parent"":3},
                            {""id"":7,""name"":""adi"",""parent"":5}
                         ]";
            var input = JsonSerializer.Deserialize<Person[]>(inputJson, _serializerOptions);

            var expectedJson = @"[
                                {""id"":1,""name"":""david"",""childs"":[
                                    {""id"":2,""name"":""tom"",""childs"":[]},
                                    {""id"":3,""name"":""tami"",""childs"":[
                                        {""id"":6,""name"":""uri"",""childs"":[]}
                                    ]},
                                    {""id"":4,""name"":""gal"",""childs"":[
                                        {""id"":5,""name"":""uri"",""childs"":[
                                            {""id"":7,""name"":""adi"",""childs"":[]}
                                        ]}
                                    ]}
                                ]}
                            ]";
            var expected = JsonSerializer.Deserialize<List<PersonTreeNode>>(expectedJson, _serializerOptions);

            // Act
            var result = _hierarchyBuilder.ConvertToHierarchy(input);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ConvertToHierarchy_WithLostChildren_ReturnsTreeWithExtraRoots()
        {
            // Arrange
            var inputJson = @"[
                            {""id"":1,""name"":""david"",""parent"":null},
                            {""id"":2,""name"":""tom"",""parent"":1},
                            {""id"":7,""name"":""LostChildOne"",""parent"":5},
                            {""id"":8,""name"":""LostChildTwo"",""parent"":5}
                         ]";
            var input = JsonSerializer.Deserialize<Person[]>(inputJson, _serializerOptions);

            var expectedJson = @"[
                                {""id"":1,""name"":""david"",""childs"":[
                                    {""id"":2,""name"":""tom"",""childs"":[]}
                                ]},
                                {""id"":7,""name"":""LostChildOne"",""childs"":[]},
                                {""id"":8,""name"":""LostChildTwo"",""childs"":[]}
                            ]";
            var expected = JsonSerializer.Deserialize<List<PersonTreeNode>>(expectedJson, _serializerOptions);

            // Act
            var result = _hierarchyBuilder.ConvertToHierarchy(input);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ConvertToHierarchy_WithMultiplePersonsWithoutParent_ReturnsTreeWithExtraRoots()
        {
            // Arrange
            var inputJson = @"[
                            {""id"":1,""name"":""david"",""parent"":null},
                            {""id"":2,""name"":""tom"",""parent"":1},
                            {""id"":3,""name"":""tami"",""parent"":1},
                            {""id"":4,""name"":""gal"",""parent"":null},
                            {""id"":5,""name"":""uri"",""parent"":4},
                            {""id"":6,""name"":""uri"",""parent"":3},
                            {""id"":7,""name"":""adi"",""parent"":5}
                         ]";
            var input = JsonSerializer.Deserialize<Person[]>(inputJson, _serializerOptions);

            var expectedJson = @"[
                                {""id"":1,""name"":""david"",""childs"":[
                                    {""id"":2,""name"":""tom"",""childs"":[]},
                                    {""id"":3,""name"":""tami"",""childs"":[
                                        {""id"":6,""name"":""uri"",""childs"":[]}
                                    ]}
                                ]},
                                {""id"":4,""name"":""gal"",""childs"":[
                                    {""id"":5,""name"":""uri"",""childs"":[
                                        {""id"":7,""name"":""adi"",""childs"":[]}
                                    ]}
                                ]}
                            ]";
            var expected = JsonSerializer.Deserialize<List<PersonTreeNode>>(expectedJson, _serializerOptions);

            // Act
            var result = _hierarchyBuilder.ConvertToHierarchy(input);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }


        [Test]
        public void ConvertToHierarchy_WithIdEquelsParent_ReturnsTreeWithExtraRoots()
        {
            // Arrange
            var inputJson = @"[
                            {""id"":1,""name"":""david"",""parent"":null},
                            {""id"":2,""name"":""tom"",""parent"":1},
                            {""id"":3,""name"":""tami"",""parent"":3}
                         ]";
            var input = JsonSerializer.Deserialize<Person[]>(inputJson, _serializerOptions);

            var expectedJson = @"[
                                {""id"":1,""name"":""david"",""childs"":[
                                    {""id"":2,""name"":""tom"",""childs"":[]}
                                ]},
                                {""id"":3,""name"":""tami"",""childs"":[]}
                            ]";
            var expected = JsonSerializer.Deserialize<List<PersonTreeNode>>(expectedJson, _serializerOptions);

            // Act
            var result = _hierarchyBuilder.ConvertToHierarchy(input);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}