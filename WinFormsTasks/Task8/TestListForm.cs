using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WinFormsTasks.Common;

namespace WinFormsTasks.Task8;
[SelectableForm("TestList (Excercise 2)")]
public partial class TestListForm : Form {
    public TestListForm() {
        InitializeComponent();

        FormBorderStyle = FormBorderStyle.Fixed3D;
        Size = new Size(625, 650);
        Text = "Работа со списками";

        GroupBox leftContainer = MakeLeftContainer();
        StackPanel rightContainer = MakeRightContainer(
            ClientSize.Width,
            leftContainer.Top);
        CheckedListBox memberList = MakeMemberList();
        ComboBox peopleList = MakePeopleList();
        Button addButton = MakeAddButton();
        Button deleteButton = MakeDeleteButton();
        Button sortButton = MakeSortButton();

        SetAddButtonHandler(addButton, this, memberList, peopleList);
        SetDeleteButtonHandler(deleteButton, this, memberList);
        SetSortButtonHandler(sortButton, memberList);

        PopulatePeopleList(peopleList);
        PopulateLeftContainer(leftContainer, memberList);
        PopulateRightContainer(rightContainer, peopleList, addButton, deleteButton, sortButton);

        PopulateOwner(this, leftContainer, rightContainer);
    }

    private static void PopulateOwner(Form owner, GroupBox leftContainer, StackPanel rightContainer) {
        owner.Controls.AddRange(new Control[] {
            leftContainer,
            rightContainer,
        });
    }

    private static void PopulateRightContainer(
        StackPanel rightContainer,
        ComboBox peopleList,
        Button addButton,
        Button deleteButton,
        Button sortButton
    ) {
        rightContainer.Controls.AddRange(new Control[] {
            peopleList,
            addButton,
            deleteButton,
            sortButton,
        });
    }

    private static void PopulateLeftContainer(GroupBox leftContainer, CheckedListBox memberList) {
        leftContainer.Controls.Add(memberList);
    }

    private static void PopulatePeopleList(ComboBox peopleList) {
        peopleList.Items.AddRange(new[] {
            "Participant 1",
            "Participant 2",
            "Participant 3",
            "Participant 4",
            "Participant 5",
            "Participant 6",
        });
    }

    private static void SetSortButtonHandler(Button sortButton, CheckedListBox memberList) {
        sortButton.Click += delegate {
            if (memberList.Items.Count < 1) {
                return;
            }
            var sorted = memberList.Items
                .Cast<string>()
                .Order()
                .ToArray();
            for (int i = 0; i < memberList.Items.Count; i++) {
                memberList.Items[i] = sorted[i];
            }
        };
    }

    private static void SetDeleteButtonHandler(Button deleteButton, Form owner, CheckedListBox memberList) {
        deleteButton.Click += delegate {
            var checkedIndices = memberList.CheckedIndices;
            if (checkedIndices.Count < 1) {
                ShowInvalidActionMessageBox(
                    owner: owner,
                    text: "Отметьте удаляемые элементы в списке.",
                    caption: "Не удалось удалить элементы");
                return;
            }
            while (checkedIndices.Count > 0) {
                memberList.Items.RemoveAt(checkedIndices[checkedIndices.Count - 1]);
            }
        };
    }

    private static void SetAddButtonHandler(Button addButton, Form owner, CheckedListBox memberList, ComboBox peopleList) {
        addButton.Click += delegate {
            var personIndex = peopleList.SelectedIndex;
            object? entry = null;
            if (personIndex != -1) {
                entry = peopleList.Items[personIndex];
                peopleList.Items.RemoveAt(personIndex);
            } else if (!string.IsNullOrWhiteSpace(peopleList.Text)) {
                entry = peopleList.Text;
            } else {
                ShowInvalidActionMessageBox(
                    owner: owner,
                    text: "Выберите элемент из списка или введите новый.",
                    caption: "Не удалось добавить элемент");
            }
            if (entry is not null and string entryString) {
                entry = entryString.Trim();
                memberList.Items.Add(entry);
            }
            peopleList.ResetText();
        };
    }

    private static Button MakeSortButton() =>
        new Button() {
            AutoSize = true,
            Name = "sortButton",
            Text = "Сортировать",
        };

    private static Button MakeDeleteButton() =>
        new Button() {
            AutoSize = true,
            Name = "deleteButton",
            Text = "Удалить",
        };

    private static Button MakeAddButton() =>
        new Button() {
            AutoSize = true,
            Name = "addButton",
            Text = "Добавить",
        };

    private static ComboBox MakePeopleList() =>
        new ComboBox() {
            Name = "peopleList",
            Text = string.Empty,
        };

    private static CheckedListBox MakeMemberList() =>
        new CheckedListBox() {
            AutoSize = true,
            Dock = DockStyle.Fill,
            Name = "memberList",
        };

    private static StackPanel MakeRightContainer(int ownerClientSizeWidth, int leftContainerTop) =>
        new StackPanel() {
            Size = new Size(290, 600),
            Location = new Point(
                ownerClientSizeWidth - 290 - 10,
                leftContainerTop + 20),
        };

    private static GroupBox MakeLeftContainer() =>
        new GroupBox() {
            AutoSize = false,
            Size = new Size(290, 600),
            Text = "Список участников",
        };

    private static void ShowInvalidActionMessageBox(Form owner, string text, string caption) =>
        MessageBox.Show(
            owner: owner,
            text: text,
            caption: caption,
            buttons: MessageBoxButtons.OK,
            icon: MessageBoxIcon.Information);
}
