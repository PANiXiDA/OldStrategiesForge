using BaseDAL;
using Common.ConvertParams.ProfileService;
using Common.SearchParams.ProfileService;
using ProfileService.DAL.DbModels;
using ProfileService.DAL.DbModels.Models;
using ProfileService.Dto;

namespace ProfileService.DAL.Interfaces;

public interface IFramesDAL : IBaseDal<DefaultDbContext, Frame, FramesDto, int, FramesSearchParams, FramesConvertParams>
{
}
