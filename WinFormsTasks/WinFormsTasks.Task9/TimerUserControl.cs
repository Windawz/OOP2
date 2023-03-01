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
public partial class TimerUserControl : UserControl {
    public TimerUserControl() {
        InitializeComponent();

        Size = new(300, 200);

        _label = new Label() {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter,
        };
        Controls.Add(_label);

        _timer = new Timer() {
            Interval = 1000,
        };
        EventHandler onTick = delegate {
            _label.Text = DateTime.Now.ToLongTimeString();
        };
        _timer.Tick += onTick;
        components!.Add(_timer);

        onTick(_timer, EventArgs.Empty);
    }

    private readonly Label _label;
    private readonly Timer _timer;

    public float TextSizeInPoints {
        get => _label.Font.SizeInPoints;
        set {
            var oldFont = _label.Font;
            var newFont = new Font(
                oldFont.FontFamily, value);
            _label.Font = newFont;
            oldFont.Dispose();
        }
    }

    public bool TimerEnabled {
        get => _timer.Enabled;
        set => _timer.Enabled = value;
    }
}
