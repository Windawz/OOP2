using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
internal static class SelectableFormLookup {
    public static IEnumerable<SelectableFormInfo> EnumerateFormInfos(Assembly assembly) {
        var targets = EnumerateTargets(assembly);
        var invalidTargets = EnumerateInvalidTargets(targets);
        ThrowIfAnyInvalidTargets(invalidTargets);
        var validTargets = EnumerateValidTargets(targets, invalidTargets);
        return EnumerateFormInfos(validTargets);
    }

    private static IEnumerable<SelectableFormInfo> EnumerateFormInfos(IEnumerable<Target> targets) =>
        targets.Select(target => new SelectableFormInfo(
            Factory: GetFactory(target.Type),
            FormName: target.Attribute.FormName,
            FormType: target.Type));

    private static Func<Form> GetFactory(Type targetType) =>
        Expression.Lambda<Func<Form>>(
            Expression.New(targetType),
            Array.Empty<ParameterExpression>())
        .Compile();

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
        targets.Select(target => (type: target.Type, exception: GetExceptionIfInvalidTarget(target.Type)))
            .Where(target => target.exception is not null)
            .Select(target => new InvalidTarget(target.type, target.exception!));

    private static SelectableTargetException? GetExceptionIfInvalidTarget(Type targetType) =>
        !IsValidTargetType(targetType)
            ? new SelectableTargetTypeException(targetType)
            : !HasValidConstructor(targetType)
                ? new SelectableTargetFactoryException(targetType)
                : null;

    private static bool HasValidConstructor(Type targetType) =>
        targetType.GetConstructor(Array.Empty<Type>()) is not null;

    private static bool IsValidTargetType(Type targetType) =>
        targetType.IsAssignableTo(typeof(Form));

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
}
