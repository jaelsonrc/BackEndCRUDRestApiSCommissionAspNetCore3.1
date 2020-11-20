using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.Utils
{
	public interface IQueryParameters
	{
		const int maxPageSize = 50;
		int PageNumber { get; set; }
		int PageSize { get; set; }
		string OrderBy { get; set; }
	}
}
