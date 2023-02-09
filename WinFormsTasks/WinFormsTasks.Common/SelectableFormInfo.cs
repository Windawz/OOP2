using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
public readonly record struct SelectableFormInfo(
    Func<Form> Factory,
    string? FormName,
    Type FormType);
