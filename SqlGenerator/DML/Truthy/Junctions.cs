using System.Collections.Generic;

namespace SqlGenerator.DML.Truthy
{
    public class Junctions : ITruthy
    {
        public JunctionOp Op { get; }
        public IEnumerable<ITruthy> Truthies { get; }

        public Junctions(JunctionOp op, params ITruthy[] truthies)
        {
            Op = op;
            Truthies = truthies;
        }
    }
}