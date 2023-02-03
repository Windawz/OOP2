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

        var buttons = infos.Select(info => MakeOpenerButton(info));
        var container = MakeButtonContainer(buttons);
        Controls.Add(container);

        Text = "Form Selector";
    }

    private static Control MakeButtonContainer(IEnumerable<Button> buttons) {
        var container = new FlowLayoutPanel() {
            AutoScroll = true,
            BorderStyle = BorderStyle.Fixed3D,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            Padding = new(0, 0, 7, 0),
            WrapContents = false,
        };

        container.Controls.AddRange(buttons.ToArray());
        container.SizeChanged += delegate {
            AdjustSize(container.Controls.Cast<Control>());
        };

        return container;
    }

    private static Button MakeOpenerButton(SelectableFormInfo info) {
        var button = new Button() {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowOnly,
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
}
