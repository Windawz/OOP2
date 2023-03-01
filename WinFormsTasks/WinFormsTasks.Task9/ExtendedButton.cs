using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTasks.Task9;
internal class ExtendedButton : Button {
    public int ClickCount { get; private set; } = 0;

    protected override void OnClick(EventArgs e) {
        ClickCount += 1;
        base.OnClick(e);
    }

    protected override void OnPaint(PaintEventArgs pevent) {
        base.OnPaint(pevent);

        Graphics graphics = pevent.Graphics;
        string clickCountString = ClickCount.ToString();
        SizeF size = graphics.MeasureString(clickCountString, Font, Width);
        graphics.DrawString(
            clickCountString,
            Font,
            SystemBrushes.ControlText,
            Width - size.Width - 3,
            Height - size.Height - 3);
    }
}
