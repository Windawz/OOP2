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
[SelectableForm("Mdi Parent Form")]
public partial class MdiParentForm : Form {
    public MdiParentForm() {
        InitializeComponent();

        IsMdiContainer = true;
        WindowState = FormWindowState.Maximized;

        var addSubmenu = MakeAddSubmenu(this);
        var closeSubmenu = MakeCloseSubmenu(this);
        var windowsSubmenu = MakeWindowsSubmenu(this);

        var menuStrip = MakeMenuStrip();

        AddSubmenus(menuStrip,
            addSubmenu,
            windowsSubmenu,
            closeSubmenu);

        Controls.Add(menuStrip);
    }

    private static ToolStripMenuItem MakeSubmenu() {
        var submenu = new ToolStripMenuItem() { };
        return submenu;
    }

    private static bool IsChildActive(Form mdiChild) =>
        mdiChild.MdiParent?.ActiveMdiChild is not null and Form activeChild && activeChild == mdiChild;

    private static void DoHighlightButtonIfChildActive(ToolStripButton button, Form mdiChild) {
        button.Paint += delegate {
            if (IsChildActive(mdiChild)) {
                button.Checked = true;
            }
        };
    }

    private static ToolStripButton MakeButtonForChild(Form mdiChild) {
        var button = new ToolStripButton() {
            AutoSize = true,
            Text = mdiChild.Text,
            Width = 75,
        };

        return button;
    }

    private static void AddButtonsForMdiChildren(
        Form mdiParent,
        ToolStripMenuItem submenu,
        Action<ToolStripButton, Form> buttonAndChildConfigurer
    ) {
        submenu.DropDownOpening += delegate {
            var buttons = mdiParent.MdiChildren
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

    private static ToolStripMenuItem MakeAddSubmenu(Form mdiParent) {
        var submenu = MakeSubmenu();
        submenu.Text = "&Add";
        submenu.Click += delegate {
            var child = new MdiChildForm();
            child.MdiParent = mdiParent;
            child.Text = $"Child {mdiParent.MdiChildren.Length}";
            child.Show();
        };
        return submenu;
    }

    private static ToolStripMenuItem MakeCloseSubmenu(Form mdiParent) {
        var submenu = MakeSubmenu();
        submenu.Text = "&Close";
        AddButtonsForMdiChildren(mdiParent, submenu, (button, mdiChild) => {
            button.Click += delegate {
                mdiChild.Close();
            };
        });
        return submenu;
    }

    private static ToolStripMenuItem MakeWindowsSubmenu(Form mdiParent) {
        var submenu = MakeSubmenu();
        submenu.Text = "&Windows";

        AddButtonsForMdiChildren(mdiParent, submenu, (button, mdiChild) => {
            button.Click += delegate {
                mdiChild.Activate();
            };
            DoHighlightButtonIfChildActive(button, mdiChild);
        });

        return submenu;
    }

    private static MenuStrip MakeMenuStrip() =>
        new MenuStrip() {
            Text = "&File",
        };

    private static void AddSubmenus(MenuStrip menuStrip, params ToolStripMenuItem[] submenus) {
        menuStrip.Items.AddRange(submenus);
    }
}
