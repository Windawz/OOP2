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

namespace WinFormsTasks.Task7;
[SelectableForm("Diamond Form Opener")]
public partial class DiamondFormHostForm : Form {
    public DiamondFormHostForm() {
        InitializeComponent();

        var openerButton = new Button() {
            AutoSize = true,
            Dock = DockStyle.Top,
            Text = "Open Diamond Form",
        };

        void OpenDiamondForm(object? sender, EventArgs e) {
            var diamondForm = new DiamondForm();
            AddOwnedForm(diamondForm);
            diamondForm.FormClosed += delegate {
                openerButton.Click += OpenDiamondForm;
            };
            openerButton.Click -= OpenDiamondForm;
            diamondForm.Show();
        };

        openerButton.Click += OpenDiamondForm;
        Controls.Add(openerButton);
    }
}
