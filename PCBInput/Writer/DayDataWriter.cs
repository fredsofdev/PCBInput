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
    public class DayDataWriter : RecordRepoBase<IDayEndRecordRepository>, IDataWriter<DayEndRecord>
    {
        public DayDataWriter(IUnitOfWork<IDayEndRecordRepository> work) =>
            this.work = work;

        public void Write(List<DayEndRecord> data, DateTime date)
        {
             RenewUnitOfWork<SendDataContext>(date);
             work!.Repo.AddRange(data);
        }
    }
}