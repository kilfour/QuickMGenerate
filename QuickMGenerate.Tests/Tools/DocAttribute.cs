namespace QuickMGenerate.Tests.Tools;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DocAttribute : Attribute, IEquatable<DocAttribute>
{
	public string Chapter { get; set; } = string.Empty;
	public int ChapterOrder { get; set; }
	public string Caption { get; set; } = string.Empty;
	public int CaptionOrder { get; set; }
	public string Content { get; set; } = string.Empty;
	public int Order { get; set; }

	public bool Equals(DocAttribute? other) =>
		other is not null &&
		Chapter == other.Chapter &&
		ChapterOrder == other.ChapterOrder &&
		Caption == other.Caption &&
		CaptionOrder == other.CaptionOrder &&
		Content == other.Content &&
		Order == other.Order;

	public override bool Equals(object? obj) =>
		obj is DocAttribute other && Equals(other);

	public override int GetHashCode() =>
		HashCode.Combine(Chapter, ChapterOrder, Caption, CaptionOrder, Content, Order);
}