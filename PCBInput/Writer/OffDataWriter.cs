using DBLib.Record;
using DBLib.Record.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.Writer
{
    public class OffDataWriter : RecordRepoBase<SendRecordRepository>, IDataWriter<IGrouping<DateTime, SendItem>>
    {
        public void Write(List<IGrouping<DateTime, SendItem>> data, DateTime date)
        {
            foreach(var group in data)
            {
                RenewUnitOfWork<SendDataContext>(group.Key);
                work!.Repo.AddRange(group.ToList());
                work!.Complete();
            }
        }
    }
}
