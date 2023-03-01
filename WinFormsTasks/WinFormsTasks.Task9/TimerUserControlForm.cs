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

namespace WinFormsTasks.Task9;
[SelectableForm]
public partial class TimerUserControlForm : Form {
    public TimerUserControlForm() {
        InitializeComponent();

        var timerUserControl = new TimerUserControl() {
            BorderStyle = BorderStyle.FixedSingle,
            Dock = DockStyle.Fill,
            TextSizeInPoints = 20.0f,
            TimerEnabled = true,
        };

        Controls.Add(timerUserControl);
    }
}
