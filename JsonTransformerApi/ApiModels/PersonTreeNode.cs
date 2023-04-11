namespace JsonTransformerApi.ApiModels
{
    /// <summary>
    /// Hierarchical model for person
    /// </summary>
    /// <param name="Id">Id of the person</param>
    /// <param name="Name">Name of the person</param>
    public record PersonTreeNode(int Id, string Name)
    {
        /// <summary>
        /// List of persons children. Empty if children don't exist
        /// </summary>
        public List<PersonTreeNode> Childs { get; set; } = new();
    }
}
