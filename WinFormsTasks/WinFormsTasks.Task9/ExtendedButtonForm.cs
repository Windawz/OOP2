using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinFormsTasks.Common;

namespace WinFormsTasks.Task9;
[SelectableForm]
public partial class ExtendedButtonForm : Form {
    public ExtendedButtonForm() {
        InitializeComponent();

        var extendedButton = new ExtendedButton() {
            Text = "Click Me!",
            Size = new(180, 75),
        };
        Controls.Add(extendedButton);
    }
}
