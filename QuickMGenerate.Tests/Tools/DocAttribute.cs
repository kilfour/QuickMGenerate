using System;

namespace QuickMGenerate.Tests.Tools
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class DocAttribute : Attribute
	{
		public string Chapter { get; set; }
		public int ChapterOrder { get; set; }
		public string Caption { get; set; }
		public int CaptionOrder { get; set; }
		public string Content { get; set; }
		public int Order { get; set; }

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			return Equals(obj as DocAttribute);
		}

		public bool Equals(DocAttribute other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return base.Equals(other) && Equals(other.Chapter, Chapter) && other.ChapterOrder == ChapterOrder && Equals(other.Caption, Caption) && other.CaptionOrder == CaptionOrder && Equals(other.Content, Content) && other.Order == Order;
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = base.GetHashCode();
				result = (result*397) ^ (Chapter != null ? Chapter.GetHashCode() : 0);
				result = (result*397) ^ ChapterOrder;
				result = (result*397) ^ (Caption != null ? Caption.GetHashCode() : 0);
				result = (result*397) ^ CaptionOrder;
				result = (result*397) ^ (Content != null ? Content.GetHashCode() : 0);
				result = (result*397) ^ Order;
				return result;
			}
		}
	}
}
