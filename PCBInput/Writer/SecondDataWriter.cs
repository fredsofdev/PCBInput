using DBLib;
using DBLib.Record;
using DBLib.Record.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.Writer
{
    public class SecondDataWriter : IDataWriter<Item>
    {
        private IUnitOfWork<IItemRecordRepository> work;

        public SecondDataWriter(IUnitOfWork<IItemRecordRepository> work)
        {
            this.work = work;
        }

        public void Write(List<Item> data, DateTime date)
        {
            var dbDate = work.Repo.GetDbContextDate();
            if(date.Hour != dbDate.Hour)
            {
                RenewUnitOfWork(date);
            }
            work.Repo.AddRange(data);
        }

        private void RenewUnitOfWork(DateTime date)
        {
            work.Dispose();
            work = (IUnitOfWork<IItemRecordRepository>) new UnitOfWork<ItemRecordRepository>(new RecordDataContext(date));
        }
    }
}
