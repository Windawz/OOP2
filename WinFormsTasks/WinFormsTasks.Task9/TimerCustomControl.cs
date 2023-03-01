using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTasks.Task9;
public partial class TimerCustomControl : Control {
    public TimerCustomControl() {
        InitializeComponent();

        Size = new(60, 20);

        _timer = new Timer() {
            Interval = 1000,
        };
        _timer.Tick += delegate {
            Refresh();
        };
        components!.Add(_timer);
    }

    private readonly Timer _timer;

    public bool TimerEnabled {
        get => _timer.Enabled;
        set => _timer.Enabled = value;
    }

    protected override void OnPaint(PaintEventArgs pe) {
        base.OnPaint(pe);
        var graphics = pe.Graphics;
        graphics.FillRectangle(
            Brushes.Blue,
            0,
            0,
            Width,
            Height);

        string timeString = DateTime.Now.ToLongTimeString();
        graphics.DrawString(
            timeString,
            Font,
            new SolidBrush(ForeColor),
            0,
            0);
    }
}
