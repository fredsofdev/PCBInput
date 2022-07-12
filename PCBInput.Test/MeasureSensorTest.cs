using DBLib;
using DBLib.Record.Entities;
using DBLib.Setup;
using DBLib.Setup.Entities;
using Moq;
using PCBInput.Manipulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PCBInput.Test
{
    public class MeasureSensorTest
    {

        [Fact]
        public void TestMeasureEmpty()
        {
            var Sensorlist = new List<ItemDetail>()
            {
                new ItemDetail()
                    {
                        Id = 1,
                        FacIdForKey = 61,
                        Facility = Constants.FAC[61],
                        FacilityNum = 0,
                        ItemCode = "T",
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = -2,
                        ItemMaxRange = 40,
                        DefaultValue = 0,
                        InputCh = 1,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 1
                    },
                new ItemDetail()
                    {
                        Id = 2,
                        FacIdForKey = 41,
                        FacilityNum = 0,
                        Facility = Constants.FAC[41],
                        ItemCode = "D",
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 0,
                        ItemMaxRange = 150,
                        DefaultValue = 0,
                        InputCh = 2,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 0
                    },
                new ItemDetail()
                    {
                        Id= 3,
                        FacIdForKey = 61,
                        Facility = Constants.FAC[61],
                        FacilityNum = 0,
                        ItemCode = "H",
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 4.5,
                        ItemMaxRange = 7.5,
                        DefaultValue = 0,
                        InputCh = 3,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 2
                    },
                new ItemDetail()
                    {
                        Id = 4,
                        FacIdForKey = 62,
                        Facility = Constants.FAC[62],
                        FacilityNum = 0,
                        ItemCode = "A",
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 10,
                        ItemMaxRange = 20,
                        DefaultValue = 15,
                        InputCh = 4,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 0
                    },
                new ItemDetail()
                    {
                        Id = 5,
                        FacIdForKey = 1,
                        Facility = Constants.FAC[1],
                        FacilityNum = 0,
                        ItemCode = "A",
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 10,
                        ItemMaxRange = 20,
                        DefaultValue = 15,
                        InputCh = 5,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 0
                    }
            };

            var modifier = getManipulator(Sensorlist);

            var result =  modifier.Manipulate(new List<int> { }, DateTime.Now);

            Assert.Empty(result);
        }

        [Fact]
        public void TestMeasureIsInspecting()
        {
            var Sensorlist = new List<ItemDetail>()
            {
                new ItemDetail()
                    {
                        Id = 1,
                        FacIdForKey = 61,
                        Facility = Constants.FAC[61],
                        FacilityNum = 0,
                        ItemCode = "T",
                        ItemType = Constants.TYPES.Single(t =>t.code == "T"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = -2,
                        ItemMaxRange = 40,
                        DefaultValue = 0,
                        InputCh = 1,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 1
                    },
                new ItemDetail()
                    {
                        Id = 2,
                        FacIdForKey = 41,
                        FacilityNum = 0,
                        Facility = Constants.FAC[41],
                        ItemCode = "D",
                        ItemType = Constants.TYPES.Single(t =>t.code == "D"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 0,
                        ItemMaxRange = 150,
                        DefaultValue = 0,
                        InputCh = 2,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 0
                    },
                new ItemDetail()
                    {
                        Id= 3,
                        FacIdForKey = 61,
                        Facility = Constants.FAC[61],
                        FacilityNum = 0,
                        ItemCode = "H",
                        ItemType = Constants.TYPES.Single(t =>t.code == "H"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 4.5,
                        ItemMaxRange = 7.5,
                        DefaultValue = 0,
                        InputCh = 3,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 2
                    },
                new ItemDetail()
                    {
                        Id = 4,
                        FacIdForKey = 62,
                        Facility = Constants.FAC[62],
                        FacilityNum = 0,
                        ItemCode = "A",
                        ItemType = Constants.TYPES.Single(t =>t.code == "A"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 10,
                        ItemMaxRange = 20,
                        DefaultValue = 15,
                        InputCh = 4,
                        SerPort = 4,
                        IsInspection = true,
                        InputType = 0
                    },
                new ItemDetail()
                    {
                        Id = 5,
                        FacIdForKey = 1,
                        Facility = Constants.FAC[1],
                        FacilityNum = 0,
                        ItemCode = "A",
                        ItemType = Constants.TYPES.Single(t =>t.code == "A"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 10,
                        ItemMaxRange = 20,
                        DefaultValue = 15,
                        InputCh = 5,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 0
                    }
            };

            var modifier = getManipulator(Sensorlist);

            var result = modifier.Manipulate(new List<int> {0,0,0,0,0,0,0,0}, DateTime.Now);

            Item targetItem = result.Where(x => x.FacilityCode.Contains("P")).FirstOrDefault()!;

            Assert.Equal(8, targetItem.RptState);
            Assert.Equal(4, result.Where(s =>s.RptState == 8).ToList().Count);
        }

        [Fact]
        public void TestMeasureConnectionProblem()
        {
            var Sensorlist = new List<ItemDetail>()
            {
                new ItemDetail()
                    {
                        Id = 1,
                        FacIdForKey = 61,
                        Facility = Constants.FAC[61],
                        FacilityNum = 0,
                        ItemCode = "T",
                        ItemType = Constants.TYPES.Single(t =>t.code == "T"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = -2,
                        ItemMaxRange = 40,
                        DefaultValue = 0,
                        InputCh = 1,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 1
                    },
                new ItemDetail()
                    {
                        Id = 2,
                        FacIdForKey = 41,
                        FacilityNum = 0,
                        Facility = Constants.FAC[41],
                        ItemCode = "D",
                        ItemType = Constants.TYPES.Single(t =>t.code == "D"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 0,
                        ItemMaxRange = 150,
                        DefaultValue = 0,
                        InputCh = 2,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 0
                    },
                new ItemDetail()
                    {
                        Id= 3,
                        FacIdForKey = 61,
                        Facility = Constants.FAC[61],
                        FacilityNum = 0,
                        ItemCode = "H",
                        ItemType = Constants.TYPES.Single(t =>t.code == "H"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 4.5,
                        ItemMaxRange = 7.5,
                        DefaultValue = 0,
                        InputCh = 3,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 2
                    },
                new ItemDetail()
                    {
                        Id = 4,
                        FacIdForKey = 62,
                        Facility = Constants.FAC[62],
                        FacilityNum = 0,
                        ItemCode = "A",
                        ItemType = Constants.TYPES.Single(t =>t.code == "A"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 10,
                        ItemMaxRange = 20,
                        DefaultValue = 15,
                        InputCh = 4,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 0
                    },
                new ItemDetail()
                    {
                        Id = 5,
                        FacIdForKey = 1,
                        Facility = Constants.FAC[1],
                        FacilityNum = 0,
                        ItemCode = "A",
                        ItemType = Constants.TYPES.Single(t =>t.code == "A"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = 10,
                        ItemMaxRange = 20,
                        DefaultValue = 15,
                        InputCh = 5,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 0
                    }
            };

            var modifier = getManipulator(Sensorlist);

            var result = modifier.Manipulate(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0 }, DateTime.Now);

            Item targetItem = result.FirstOrDefault()!;

            Assert.Equal(2, targetItem.RptState);
            Assert.Equal(Sensorlist.Count, result.Where(t=>t.RptState == 2).ToList().Count);
        }

        [Fact]
        public void TestMeasureNormalState()
        {
            var Sensorlist = new List<ItemDetail>()
            {
                new ItemDetail()
                    {
                        Id = 1,
                        FacIdForKey = 61,
                        Facility = Constants.FAC[61],
                        FacilityNum = 0,
                        ItemCode = "T",
                        ItemType = Constants.TYPES.Single(t =>t.code == "T"),
                        ItemNum = 0,
                        DeviceCode = "0",
                        IsAlarmState = false,
                        ItemMinRange = -2,
                        ItemMaxRange = 40,
                        DefaultValue = 10,
                        InputCh = 1,
                        SerPort = 4,
                        IsInspection = false,
                        InputType = 1
                    },
            };

            var modifier = getManipulator(Sensorlist);

            var result = modifier.Manipulate(new List<int> { 1600, 0, 0}, DateTime.Now);

            Item targetItem = result.FirstOrDefault()!;

            Assert.Equal(0, targetItem.RptState);
            Assert.Equal(Sensorlist.Count, result.Where(t => t.RptState == 0).ToList().Count);
        }

        [Fact]
        public void TestMeasureAbnormalState()
        {
            var Sensorlist = new List<ItemDetail>()
            {
                new ItemDetail()
                {
                    Id = 1,
                    FacIdForKey = 61,
                    Facility = Constants.FAC[61],
                    FacilityNum = 0,
                    ItemCode = "T",
                    ItemType = Constants.TYPES.Single(t =>t.code == "T"),
                    ItemNum = 0,
                    DeviceCode = "0",
                    IsAlarmState = false,
                    ItemMinRange = -2,
                    ItemMaxRange = 20,
                    DefaultValue = 2,
                    InputCh = 1,
                    SerPort = 4,
                    IsInspection = false,
                    InputType = 1
                },
            };

            var modifier = getManipulator(Sensorlist);

            var result = modifier.Manipulate(new List<int> { 4000, 0, }, DateTime.Now);

            Item targetItem = result.FirstOrDefault()!;

            Assert.Equal(1, targetItem.RptState);
        }

        private MeasureSensor getManipulator(List<ItemDetail> details)
        {
            var dbSetMock = Helper.getMockDbSet(details);
            var context = new Mock<DbContextBase>();

            context.Setup(x => x.Set<ItemDetail>()).Returns(dbSetMock.Object);

            return new MeasureSensor(new UnitOfWork<ItemDetailRepository>(context.Object));
        }
    }
}
