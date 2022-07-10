﻿using DBLib;
using DBLib.Record;
using DBLib.Record.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.Writer
{
    public class SecondDataWriter : RecordRepoBase<IItemRecordRepository>, IDataWriter<Item>
    {
        public SecondDataWriter(IUnitOfWork<IItemRecordRepository> work) =>
            this.work = work;
        
        public void Write(List<Item> data, DateTime date)
        {
            RenewUnitOfWork<RecordDataContext>(date);
            work!.Repo.AddRange(data);
        }
    }
}
