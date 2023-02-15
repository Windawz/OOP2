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
public partial class MdiParentForm : Form {
    public MdiParentForm() {
        InitializeComponent();

        IsMdiContainer = true;
        WindowState = FormWindowState.Maximized;

        var addSubmenu = MakeAddSubmenu();
        var closeSubmenu = MakeCloseSubmenu();
        var windowsSubmenu = MakeWindowsSubmenu();

        var menuStrip = MakeMenuStrip();

        AddSubmenus(menuStrip,
            addSubmenu,
            windowsSubmenu,
            closeSubmenu);

        Controls.Add(menuStrip);

        AddSubmenu = addSubmenu;
        CloseSubmenu = closeSubmenu;
        WindowsSubmenu = windowsSubmenu;
    }

    public ToolStripMenuItem AddSubmenu { get; }
    public ToolStripMenuItem CloseSubmenu { get; }
    public ToolStripMenuItem WindowsSubmenu { get; }

    public event EventHandler<MdiChildForm>? ChildAdded;
    public event EventHandler? ChildClosed;

    private ToolStripMenuItem MakeAddSubmenu() {
        var submenu = MakeSubmenu();
        submenu.Text = "&Add";
        submenu.Click += delegate {
            var child = new MdiChildForm();
            child.MdiParent = this;
            child.Text = $"Child {MdiChildren.Length}";
            child.Show();

            ChildAdded?.Invoke(this, child);
        };
        return submenu;
    }

    private ToolStripMenuItem MakeCloseSubmenu() {
        var submenu = MakeSubmenu();
        submenu.Text = "&Close";
        AddButtonsForMdiChildren(this, submenu, (button, mdiChild) => {
            button.Click += delegate {
                mdiChild.Close();
            };

            ChildClosed?.Invoke(this, EventArgs.Empty);
        });
        return submenu;
    }

    private ToolStripMenuItem MakeWindowsSubmenu() {
        var submenu = MakeSubmenu();
        submenu.Text = "&Windows";

        AddButtonsForMdiChildren(this, submenu, (button, mdiChild) => {
            button.Click += delegate {
                mdiChild.Activate();
            };
            DoHighlightButtonIfChildActive(button, mdiChild);
        });

        return submenu;
    }

    private static ToolStripMenuItem MakeSubmenu() {
        var submenu = new ToolStripMenuItem() { };
        return submenu;
    }

    private static bool IsChildActive(MdiChildForm mdiChild) =>
        mdiChild.MdiParent?.ActiveMdiChild is not null and Form activeChild && activeChild == mdiChild;

    private static void DoHighlightButtonIfChildActive(ToolStripButton button, MdiChildForm mdiChild) {
        button.Paint += delegate {
            if (IsChildActive(mdiChild)) {
                button.Checked = true;
            }
        };
    }

    private static ToolStripButton MakeButtonForChild(MdiChildForm mdiChild) {
        var button = new ToolStripButton() {
            AutoSize = true,
            Text = mdiChild.Text,
            Width = 75,
        };

        return button;
    }

    private static void AddButtonsForMdiChildren(
        MdiParentForm mdiParent,
        ToolStripMenuItem submenu,
        Action<ToolStripButton, MdiChildForm> buttonAndChildConfigurer
    ) {
        submenu.DropDownOpening += delegate {
            var buttons = mdiParent.MdiChildren
                .Cast<MdiChildForm>()
                .Select(mdiChild => {
                    var button = MakeButtonForChild(mdiChild);
                    buttonAndChildConfigurer(button, mdiChild);
                    return button;
                })
                .ToArray();
            submenu.DropDownItems.Clear();
            submenu.DropDownItems.AddRange(buttons);
        };
    }

    private static MenuStrip MakeMenuStrip() =>
        new MenuStrip() {
            Text = "&File",
        };

    private static void AddSubmenus(MenuStrip menuStrip, params ToolStripMenuItem[] submenus) {
        menuStrip.Items.AddRange(submenus);
    }
}
