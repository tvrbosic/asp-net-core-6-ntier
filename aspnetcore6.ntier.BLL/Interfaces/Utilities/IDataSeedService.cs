namespace aspnetcore6.ntier.Services.Interfaces.Utilities
{
    public interface IDataSeedService
    {
        public Task DevelopmentDataSeed();
        public Task TestDataSeed();
        public Task UatDataSeed();
        public Task ProductionDataSeed();
    }
}
