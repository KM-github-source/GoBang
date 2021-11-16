using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client.Algorithm {
	public class GoBang {
		protected List<Position> History;
		public GoBang(uint row, uint col, Player offensive = Player.Black) {
			RowCount = row;
			ColCount = col;
			Offensive = offensive;
			Winner = Player.None;
			Situation = new Player[row, col];
			History = new();
			for (int i = 0; i < row; ++i)
				for (int j = 0; j < col; ++j)
					Situation[i, j] = Player.None;
		}
		public uint RowCount { get; init; }
		public uint ColCount { get; init; }
		public uint MoveCount { get => (uint)History.Count; }
		public Player Offensive { get; init; }
		public Player Winner { get; private set; }
		public bool Finished { get => Winner != Player.None; }
		public Player CurrentPlayer {
			get => (MoveCount + (uint)Offensive & 1) == 1 ? Player.White : Player.Black;
		}
		public Player[,] Situation { protected get; init; }
		public Player this[uint row, uint col] {
			get => Situation[row, col];
			protected set => Situation[row, col] = value;
		}
		public Player this[int row, int col] {
			get => Situation[row, col];
			protected set => Situation[row, col] = value;
		}
		public void Move(uint row, uint col) {
			if (row >= RowCount || col >= ColCount)
				throw new ArgumentOutOfRangeException(row >= RowCount ? nameof(row) : nameof(col), "Placement position out of range");
			else if (Finished)
				throw new InvalidOperationException("Game has finished");
			var curPlayer = CurrentPlayer;
			Situation[row, col] = curPlayer;
			int sequenceCount = 1;
			for (uint i = col - 1; i >= 0 && Situation[row, i] == curPlayer; --i, ++sequenceCount) ;
			for (uint i = col + 1; i < ColCount && Situation[row, i] == curPlayer; ++i, ++sequenceCount) ;
			if (sequenceCount >= 5)
				goto Finish;
			sequenceCount = 1;
			for (uint i = row - 1; i >= 0 && Situation[i, col] == curPlayer; --i, ++sequenceCount) ;
			for (uint i = row + 1; i < RowCount && Situation[i, col] == curPlayer; ++i, ++sequenceCount) ;
			if (sequenceCount >= 5)
				goto Finish;
			sequenceCount = 1;
			for (uint i = row - 1, j = col - 1; i >= 0 && j >= 0 && Situation[i, j] == curPlayer; --i, --j, ++sequenceCount) ;
			for (uint i = row + 1, j = col + 1; i < RowCount && j < ColCount && Situation[i, j] == curPlayer; ++i, ++j, ++sequenceCount) ;
			if (sequenceCount >= 5)
				goto Finish;
			sequenceCount = 1;
			for (uint i = row - 1, j = col + 1; i >= 0 && j < ColCount && Situation[i, j] == curPlayer; --i, ++j, ++sequenceCount) ;
			for (uint i = row + 1, j = col - 1; i < RowCount && j >= 0 && Situation[i, j] == curPlayer; ++i, --j, ++sequenceCount) ;
			Finish:
			if (sequenceCount >= 5)
				Winner = curPlayer;
			History.Add(new(row, col));
		}
		public void Move(Position pos) => Move(pos.Row, pos.Col);
		public Position Retract() {
			if (History.Count == 0)
				throw new InvalidOperationException("No chesspieces are placed");
			var lastStep = History[^1];
			History.RemoveAt(History.Count - 1);
			return lastStep;
		}
		public enum Player : byte {
			None = 0,
			White = 1,
			Black = 2
		}
	}
	public class Position {
		public uint Row;
		public uint Col;
		public Position(uint row, uint col) {
			Row = row;
			Col = col;
		}
	}
	public static class GoBangUtility {
		public static IEnumerable<Position> Transform(this GoBang goBang) {
			for (uint i = 0; i < goBang.RowCount; ++i)
				for (uint j = 0; j < goBang.ColCount; ++j)
					if (goBang[i, j] == GoBang.Player.None)
						yield return new(i, j);
		}
		public static GoBang Apply(this GoBang goBang, Position pos) {
			goBang.Move(pos);
			return goBang;
		}
	}
}
