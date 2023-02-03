using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
public class SelectableTargetTypeException : SelectableTargetException {
    public SelectableTargetTypeException(Type targetType) : base(MakeMessage(targetType)) { }

    private static string MakeMessage(Type targetType) =>
        $"Invalid application of {nameof(SelectableFormAttribute)} to type {targetType.FullName}";
}
