using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.Manipulator
{
    public interface IManipulator<T, V>
    {
        List<V> Manipulate(List<T> data, DateTime date);
    }
}
