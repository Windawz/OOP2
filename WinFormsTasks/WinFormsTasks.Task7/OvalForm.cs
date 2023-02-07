using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinFormsTasks.Common;

namespace WinFormsTasks.Task7;
[SelectableForm("Oval Form")]
public partial class OvalForm : Form {
    public OvalForm() {
        InitializeComponent();
        MakeOval(this);
        AddCloseButton(this);
        Draggable.MakeDraggable(this);

        BackColor = Color.DarkOrange;
    }

    private static void AddCloseButton(Form form) {
        var button = new Button() {
            AutoSize = true,
            BackColor = Color.White,
            Text = "Close Form",
        };

        button.Location = new Point(form.Width / 2 - button.Width / 2, form.Height / 2 - button.Height / 2);

        button.Click += delegate {
            form.Close();
        };

        form.Controls.Add(button);
    }

    private static void MakeOval(Form form) {
        form.FormBorderStyle = FormBorderStyle.None;
        var sizeRect = form.ClientRectangle;
        var path = new GraphicsPath();
        path.AddEllipse(sizeRect);
        form.Region = new Region(path);
    }
}
