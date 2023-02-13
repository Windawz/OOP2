using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WinFormsTimer = System.Windows.Forms.Timer;

namespace WinFormsTasks.Common;
public class Glitch : IDisposable {
    public Glitch(Form mainForm) {
        _mainForm = mainForm;
        _factory = FormFactory.Get(_mainForm.GetType());
    }

    private readonly Form _mainForm;
    private readonly FormFactory _factory;
    private readonly List<Instance> _instances = new();

    public static void Attach(Form form) {
        var glitch = new Glitch(form);
        form.FormClosing += delegate {
            glitch.Dispose();
        };
        glitch.Start();
    }

    public static void Attach(SelectorForm selectorForm, Type attachedFormType) {
        if (!attachedFormType.IsAssignableTo(typeof(Form))) {
            throw new ArgumentException(
                $"{nameof(attachedFormType)} must be a type derived from {nameof(Form)}",
                nameof(attachedFormType));
        }
        FormOpenerButton? button = selectorForm
            .OpenerButtons
            .Where(b => b.FormType == attachedFormType)
            .SingleOrDefault();
        if (button is null) {
            throw new ArgumentException(
                "No button found on selector form for specified form type",
                nameof(attachedFormType));
        }
        button.FormOpened += (_, e) => Attach(e.Form);
    }

    public void Start() {
        CreateInstance(_mainForm, false);
    }

    public void Dispose() {
        while (_instances.Count >= 1) {
            var instance = _instances[_instances.Count - 1];
            DestroyInstance(instance);
        }
    }

    private void CreateInstance(Form form, bool ownsForm) {
        var instance = new Instance(
            form,
            ownsForm,
            onDie: (instance) => {
                DestroyInstance(instance);
            },
            onDuplicate: () => {
                CreateInstance(_factory.Make(), true);
            });
        _instances.Add(instance);
        instance.Start();
    }

    private void DestroyInstance(Instance instance) {
        if (_instances.Remove(instance)) {
            instance.Dispose();
        }
    }

    private class Instance : IDisposable {
        private const int TickInterval = 25;

        public Instance(Form form, bool ownsForm, Action<Instance> onDie, Action onDuplicate) {
            _effect = MakeEffect(this, form, onDie, onDuplicate);
            _form = form;
            _timer = MakeTimer(_effect);
            _ownsForm = ownsForm;
        }

        private readonly Effect _effect;
        private readonly Form _form;
        private readonly WinFormsTimer _timer;
        private readonly bool _ownsForm;
        private bool _disposed = false;

        public void Start() {
            _form.Show();
            _form.Activate();
            _timer.Start();
        }

        public void Dispose() {
            if (!_disposed) {
                _effect.Dispose();
                if (_ownsForm) {
                    _form.Close();
                }
                _timer.Stop();
                _timer.Dispose();
                _disposed = true;
            }
        }

        private static Effect MakeEffect(Instance instance, Form form, Action<Instance> onDie, Action onDuplicate) {
            var effect = new Effect(form);
            effect.MustDie += delegate {
                onDie(instance);
            };
            effect.MustDuplicate += delegate {
                onDuplicate();
            };
            return effect;
        }

        private static WinFormsTimer MakeTimer(Effect effect) {
            var timer = new WinFormsTimer() {
                Interval = TickInterval,
            };
            timer.Tick += delegate {
                effect.Apply();
            };
            return timer;
        }
    }

    private class Effect : IDisposable {
        private const int MinLocationDeltaX = -100;
        private const int MaxLocationDeltaX = 300;
        private const int MinLocationDeltaY = -100;
        private const int MaxLocationDeltaY = 300;
        private const int MinSizeWidth = 100;
        private const int MaxSizeWidth = 1000;
        private const int MinSizeHeight = 75;
        private const int MaxSizeHeight = 700;
        private static double DieProbability = 0.0045;
        private static double DuplicateProbability = 0.009;

        public Effect(Form form) {
            _savedLocation = form.Location;
            _savedSize = form.Size;
            _savedBackColor = form.BackColor;
            _savedOpacity = form.Opacity;
            _savedFormBorderStyle = form.FormBorderStyle;
            _form = form;
        }

        private static readonly FormBorderStyle[] _formBorderStyles =
            Enum.GetValues<FormBorderStyle>();
        private readonly Point _savedLocation;
        private readonly Size _savedSize;
        private readonly Color _savedBackColor;
        private readonly double _savedOpacity;
        private readonly FormBorderStyle _savedFormBorderStyle;
        private readonly Form _form;

        public event EventHandler? MustDie;
        public event EventHandler? MustDuplicate;

        public void Apply() {
            var random = Random.Shared;

            _form.Location = GetRandomLocation(random);
            _form.Size = GetRandomSize(random);
            _form.BackColor = GetRandomBackColor(random);
            _form.Opacity = GetRandomOpacity(random);
            _form.FormBorderStyle = GetRandomFormBorderStyle(random);

            if (ShouldRandomlyDie(random)) {
                MustDie?.Invoke(this, EventArgs.Empty);
            }
            if (ShouldRandomlyDuplicate(random)) {
                MustDuplicate?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Dispose() {
            _form.Location = _savedLocation;
            _form.Size = _savedSize;
            _form.BackColor = _savedBackColor;
            _form.Opacity = _savedOpacity;
            _form.FormBorderStyle = _savedFormBorderStyle;
        }

        private static Point GetRandomLocation(Random random) =>
            new Point(
                random.Next(MinLocationDeltaX, MaxLocationDeltaX),
                random.Next(MinLocationDeltaY, MaxLocationDeltaY));

        private static Size GetRandomSize(Random random) =>
            new Size(
                random.Next(MinSizeWidth, MaxSizeWidth),
                random.Next(MinSizeHeight, MaxSizeHeight));

        private static Color GetRandomBackColor(Random random) =>
            Color.FromArgb(
                random.Next(0, 255),
                random.Next(0, 255),
                random.Next(0, 255));

        private static double GetRandomOpacity(Random random) =>
            random.NextDouble();

        private static FormBorderStyle GetRandomFormBorderStyle(Random random) =>
            _formBorderStyles[random.Next(0, _formBorderStyles.Length - 1)];

        private static bool ShouldRandomlyDie(Random random) =>
            random.NextDouble() < DieProbability;

        private static bool ShouldRandomlyDuplicate(Random random) =>
            random.NextDouble() < DuplicateProbability;
    }
}
