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

    private static IEnumerable<SelectableFormInfo> EnumerateFormInfos(IEnumerable<(Type type, SelectableFormAttribute attribute)> targets) =>
        targets.Select(target => new SelectableFormInfo(
            Factory: GetFactory(target.type),
            FormName: target.attribute.FormName ?? GetFormName(target.type)));

    private static string GetFormName(Type targetType) =>
        targetType.Name;

    private static Func<Form> GetFactory(Type targetType) =>
        Expression.Lambda<Func<Form>>(
            Expression.New(targetType),
            Array.Empty<ParameterExpression>())
        .Compile();

    private static IEnumerable<(Type type, SelectableFormAttribute attribute)> EnumerateValidTargets(
        IEnumerable<(Type type, SelectableFormAttribute attribute)> targets,
        IEnumerable<(Type type, SelectableTargetException exception)> invalidTargets
    ) =>
        targets.ExceptBy(invalidTargets.Select(target => target.type), target => target.type);

    private static void ThrowIfAnyInvalidTargets(IEnumerable<(Type type, SelectableTargetException exception)> invalidTargets) {
        if (invalidTargets.Any()) {
            throw invalidTargets.First().exception;
        }
    }

    private static IEnumerable<(Type targetType, SelectableTargetException exception)> EnumerateInvalidTargets(
        IEnumerable<(Type type, SelectableFormAttribute attribute)> targets
    ) =>
        targets.Select(target => (type: target.type, exception: GetExceptionIfInvalidTarget(target.type)))
            .Where(target => target.exception is not null)
            .Select(target => (type: target.type, exception: target.exception!));

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

    private static IEnumerable<(Type, SelectableFormAttribute)> EnumerateTargets(Assembly assembly) =>
        assembly.ExportedTypes
            .Select(type => (
                type: type,
                attribute: type.GetCustomAttribute<SelectableFormAttribute>()))
            .Where(target => target.attribute is not null)
            .Select(target => (target.type, attribute: (SelectableFormAttribute)target.attribute!));
}
