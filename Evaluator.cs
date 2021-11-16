using System;

uint tuple6type[4][4][4][4][4][4];

namespace Client.Algorithm {
	public delegate TValue Evaluator<TGame, TValue>(TGame game);
	/// <summary>
	/// Some predefined evaluation functions
	/// </summary>
    public enum ChessType : byte {
		OTHER = 0,
		WIN = 1,
		LOSE = 2,
		FLEX4 = 3,
		flex4 = 4,
		BLOCK4 = 5,
		block4 = 6,
		FLEX3 = 7,
		flex3 = 8,
		BLOCK3 = 9,
		block3 = 10,
		FLEX2 = 11,
		flex2 = 12,
		BLOCK2 = 13,
		block2 = 14,
		FLEX1 = 15,
		flex1 = 16
    }
	public static class Evaluators {
		public void Inittuple6type
        {

        }
	}
}
