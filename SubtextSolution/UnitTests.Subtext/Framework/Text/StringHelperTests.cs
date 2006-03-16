using System;
using MbUnit.Framework;
using Subtext.Framework.Text;

namespace UnitTests.Subtext.Framework.Text
{
	/// <summary>
	/// Summary description for StringHelperTests.
	/// </summary>
	[TestFixture]
	public class StringHelperTests
	{
		/// <summary>
		/// Tests that we can properly pascal case text.
		/// </summary>
		/// <remarks>
		/// Does not remove punctuation.
		/// </remarks>
		/// <param name="original"></param>
		/// <param name="expected"></param>
		[RowTest]
		[Row("", "")]
		[Row("a", "A")]
		[Row("A", "A")]
		[Row("A B", "AB")]
		[Row("a bee keeper's dream.", "ABeeKeeper'sDream.")]
		public void PascalCaseTests(string original, string expected)
		{
			Assert.AreEqual(expected, StringHelper.PascalCase(original));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void PascalCaseThrowsArgumentNullException()
		{
			StringHelper.PascalCase(null);
		}
	}
}