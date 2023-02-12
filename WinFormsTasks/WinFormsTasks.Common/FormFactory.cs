using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsTasks.Common;
internal static class FormFactory {
    public static Func<Form> Get(Type formType) =>
        Expression.Lambda<Func<Form>>(
            Expression.New(formType),
            Array.Empty<ParameterExpression>())
        .Compile();
}
