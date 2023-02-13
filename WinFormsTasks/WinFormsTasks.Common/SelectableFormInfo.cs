using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
internal readonly struct SelectableFormInfo {
    public SelectableFormInfo(FormFactory factory, string? formName, int weight) {
        Factory = factory;
        FormName = formName;
        Weight = weight;
    }

    public FormFactory Factory { get; }
    public string? FormName { get; }
    public int Weight { get; }

    public static IEnumerable<SelectableFormInfo> EnumerateSelectableForms(Assembly assembly) {
        var targets = EnumerateTargets(assembly);
        var invalidTargets = EnumerateInvalidTargets(targets);
        ThrowIfAnyInvalidTargets(invalidTargets);
        var validTargets = EnumerateValidTargets(targets, invalidTargets);
        return validTargets.Select(target => Create(
            target.Type,
            target.Attribute));
    }

    public static SelectableFormInfo Create(Type formType) {
        var attribute = formType.GetCustomAttribute<SelectableFormAttribute>();
        if (attribute is null) {
            throw new ArgumentException($"Type has no {nameof(SelectableFormAttribute)}", nameof(formType));
        }
        return Create(formType, attribute);
    }

    public static SelectableFormInfo Create(Type formType, SelectableFormAttribute attribute) =>
        Create(formType, attribute.FormName, attribute.Weight);

    public static SelectableFormInfo Create(Type formType, string? formName, int weight) {
        var exception = GetExceptionIfInvalidType(formType);

        if (exception is not null) {
            throw new ArgumentException(
                $"Invalid form type",
                nameof(formType),
                exception);
        }

        return new SelectableFormInfo(
            FormFactory.Get(formType),
            formName,
            weight);
    }

    private static IEnumerable<Target> EnumerateValidTargets(
        IEnumerable<Target> targets,
        IEnumerable<InvalidTarget> invalidTargets
    ) =>
        targets.ExceptBy(invalidTargets.Select(target => target.Type), target => target.Type);

    private static void ThrowIfAnyInvalidTargets(IEnumerable<InvalidTarget> invalidTargets) {
        if (invalidTargets.Any()) {
            throw invalidTargets.First().Exception;
        }
    }

    private static IEnumerable<InvalidTarget> EnumerateInvalidTargets(
        IEnumerable<Target> targets
    ) =>
        targets.Select(target => (type: target.Type, exception: GetExceptionIfInvalidType(target.Type)))
            .Where(target => target.exception is not null)
            .Select(target => new InvalidTarget(target.type, target.exception!));

    private static SelectableTargetException? GetExceptionIfInvalidType(Type formType) =>
        !FormType.IsFormType(formType)
            ? new SelectableTargetTypeException(formType)
            : !HasValidConstructor(formType)
                ? new SelectableTargetFactoryException(formType)
                : null;

    private static bool HasValidConstructor(Type formType) =>
        formType.GetConstructor(Array.Empty<Type>()) is not null;

    private static IEnumerable<Target> EnumerateTargets(Assembly assembly) =>
        assembly.ExportedTypes
            .Select(type => (
                type: type,
                attribute: type.GetCustomAttribute<SelectableFormAttribute>()))
            .Where(target => target.attribute is not null)
            .Select(target => new Target(target.type, target.attribute!));

    private readonly record struct Target(
        Type Type,
        SelectableFormAttribute Attribute);

    private readonly record struct InvalidTarget(
        Type Type,
        SelectableTargetException Exception);
};
