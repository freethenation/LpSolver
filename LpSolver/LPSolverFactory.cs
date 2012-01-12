using System;
using System.Collections.Generic;
using System.Text;

namespace LPSolver
{
	public class LPSolverFactory
	{
		public static ILPSolver CreateLPSolver(int numberVariables, bool maximize)
		{
			if (IntPtr.Size == 4)//32bit
			{
				return new LPSolver32(numberVariables, maximize);
			}
			else//64bit
			{
				throw new System.NotImplementedException("64bit version is not yet implemented");
			}
		}
	}
}
