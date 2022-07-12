using DBLib;
using DBLib.Record;

namespace PCBInput
{
    public abstract class RecordRepoBase<T> where T : IRecordRepository
    {
        public IUnitOfWork<T>? work { get; set; }

        public void RenewUnitOfWork<V>(DateTime date) where V : DbContextBase
        {
            var dbDate = work!.Repo.GetDbContextDate();
            if(work.Repo.GetType() == typeof(RecordDataContext))
            {
                if (date.Hour != dbDate.Hour)
                {
                    work!.Dispose();
                    work = new UnitOfWork<T>((V)Activator.CreateInstance(typeof(V), date)!);
                }
            }
            else if(work.Repo.GetType() == typeof(SendDataContext))
            {
                if (date.Day != dbDate.Day)
                {
                    work!.Dispose();
                    work = new UnitOfWork<T>((V)Activator.CreateInstance(typeof(V), date)!);
                }
            }
        }
    }
}
