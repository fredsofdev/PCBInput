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
    public class MinuteDataWriter : RecordRepoBase<SendRecordRepository>, IDataWriter<SendItem>
    {
        public MinuteDataWriter(IUnitOfWork<SendRecordRepository> work) =>
            this.work = work;
        
        public void Write(List<SendItem> data, DateTime date)
        {
            RenewUnitOfWork<SendDataContext>(date);
            work!.Repo.AddRange(data);
            work!.Complete();
        }
    }
}
