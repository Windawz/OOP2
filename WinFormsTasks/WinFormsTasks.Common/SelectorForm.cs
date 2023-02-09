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

    public SelectorForm(params SelectableFormInfo[] infos) : this(infos.AsEnumerable()) { }

    private SelectorForm(Assembly assembly) : this(SelectableFormLookup.EnumerateFormInfos(assembly)) { }

    private SelectorForm(IEnumerable<SelectableFormInfo> infos) {
        InitializeComponent();

        var buttons = infos
            .Select(info => MakeOpenerButton(info))
            .ToArray();

        var container = new StackPanel() {
            Dock = DockStyle.Fill,
        };
        container.Controls.AddRange(buttons);

        Controls.Add(container);

        Text = "Form Selector";
    }

    private static Button MakeOpenerButton(SelectableFormInfo info) {
        var button = new Button() {
            AutoSize = true,
            Text = info.FormName,
        };

        void OnClick(object? sender, EventArgs e) {
            button.Click -= OnClick;
            var form = info.Factory();
            form.FormClosing += delegate {
                button.Click += OnClick;
            };

            form.Show();
            form.Activate();
        }

        button.Click += OnClick;

        return button;
    }

    private static void AdjustSize(IEnumerable<Control> controls) {
        if (!controls.Any()) {
            return;
        }
        var parent = controls.First().Parent
            ?? throw new InvalidOperationException("Control whose size is adjusted has no parent");
        int width = parent.ClientSize.Width - parent.Padding.Right;
        int height = controls.Select(b => b.Height).Max();
        foreach (var control in controls) {
            control.AutoSize = false;
            control.Width = width;
            control.Height = height;
            control.Visible = true;
        }
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
}
