namespace JsonTransformerApi.ApiModels
{
    /// <summary>
    /// Flat model for person
    /// </summary>
    /// <param name="Id">Id of the person</param>
    /// <param name="Name">Name of the person</param>
    /// <param name="Parent">Optional Id of the person's parent. Null if parent doesn't exists</param>
    public record Person(int Id, string Name, int? Parent);
}
