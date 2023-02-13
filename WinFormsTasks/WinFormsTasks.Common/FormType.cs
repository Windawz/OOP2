using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
internal static class FormType {
    public static bool IsFormType(Type type) =>
        type.IsAssignableTo(typeof(Form));
}
