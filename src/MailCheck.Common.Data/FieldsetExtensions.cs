using System;
using System.Linq;

namespace MailCheck.Common.Data
{
    public static class FieldsetExtensions
    {
        public static string ToSumSql(this Fieldset fields)
        {
            return string.Join(" + ", fields.Append("0"));
        }

        public static string ToFieldListSql(this Fieldset fields)
        {
            return string.Join(", ", fields);
        }

        public static string ToAliasedFieldListSql(this Fieldset fields, Func<string, int, string> aliasingFunction = null)
        {
            aliasingFunction = aliasingFunction ?? ((f, i) => $"{f} as col{i}");
            var aliasedFields = new Fieldset(fields.Select(aliasingFunction));
            return aliasedFields.ToFieldListSql();
        }

        public static string ToValuesParameterListSql(this Fieldset fields, int numRecords)
        {
            if (fields.Count == 0 || numRecords == 0) return string.Empty;

            var rows = Enumerable
                .Range(0, numRecords)
                .GroupJoin(
                    fields,
                    _ => true,
                    _ => true,
                    (rowNum, fieldsNames) => string.Join(", ", fieldsNames.Select(name => $"@{name}_{rowNum}"))
                );

            return $"( {string.Join(" ), ( ", rows)} )";
        }

        public static string ToAliasedParameterListSql(this Fieldset fields)
        {
            if (fields.Count == 0) return string.Empty;

            return string.Join(", ", fields.Select(name => $"@{name} as `{name}`"));
        }
    }
}