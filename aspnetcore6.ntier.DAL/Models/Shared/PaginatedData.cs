﻿using aspnetcore6.ntier.DAL.Models.Abstract;
using Microsoft.EntityFrameworkCore;

namespace aspnetcore6.ntier.DAL.Models.Shared
{
    public class PaginatedData<TEntity> where TEntity : BaseEntity
    {
        public PaginatedData(IEnumerable<TEntity> entities, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Data = entities;
        }

        public IEnumerable<TEntity> Data { get; private set; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        

        public static async Task<PaginatedData<TEntity>> ToPaginatedData(IQueryable<TEntity> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var entities = await source.OrderBy(e => e.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedData<TEntity>(entities, count, pageNumber, pageSize);
        }
    }
}