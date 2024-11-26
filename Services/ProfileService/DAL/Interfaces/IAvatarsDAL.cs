using BaseDAL;
using Common.ConvertParams.ProfileService;
using Common.SearchParams.ProfileService;
using ProfileService.DAL.DbModels.Models;
using ProfileService.DAL.DbModels;
using ProfileService.Dto;

namespace ProfileService.DAL.Interfaces;

public interface IAvatarsDAL : IBaseDal<DefaultDbContext, Avatar, AvatarsDto, int, AvatarsSearchParams, AvatarsConvertParams>
{
}
