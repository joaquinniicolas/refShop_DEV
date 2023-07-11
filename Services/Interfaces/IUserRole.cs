namespace refShop_DEV.Services.Interfaces
{
    public interface IUserRole
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Level { get; set; }
    }

}
