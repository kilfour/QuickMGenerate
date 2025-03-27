namespace QuickMGenerate.UnderTheHood
{
	public struct Unit
		: IComparable<Unit>, IEquatable<Unit>
	{
		private static readonly Unit TheInstance = new Unit();

		public static Unit Instance
		{
			get
			{
				return TheInstance;
			}
		}

		public static bool operator ==(Unit left, Unit right)
		{
			return true;
		}

		public static bool operator !=(Unit left, Unit right)
		{
			return false;
		}

		public override bool Equals(object obj)
		{
			return obj is Unit;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		int IComparable<Unit>.CompareTo(Unit other)
		{
			return 0;
		}

		bool IEquatable<Unit>.Equals(Unit other)
		{
			return true;
		}
	}
}