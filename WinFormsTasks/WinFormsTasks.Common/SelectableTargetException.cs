using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
public class SelectableTargetException : Exception {
    public SelectableTargetException(string message) : base(message) { }
}
