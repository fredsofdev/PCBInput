using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.Writer
{
    public interface IDataWriter<T>
    {
        void Write(List<T> data, DateTime date);
    }
}
