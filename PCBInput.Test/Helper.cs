using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.Test
{
    internal class Helper
    {
        public static Mock<DbSet<TEntity>> getMockDbSet<TEntity>(List<TEntity> list) where TEntity : class
        {
            var dbSetMock = new Mock<DbSet<TEntity>>();
            dbSetMock.As<IQueryable<TEntity>>().Setup(x => x.Provider).Returns(list.AsQueryable().Provider);
            dbSetMock.As<IQueryable<TEntity>>().Setup(x => x.Expression).Returns(list.AsQueryable().Expression);
            dbSetMock.As<IQueryable<TEntity>>().Setup(x => x.ElementType).Returns(list.AsQueryable().ElementType);
            dbSetMock.As<IQueryable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(list.AsQueryable().GetEnumerator());
            return dbSetMock;
        }
    }
}
