using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
public class SelectableTargetFactoryException : SelectableTargetException {
    public SelectableTargetFactoryException(Type targetType) : base(MakeMessage(targetType)) { }

    private static string MakeMessage(Type targetType) =>
        $"Selectable form of type {targetType.FullName} has no valid (parameterless) constructor";
}
