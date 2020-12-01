namespace DbStorage.Interface
{
    public interface IDbEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
