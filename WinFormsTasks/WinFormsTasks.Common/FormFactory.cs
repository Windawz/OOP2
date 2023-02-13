using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
internal class FormFactory {
    private FormFactory(Func<Form> factoryMethod, Type formType) {
        _factoryMethod = factoryMethod;
        FormType = formType;
    }

    private readonly Func<Form> _factoryMethod;

    public Type FormType { get; }

    public static FormFactory Get(Type formType) =>
        new(GetFactoryMethod(formType), formType);

    public Form Make() =>
        _factoryMethod();

    private static Func<Form> GetFactoryMethod(Type formType) =>
        Expression.Lambda<Func<Form>>(
            Expression.New(formType),
            Array.Empty<ParameterExpression>())
        .Compile();
}
