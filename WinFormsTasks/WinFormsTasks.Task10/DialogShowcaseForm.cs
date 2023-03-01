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

namespace WinFormsTasks.Task10;
[SelectableForm]
public partial class DialogShowcaseForm : Form {
    private const string FileFilterString = "Text files (*.txt)|*.txt";

    public DialogShowcaseForm() {
        InitializeComponent();

        var container = new ToolStripContainer() {
            Dock = DockStyle.Fill,
        };
        Controls.Add(container);

        var richTextBox = new RichTextBox() {
            Dock = DockStyle.Fill,
        };
        container.ContentPanel.Controls.Add(richTextBox);

        var menu = new MenuStrip() {
            Dock = DockStyle.Top,
        };
        container.TopToolStripPanel.Controls.Add(menu);

        var fileSubmenu = new ToolStripMenuItem("Файл");
        menu.Items.Add(fileSubmenu);

        var formatSubmenu = new ToolStripMenuItem("Формат");
        menu.Items.Add(formatSubmenu);

        var saveAsButton = new ToolStripButton("Сохранить как...") {
            Width = 100,
        };
        saveAsButton.Click += delegate {
            var dialog = new SaveFileDialog() {
                Filter = FileFilterString,
            };
            DialogResult result = dialog.ShowDialog();
            if (result is DialogResult.OK) {
                string fullName = dialog.FileName;
                if (!string.IsNullOrWhiteSpace(fullName)) {
                    richTextBox.SaveFile(
                        dialog.FileName,
                        RichTextBoxStreamType.PlainText);
                } else {
                    MessageBox.Show(
                        this,
                        $"File name \"{fullName}\" is not allowed",
                        "Invalid File Name",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        };
        fileSubmenu.DropDownItems.Add(saveAsButton);

        var openFileButton = new ToolStripButton("Открыть...");
        openFileButton.Click += delegate {
            var dialog = new OpenFileDialog() {
                InitialDirectory = @"C:\",
                Filter = FileFilterString,
            };

            DialogResult result = dialog.ShowDialog();

            if (result is DialogResult.OK) {
                try {
                    using var stream = dialog.OpenFile();
                    richTextBox.LoadFile(stream, RichTextBoxStreamType.PlainText);
                } catch (Exception ex) {
                    MessageBox.Show(
                        this,
                        $"Could not read file from disk. Error message: \"{ex.Message}\"",
                        "Failed To Open File",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        };
        fileSubmenu.DropDownItems.Add(openFileButton);

        var backColorButton = new ToolStripButton("Цвет фона...") {
            Width = 100,
        };
        backColorButton.Click += delegate {
            var dialog = new ColorDialog();
            DialogResult result = dialog.ShowDialog();
            if (result is DialogResult.OK) {
                richTextBox.BackColor = dialog.Color;
            }
        };
        formatSubmenu.DropDownItems.Add(backColorButton);

        var fontButton = new ToolStripButton("Шрифт...");
        fontButton.Click += delegate {
            var dialog = new FontDialog() {
                Font = richTextBox.Font,
            };
            DialogResult result = dialog.ShowDialog();
            if (result is DialogResult.OK) {
                richTextBox.Font = dialog.Font;
            }
        };
        formatSubmenu.DropDownItems.Add(fontButton);
    }
}
