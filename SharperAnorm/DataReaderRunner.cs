using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace SharperAnorm
{
    public class DataReaderRunner : Runner<IDataRecord>
    {
        public DataReaderRunner(Func<Task<DbConnection>> connectionProvider, Func<DbConnection, Task> connectionDisposer) : base(connectionProvider, connectionDisposer)
        {
        }

        protected override IEnumerator<T> Enumerate<T>(DbDataReader result, RowParser<T, IDataRecord> parser)
        {
            return new DataReaderParsingEnumerator<T>(result, parser);
        }

        protected override async Task<T> Single<T>(DbDataReader result, RowParser<T, IDataRecord> parser, CancellationToken ct)
        {
            var success = await result.ReadAsync(ct);
            if (!success)
            {
                throw new DataException("Expected a single value, but got nothing!");
            }

            var parseResult = parser.Parse(result).Value;

            success = await result.ReadAsync(ct);
            if (success)
            {
                throw new DataException("Expected a single value, but got more than that!");
            }

            return parseResult;
        }
    }
}