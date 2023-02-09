using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
public class StackPanel : FlowLayoutPanel {
    public StackPanel() {
        AutoScroll = true;
        FlowDirection = FlowDirection.TopDown;
        Padding = new(
            Padding.Left,
            Padding.Top, 
            Math.Max(Padding.Right, 7), 
            Padding.Bottom);
        WrapContents = false;

        Layout += delegate {
            AdjustSize(Controls.Cast<Control>());
        };
    }

    private static void AdjustSize(IEnumerable<Control> controls) {
        if (!controls.Any()) {
            return;
        }
        var parent = controls.First().Parent
            ?? throw new InvalidOperationException("Control whose size is adjusted has no parent");
        int width = parent.ClientSize.Width - parent.Padding.Right;
        int height = controls.Select(b => b.Height).Max();
        foreach (var control in controls) {
            control.Width = width;
            control.Height = height;
        }
    }
}
