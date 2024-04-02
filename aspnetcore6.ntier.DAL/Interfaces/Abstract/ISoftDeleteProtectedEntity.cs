namespace aspnetcore6.ntier.DAL.Interfaces.Abstract
{
    public interface ISoftDeleteProtectedEntity
    {
        bool IsSoftDeleteProtected { get; set; }
    }
}
