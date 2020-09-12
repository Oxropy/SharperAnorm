using System;

namespace SqlGenerator.DML.Truthy
{
    public class Junction : ITruthy
    {
        public ITruthy Lhs { get; }
        public JunctionOp Op { get; }
        public ITruthy Rhs { get; }

        public Junction(ITruthy lhs, JunctionOp op, ITruthy rhs)
        {
            Lhs = lhs;
            Op = op;
            Rhs = rhs;
        }


        public static string GetOperatorValue(JunctionOp op)
        {
            return op switch
            {
                JunctionOp.And => "AND",
                JunctionOp.Or => "OR",
                _ => throw new NotSupportedException("Unknown operator!")
            };
        }
    }
}