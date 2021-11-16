using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Linq;

namespace Client.Algorithm {
	/// <summary>
	/// The container class that preserves the details during search process
	/// </summary>
	/// <typeparam name="TGame">A type that presents the state</typeparam>
	/// <typeparam name="TMove">A type that presents a game move</typeparam>
	/// <typeparam name="TValue">A type that presents the value of a game situation. Should support + and - operators</typeparam>
	public class Search<TGame, TMove, TValue> where TGame : class where TValue : struct, IComparable<TValue> {
		#region Constructors
		public Search() => Path = new();
		public Search(TGame src, Transformer transformer, Applier applier) : this() {
			Source = src;
			Transform = transformer;
			Apply = applier;
		}
		#endregion

		#region Delegates
		public delegate IEnumerable<TMove> Transformer(TGame game);
		public delegate TGame Applier(TGame game, TMove move);
		#endregion

		#region Properties
		/// <summary>
		/// Starting point of the search
		/// </summary>
		public TGame Source { get; set; }
		/// <summary>
		/// Search node of the source. Could be used to render a search tree.
		/// </summary>
		public Node SearchSource { get; private set; }
		/// <summary>
		/// Search node of the destination. Null if no paths were found.
		/// </summary>
		public Node SearchDestination { get; private set; }
		/// <summary>
		/// A function that presents the transforming action of each state
		/// </summary>
		public Transformer Transform { get; init; }
		public Applier Apply { get; init; }
		/// <summary>
		/// Path found in the search process. Empty if not found.
		/// </summary>
		public List<TMove> Path { get; init; }
		#endregion
		#region Methods
		public List<TMove> AlphaBetaPruning(Evaluator<TGame, TValue> evaluate, uint depth) {
			return Path;
		}
		#endregion

		#region Classes
		/// <summary>
		/// A node presents a state during search process.
		/// </summary>
		public class Node : TreeNodeBase<Node>, IEquatable<Node> {
			public Node() : base() {
				State = default;
				Cost = default;
			}
			public Node(TGame state, TValue cost = default, Node parent = null) : base(parent) {
				State = state;
				Cost = cost;
			}
			public bool Equals(Node other) => State.Equals(other.State);
			public TGame State { get; set; }
			public TValue Cost { get; set; }
			public override bool Equals(object obj) => obj is Node && Equals(obj as Node);
			public override int GetHashCode() => State.GetHashCode();
			public override string ToString() {
				var result = new StringBuilder(State.ToString());
				result.AppendLine($"Cost: {Cost}");
				return result.ToString();
			}
		}
		/// <summary>
		/// Node with score
		/// </summary>
		public class ScoredNode : Node, IComparable<ScoredNode> {
			public ScoredNode(IComparer<ScoredNode> comparer = null) : base() {
				Score = default;
				Comparer = comparer;
			}
			public ScoredNode(TGame state, TValue cost = default, ScoredNode parent = null, TValue score = default) : base(state, cost, parent)
				=> Score = score;
			public ScoredNode(IComparer<ScoredNode> comparer, TGame state, TValue cost = default, ScoredNode source = null, TValue score = default) : this(state, cost, source, score)
				=> Comparer = comparer;
			public int CompareTo(ScoredNode other) => Comparer == null ? Score.CompareTo(other.Score) : Comparer.Compare(this, other);
			public new ScoredNode Parent {
				get => (this as Node).Parent as ScoredNode;
				set => (this as Node).Parent = value;
			}
			public TValue Score { get; set; }
			private IComparer<ScoredNode> Comparer { get; } = null;
			public override string ToString() {
				var result = new StringBuilder(base.ToString());
				result.AppendLine($"Score: {Score}");
				return result.ToString();
			}
		}
		/// <summary>
		/// Comparer of nodes
		/// </summary>
		protected class NodeEqualityComparer : IEqualityComparer<Node> {
			public bool Equals(Node x, Node y) => x.State.Equals(y.State);
			public int GetHashCode([DisallowNull] Node obj) => obj.State.GetHashCode();
		}
		#endregion
	}
	public class Search<TGame, TMove> : Search<TGame, TMove, int> where TGame : class {
		public Search() : base() { }
		public Search(TGame src, Transformer transformer, Applier applier) : base(src, transformer, applier) { }
	}
}
