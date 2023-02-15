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
[SelectableForm("Container Form (Excercise 5)", 5)]
public partial class WinContainerForm : Form {
    public WinContainerForm() {
        InitializeComponent();

        var tabControl = new TabControl() {
            Dock = DockStyle.Fill,
        };
        Controls.Add(tabControl);

        var groupBoxPage = new TabPage("GroupBox");
        var panelPage = new TabPage("Panel");
        var flowLayoutPanelPage = new TabPage("FlowLayoutPanel");
        var tableLayoutPanelPage = new TabPage("TableLayoutPanel");
        var splitContainerPage = new TabPage("SplitContainer");
        tabControl.TabPages.AddRange(new[] {
            groupBoxPage,
            panelPage,
            flowLayoutPanelPage,
            tableLayoutPanelPage,
            splitContainerPage,
        });

        
        {
            var groupBox = new GroupBox() { };
            groupBoxPage.Controls.Add(groupBox);

            var firstRadioButton = new RadioButton() {
                AutoSize = true,
                Location = new(10, 20),
                Text = "First radio button",
            };
            groupBox.Controls.Add(firstRadioButton);

            var secondRadioButton = new RadioButton() {
                AutoSize = true,
                Location = new(10, 60),
                Text = "Second radio button",
            };
            groupBox.Controls.Add(secondRadioButton);

            var button = new Button() {
                AutoSize = true,
                Location = new(groupBox.Left, groupBox.Top + groupBox.Height + 20),
                Text = "Undefined",
            };
            button.Click += delegate {
                if (firstRadioButton.Checked) {
                    button.Text = "First";
                } else if (secondRadioButton.Checked) {
                    button.Text = "Second";
                }
            };
            groupBoxPage.Controls.Add(button);
        }

        {
            var panel = new Panel() {
                AutoScroll = true,
                Dock = DockStyle.Fill,
            };
            panelPage.Controls.Add(panel);

            var buttons = Enumerable.Range(0, 4)
                .Select(i => {
                    return new Button() {
                        AutoSize = true,
                        Location = new(10, 10 + 40 * i),
                        Text = "Button",
                    };
                })
                .ToArray();
            panel.Controls.AddRange(buttons);
        }

        {
            var panel = new FlowLayoutPanel() {
                Dock = DockStyle.Fill,
            };
            flowLayoutPanelPage.Controls.Add(panel);

            var buttons = Enumerable.Range(0, 4)
                .Select(i => {
                    return new Button() {
                        AutoSize = true,
                        Text = "Button",
                    };
                })
                .ToArray();
            buttons[buttons.Length - 2].Click += delegate {
                panel.SetFlowBreak(buttons[buttons.Length - 1], true);
            };
            panel.Controls.AddRange(buttons);
        }

        {
            var panel = new TableLayoutPanel() {
                AutoScroll = true,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Inset,
                Dock = DockStyle.Fill,
            };
            tableLayoutPanelPage.Controls.Add(panel);

            var button = new Button() {
                AutoSize = true,
                Text = "First Button",
            };
            void AddAnotherButton(object? sender, EventArgs e) {
                var anotherButton = new Button() {
                    AutoSize = true,
                    Text = "Another Button",
                };
                panel!.Controls.Add(anotherButton, 1, 1);
                button.Click -= AddAnotherButton;
            }
            button.Click += AddAnotherButton;
            panel.Controls.Add(button, 0, 0);
        }

        {
            var container = new SplitContainer() {
                BorderStyle = BorderStyle.Fixed3D,
            };
            splitContainerPage.Controls.Add(container);

            var panel1Button = new Button() {
                AutoSize = true,
                Text = "Fix/Unfix Panel1",
            };
            panel1Button.Click += delegate {
                if (container.FixedPanel == FixedPanel.Panel1) {
                    container.FixedPanel = FixedPanel.None;
                } else {
                    container.FixedPanel = FixedPanel.Panel1;
                }
            };
            container.Panel1.Controls.Add(panel1Button);

            var splitterButton = new Button() {
                AutoSize = true,
                Text = "Fix/Unfix Splitter",
            };
            splitterButton.Click += delegate {
                container.IsSplitterFixed = !container.IsSplitterFixed;
            };
            container.Panel1.Controls.Add(splitterButton);

            var panel2Button = new Button() {
                AutoSize = true,
                Text = "Collapse/Uncollapse Panel1",
            };
            panel2Button.Click += delegate {
                container.Panel1Collapsed = !container.Panel1Collapsed;
            };
            container.Panel2.Controls.Add(panel2Button);
        }
    }
}
