using System;
using System.Collections.Generic;
using System.Linq;

namespace rentasgt.Application.Common.Models
{
    public class PaginatedListResponse<T>
    {

		public int CurrentPage { get; private set; }
		public int TotalPages { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; private set; }

		public bool HasPrevious => CurrentPage > 1;
		public bool HasNext => CurrentPage < TotalPages;

		public List<T> Items { get; set; }

		public PaginatedListResponse(List<T> items, int count, int pageNumber, int pageSize)
		{
			TotalCount = count;
			PageSize = pageSize;
			CurrentPage = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);

			Items = items;
		}

		public static PaginatedListResponse<T> ToPaginatedListResponse(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var count = source.Count();
			var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

			return new PaginatedListResponse<T>(items, count, pageNumber, pageSize);
		}

	}
}
