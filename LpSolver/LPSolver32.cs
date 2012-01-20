using System;
using System.Collections.Generic;
using System.Text;

namespace LPSolver
{
	public class LPSolver32 : IDisposable, ILPSolver
	{
		private readonly int _id;
		public int VariableCount
		{
			get { return lpsolve32.get_Norig_columns(_id); }
		}
		public int ConstraintCount
		{
			get { return lpsolve32.get_Norig_rows(_id); }
		}
		internal LPSolver32(int numberVariables, bool maximize)
		{
			_id = lpsolve32.make_lp(0, numberVariables);
			if (maximize) lpsolve32.set_maxim(_id);
			else lpsolve32.set_minim(_id);
		}
		public void AddConstraint(ConstraintTypes constraintType, double rhs, params double[] coefficients)
		{
			double[] coeff = new double[coefficients.Length + 1];
			coefficients.CopyTo(coeff, 1);
			if (!lpsolve32.add_constraint(_id, coeff, constraintType, rhs)) throw new Exception();
		}
		public void SetVariableBounds(int variableIndex, double lower, double upper)
		{
			if (!lpsolve32.set_bounds(_id, variableIndex + 1, lower, upper)) throw new Exception();
		}
		string _outputFile = null;
		public string OutputFile
		{
			get
			{
				return _outputFile;
			}
			set
			{
				if (_outputFile != value)
				{
					lpsolve32.set_outputfile(_id, value);
					_outputFile = value;
				}
			}
		}
		public void SetObjective(params double[] coefficients)
		{
			double[] coeff = new double[coefficients.Length + 1];
			coefficients.CopyTo(coeff, 1);
			if (!lpsolve32.set_obj_fn(_id, coeff)) throw new Exception();
		}
		public double[] GetSolutionsVariablesValues()
		{
			double[] ret = new double[VariableCount];
			int offset = 1 + ConstraintCount;
			for (int i = 0; i < ret.Length; i++)
			{
				ret[i] = lpsolve32.get_var_primalresult(_id, i + offset);
			}
			return ret;
		}
		public double[] GetSolutionsConstraintsValues()
		{
			double[] ret = new double[ConstraintCount];
			for (int i = 0; i < ret.Length; i++)
			{
				ret[i] = lpsolve32.get_var_primalresult(_id, i + 1);
			}
			return ret;
		}
		public double GetSolutionsObjectiveValue()
		{
			return lpsolve32.get_var_primalresult(_id, 0);
		}
		public SolutionTypes Solve()
		{
			return lpsolve32.solve(_id);
		}
		public SolutionTypes Solve(out string message)
		{
			SolutionTypes status = Solve();
			message = lpsolve32.get_statustext(_id, (int)status);
			return status;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		~LPSolver32()
		{
			Dispose(false);
		}
		protected virtual void Dispose(bool disposing)
		{
			lpsolve32.delete_lp(_id);
		}
	}
}
