﻿using DBLib;
using DBLib.Record;
using DBLib.Record.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.DataProvider
{
    public class DailyDataProvider : RecordRepoBase<SendRecordRepository>, IDataProvider<SendItem>
    {

        public DailyDataProvider(IUnitOfWork<SendRecordRepository> work)=>
            this.work = work;

        public List<SendItem> GetData(DateTime date)
        {
            RenewUnitOfWork<SendDataContext>(date);
            return work!.Repo.GetAll().ToList();
        }
    }
}
