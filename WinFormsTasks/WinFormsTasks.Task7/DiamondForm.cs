using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WinFormsTasks.Common;

namespace WinFormsTasks.Task7;
public partial class DiamondForm : Form {
    public DiamondForm() {
        InitializeComponent();
        MakeDiamondShaped(this);
        AddCloseButton(this);
        Draggable.MakeDraggable(this);
        BackColor = Color.Green;
    }

    private static void AddCloseButton(Form form) {
        var center = new Point(form.ClientSize.Width / 2, form.ClientSize.Height / 2);
        var button = new Button() {
            AutoSize = true,
            Text = "Close Form",
        };
        button.Click += delegate {
            form.Close();
        };
        var location = new Point(center.X - button.Width / 2, center.Y - button.Height / 2);
        button.Location = location;
        button.BackColor = Color.White;
        form.Controls.Add(button);
    }

    private static void MakeDiamondShaped(Form form) {
        var size = form.ClientSize;
        var center = new Point(size.Width / 2, size.Height / 2);

        var path = new GraphicsPath();
        path.AddPolygon(new Point[] {
            new(center.X, 0),
            new(0, center.Y),
            new(center.X, size.Height),
            new(size.Width, center.Y),
        });
        form.Region = new Region(path);
        form.FormBorderStyle = FormBorderStyle.None;
    }
}
