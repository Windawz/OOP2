using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTasks.Common;
public partial class SelectorForm : Form {
    public SelectorForm() : this(Assembly.GetCallingAssembly()) { }

    public SelectorForm(params (Type formType, string? formName)[] tuples) : this(TuplesToInfos(tuples)) { }

    private SelectorForm(Assembly assembly) : this(SelectableFormInfo.EnumerateSelectableForms(assembly)) { }

    private SelectorForm(IEnumerable<SelectableFormInfo> infos) {
        InitializeComponent();

        _openerButtons = infos
            .Select(info => MakeOpenerButton(info))
            .ToArray();

        var container = new StackPanel() {
            Dock = DockStyle.Fill,
        };
        container.Controls.AddRange(_openerButtons);

        Controls.Add(container);

        Text = "Form Selector";
    }

    private readonly FormOpenerButton[] _openerButtons;

    public IReadOnlyList<FormOpenerButton> OpenerButtons =>
        _openerButtons;

    private static FormOpenerButton MakeOpenerButton(SelectableFormInfo info) {
        var button = new FormOpenerButton(info.Factory) {
            AutoSize = true,
            Text = info.FormName ?? FormatFormName(info.Factory.FormType.Name),
        };
        return button;
    }

    private static string FormatFormName(string rawFormName) {
        static bool IsSpacedAway(char c) =>
            char.IsUpper(c)
            || char.IsNumber(c);

        var formattedName = new StringBuilder(rawFormName.Length + 8);
        foreach (char c in rawFormName) {
            if (IsSpacedAway(c)) {
                formattedName.Append(' ');
            }
            formattedName.Append(c);
        }

        return formattedName.ToString();
    }

    private static IEnumerable<SelectableFormInfo> TuplesToInfos(IEnumerable<(Type formType, string? formName)> tuples) =>
        tuples.Select(tuple => SelectableFormInfo.FromType(tuple.formType, tuple.formName));
}
