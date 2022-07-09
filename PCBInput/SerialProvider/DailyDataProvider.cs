using DBLib;
using DBLib.Record;
using DBLib.Record.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.SerialProvider
{
    public class CollectedDataProvider : IDataProvider<Item>
    {
        private IUnitOfWork<IItemRecordRepository> work;

        public CollectedDataProvider(IUnitOfWork<IItemRecordRepository> work)
        {
            this.work = work;
        }

        public List<Item> GetData(DateTime date)
        {
            var dbDate = work.Repo.GetDbContextDate();
            if (date.Hour != dbDate.Hour)
            {
                RenewUnitOfWork(date);
            }
            return work.Repo.GetDurationalRecord(date);
        }

        private void RenewUnitOfWork(DateTime date)
        {
            work.Dispose();
            work = (IUnitOfWork<IItemRecordRepository>)new UnitOfWork<ItemRecordRepository>(new RecordDataContext(date));
        }
    }
}
