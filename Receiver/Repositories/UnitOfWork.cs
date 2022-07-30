namespace Receiver.Repositories
{
    public sealed class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _appContext;

        public UnitOfWork(AppDbContext appContext)
        {
            this._appContext = appContext;
        }

        public async Task SaveChangesAsync()
        {
            await this._appContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            this._appContext.Dispose();
        }
    }
}