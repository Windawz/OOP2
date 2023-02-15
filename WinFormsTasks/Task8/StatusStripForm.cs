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

namespace WinFormsTasks.Task8;
[SelectableForm("Status Strip Form (Excercise 4)", 4)]
public partial class StatusStripForm : ToolStripForm {
    public StatusStripForm() {
        InitializeComponent();

        Size = new(450, 350);
        WindowState = FormWindowState.Normal;

        var statusStrip = new StatusStrip() {
            Text = string.Empty,
        };
        Controls.Add(statusStrip);

        var statusStatusLabel = new ToolStripStatusLabel() {
            Name = "spWin",
            Text = "Status",
        };
        statusStrip.Items.Add(statusStatusLabel);

        CascadeButtonClicked += delegate {
            statusStatusLabel.Text = "Windows are cascaded";
        };

        TileButtonClicked += delegate {
            statusStatusLabel.Text = "Windows are tiled";
        };

        var dataStatusLabel = new ToolStripStatusLabel() {
            Name = "spData",
            Text = "Data",
        };
        statusStrip.Items.Add(dataStatusLabel);
    }
}
