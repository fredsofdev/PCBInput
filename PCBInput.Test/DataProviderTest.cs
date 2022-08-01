using DBLib;
using DBLib.Record;
using DBLib.Record.Entities;
using Moq;
using PCBInput.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PCBInput.Test
{
    public class DataProviderTest
    {
        [Fact]
        public void DataProviderGetDataTest()
        {
            var testList = new List<Item>();
            var firstTime = DateTime.Now;
            int itemCount = 5;
            foreach (var id in Enumerable.Range(1, itemCount))
            {
                testList.Add(new Item()
                {
                    Date = firstTime.AddMinutes(-5),
                    ItemId = id,
                });
            }
            var dbSetMock = Helper.getMockDbSet(testList);
            var context = new Mock<RecordDataContext>();

            context.Setup(x => x.Set<Item>()).Returns(dbSetMock.Object);
            
            context.Setup(x => x.dbFile).Returns($"local\\DAT_{firstTime.ToString("yyyyMMddHH")}.db");
            var work = new UnitOfWork<ItemRecordRepository, RecordDataContext>(context.Object);

            var dataWriter = new CollectedDataProvider(work);

            //var data = dataWriter.GetData(firstTime);
            var data = work.Repo.GetDurationalRecord(firstTime);

            Assert.True(data.SequenceEqual(testList), $"Result count {data.Count}");

        }
    }
}
