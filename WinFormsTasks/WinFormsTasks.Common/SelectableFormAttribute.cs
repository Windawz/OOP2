using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
[System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class SelectableFormAttribute : Attribute {
    public SelectableFormAttribute(string? formName = null, int weight = default) {
        FormName = formName;
        Weight = weight;
    }

    public string? FormName { get; }
    public int Weight { get; }
}
