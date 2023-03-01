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
public partial class TimerCustomControlForm : Form {
    public TimerCustomControlForm() {
        InitializeComponent();

        var timerCustomControl = new TimerCustomControl() {
            Location = new(Size.Width / 2, Size.Height / 2),
            TimerEnabled = true,
        };
        Controls.Add(timerCustomControl);
    }
}
