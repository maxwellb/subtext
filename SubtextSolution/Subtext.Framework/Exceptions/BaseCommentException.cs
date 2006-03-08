#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Base exception class for comment errors.
	/// </summary>
	[Serializable]
	public abstract class BaseCommentException : BaseSubtextException
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CommentSpamException"/> class.
		/// </summary>
		protected BaseCommentException() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CommentSpamException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		protected BaseCommentException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseCommentException"/> class.
		/// </summary>
		/// <param name="innerException">The inner exception.</param>
		protected BaseCommentException(Exception innerException) : base(innerException)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="CommentSpamException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The inner exception.</param>
		protected BaseCommentException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}