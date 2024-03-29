namespace aspnetcore6.ntier.BLL.Interfaces.Utilities
{
    public interface IDataSeed
    {
        public Task DevelopmentDataSeed();
        public Task TestDataSeed();
        public Task UatDataSeed();
        public Task ProductionDataSeed();
    }
}
