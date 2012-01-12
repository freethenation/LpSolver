using System;
namespace LPSolver
{
	public interface ILPSolver : IDisposable
	{
		void AddConstraint(ConstraintTypes constraintType, double rhs, params double[] coefficients);
		int ConstraintCount { get; }
		double[] GetSolutionsConstraintsValues();
		double GetSolutionsObjectiveValue();
		double[] GetSolutionsVariablesValues();
		void SetObjective(params double[] row);
		void SetVariableBounds(int variableIndex, double lower, double upper);
		SolutionTypes Solve();
		SolutionTypes Solve(out string message);
		int VariableCount { get; }
	}
}
