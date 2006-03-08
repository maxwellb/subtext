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
using System.Data;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Framework.Logging
{
	/// <summary>
	/// Summary description for DatabaseLoggingProvider.
	/// </summary>
	public class DatabaseLoggingProvider : LoggingProvider
	{
		/// <summary>
		/// Gets a pageable collection of log entries.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDirection">The sort direction.</param>
		/// <returns></returns>
		public override PagedLogEntryCollection GetPagedLogEntries(int pageIndex, int pageSize, SortDirection sortDirection)
		{
			IDataReader reader = DbProvider.Instance().GetPagedLogEntries(pageIndex, pageSize, sortDirection);
			PagedLogEntryCollection entries = new PagedLogEntryCollection();
			while(reader.Read())
			{
				entries.Add(DataHelper.LoadSingleLogEntry(reader));
			}
			reader.NextResult();
			entries.MaxItems = DataHelper.GetMaxItems(reader);
			return entries;
		}

		/// <summary>
		/// Clears the log.
		/// </summary>
		public override void ClearLog()
		{
			DbProvider.Instance().ClearLog();
		}

	}
}