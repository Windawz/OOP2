using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTasks.Task8;
public partial class MdiChildForm : Form {
    public MdiChildForm() {
        InitializeComponent();

        var richTextBox = MakeRichTextBox();

        Controls.Add(richTextBox);
    }

    private static MenuStrip MakeMenuStrip() {
        var menuStrip = new MenuStrip() {
            Text = "File",
        };
        
        var newSubmenu = new ToolStripMenuItem() {
            Text = "New",
        };
        var windowSubmenu = new ToolStripMenuItem() {
            Text = "Window",
        };

        menuStrip.Items.AddRange(new[] {
            newSubmenu,
            windowSubmenu,
        });

        return menuStrip;
    }

    private static RichTextBox MakeRichTextBox() {
        var richTextBox = new RichTextBox() {
            Anchor = AnchorStyles.Top | AnchorStyles.Left,
            Dock = DockStyle.Fill,
        };

        return richTextBox;
    }
}
