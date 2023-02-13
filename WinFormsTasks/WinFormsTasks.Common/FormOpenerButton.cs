using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
public class FormOpenerButton : Button {
    public FormOpenerButton(Type formType) : this(FormFactory.Get(formType)) { }

    internal FormOpenerButton(FormFactory factory) {
        _formFactory = factory;

        Click += OnClick;
    }

    private readonly FormFactory _formFactory;
    private Form? _cachedOwnerForm = null;

    public Type FormType => _formFactory.FormType;

    public event EventHandler<FormOpenedEventArgs>? FormOpened;

    private void OnClick(object? sender, EventArgs e) {
        Click -= OnClick;
        
        var form = _formFactory.Make();
        form.FormClosing += delegate {
            Click += OnClick;
        };

        GetCachedOwnerForm().AddOwnedForm(form);
        form.Show();
        form.Activate();

        FormOpened?.Invoke(this, new FormOpenedEventArgs(form));
    }

    private Form GetCachedOwnerForm() {
        _cachedOwnerForm ??= FindForm();
        return _cachedOwnerForm!;
    }

    public class FormOpenedEventArgs : EventArgs {
        public FormOpenedEventArgs(Form form) {
            Form = form;
        }

        public Form Form { get; }
    };
}
