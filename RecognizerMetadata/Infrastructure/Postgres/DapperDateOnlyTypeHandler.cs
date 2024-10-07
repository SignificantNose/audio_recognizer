using System;
using System.Data;
using Dapper;

namespace Infrastructure;

public class DapperDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
{
    public override DateOnly Parse(object value)
    {
        return DateOnly.FromDateTime((DateTime)value);
    }

    public override void SetValue(IDbDataParameter parameter, DateOnly value)
    {
        parameter.Value = value.ToDateTime(new TimeOnly(0,0));
    }
}
