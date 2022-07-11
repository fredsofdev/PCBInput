using DBLib;
using DBLib.Record;
using DBLib.Record.Entities;
using Moq;
using PCBInput.Writer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PCBInput.Test
{
    public class DataWriterTest
    {
        [Fact]
        public void TestDataWriterRenewRepo()
        {
            var testList = new List<Item>();
            var firstTime = DateTime.Now;
            int itemCount = 5;
            foreach (var id in Enumerable.Range(1, itemCount))
            {
                testList.Add(new Item()
                {
                    Date = firstTime,
                    ItemId = id,
                });
            }
            var dbSetMock = Helper.getMockDbSet(testList);
            var context = new Mock<DbContextBase>();

            context.Setup(x => x.Set<Item>()).Returns(dbSetMock.Object);
            var date = DateTime.Now;
            context.Setup(x => x.dbFile).Returns($"local\\DAT_{date.ToString("yyyyMMddHH")}.db");

            var dataWriter = new SecondDataWriter(new UnitOfWork<ItemRecordRepository>(context.Object));
            
            dataWriter.Write(testList, date);
            
            var result = dataWriter.work!.Repo.GetAll().ToList();

            Assert.True(result.SequenceEqual(testList),$"Result count {result.Count}");
        }
    }
}
