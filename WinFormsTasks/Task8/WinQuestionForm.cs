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

namespace WinFormsTasks.Task8;
[SelectableForm("WinQuestion (Excercise 1)", 1)]
public partial class WinQuestionForm : Form {
    public WinQuestionForm() {
        InitializeComponent();

        FormBorderStyle = FormBorderStyle.Fixed3D;
        Size = new Size(350, 200);
        Text = "Насущный вопрос";

        var label = new Label() {
            AutoSize = true,
            Location = new Point(0, 0),
            Text = "Вы довольны своей зарплатой?",
        };

        var buttonYes = new Button() {
            AutoSize = true,
            Location = new Point(0, 40),
            Text = "Да",
        };

        buttonYes.Click += delegate {
            MessageBox.Show(
                this,
                "Мы и не сомневались, что вы так думаете!",
                "Ответ");
        };

        var buttonNo = new Button() {
            AutoSize = true,
            Location = new Point(0, 80),
            Text = "Нет",
        };

        buttonNo.MouseMove += (_, e) => {
            buttonNo.Top -= e.Y;
            buttonNo.Left += e.X;
            if (buttonNo.Top < -10 || buttonNo.Top > 100) {
                buttonNo.Top = 60;
            }
            if (buttonNo.Left < -80 || buttonNo.Left > 250) {
                buttonNo.Left = 120;
            }
        };

        buttonNo.Click += delegate {
            Application.Exit();
        };

        Controls.Add(label);
        Controls.Add(buttonYes);
        Controls.Add(buttonNo);
    }
}
