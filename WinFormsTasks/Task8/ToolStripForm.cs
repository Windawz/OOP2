using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinFormsTasks.Common;
using WinFormsTasks.Task8.Properties;

namespace WinFormsTasks.Task8;
[SelectableForm("Tool Strip Form (Excercise 3)", 3)]
public partial class ToolStripForm : MdiParentForm {
    public ToolStripForm() {
        InitializeComponent();
        
        var panel = new Panel() { Dock = DockStyle.Top, Height = 30, };
        var splitter = new Splitter() { Dock = DockStyle.Top, };

        Controls.Add(splitter);
        Controls.Add(panel);

        var toolStripContainer = new ToolStripContainer() { Dock = DockStyle.Top, };
        panel.Controls.Add(toolStripContainer);

        var toolStrip = new ToolStrip() { };
        toolStripContainer.TopToolStripPanel.Controls.Add(toolStrip);

        var newButton = new ToolStripButton() {
            AutoSize = true,
            Image = Resources.icon01,
            Text = "New",
        };
        newButton.Click += delegate {
            AddSubmenu.PerformClick();
        };
        toolStrip.Items.Add(newButton);

        toolStrip.Items.Add(new ToolStripSeparator());

        var windowsCascadeButton = new ToolStripButton() {
            AutoSize = true,
            Image = Resources.icon02,
            Text = "Cascade",
        };
        windowsCascadeButton.Click += delegate {
            CascadeButtonClicked?.Invoke(this, EventArgs.Empty);
            LayoutMdi(MdiLayout.Cascade);
        };
        toolStrip.Items.Add(windowsCascadeButton);

        toolStrip.Items.Add(new ToolStripSeparator());

        var windowsTileButton = new ToolStripButton() {
            AutoSize = true,
            Image = Resources.icon03,
            Text = "Tile Horizontally",
        };
        windowsTileButton.Click += delegate {
            TileButtonClicked?.Invoke(this, EventArgs.Empty);
            LayoutMdi(MdiLayout.TileHorizontal);
        };
        toolStrip.Items.Add(windowsTileButton);

        toolStrip.Items.Add(new ToolStripSeparator());

        var closeAllButton = new ToolStripButton() {
            AutoSize = true,
            Text = "Close All",
        };
        closeAllButton.Click += delegate {
            foreach (var child in MdiChildren) {
                child.Close();
            }
        };
        toolStrip.Items.Add(closeAllButton);
    }

    public event EventHandler? CascadeButtonClicked;
    public event EventHandler? TileButtonClicked;
}
