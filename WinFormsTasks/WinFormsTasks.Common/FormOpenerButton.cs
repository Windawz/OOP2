using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
public class FormOpenerButton : Button {
    public FormOpenerButton(Func<Form> factory) {
        FormFactory = factory;
        OpenedFormType = FormFactory.Method.ReturnType;

        Click += OnClick;
    }

    private Form? _cachedOwnerForm = null;

    public Type OpenedFormType { get; }
    protected Func<Form> FormFactory { get; }
    protected Form OwnerForm {
        get {
            _cachedOwnerForm ??= FindForm();
            return _cachedOwnerForm!;
        }
    }

    protected virtual void OnClick(object? sender, EventArgs e) {
        Click -= OnClick;
        
        var form = FormFactory();
        form.FormClosing += delegate {
            Click += OnClick;
        };

        OwnerForm.AddOwnedForm(form);
        form.Show();
        form.Activate();
    }
}
