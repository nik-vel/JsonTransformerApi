namespace JsonTransformerApi.DataModels
{
    public record PersonData(int Id, string Name, PersonData[] Childs);
}
