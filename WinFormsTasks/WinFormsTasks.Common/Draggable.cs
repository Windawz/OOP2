using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
public static class Draggable {
    // From https://stackoverflow.com/questions/1592876/make-a-borderless-form-movable

    private const int WM_NCLBUTTONDOWN = 0xa1;
    private const int HT_CAPTION = 0x2;

    public static void MakeDraggable(Form form) {
        form.MouseDown += (_, e) => {
            if (e.Button != MouseButtons.Left) {
                return;
            }

            ReleaseCapture();
            SendMessage(form.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        };
    }

    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();
}
