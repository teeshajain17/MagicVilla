using MagicVilla_VillaAPI.Models;
using System;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepositories
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa villa);

    }
}
