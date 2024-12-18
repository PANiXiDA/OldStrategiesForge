using BaseDAL;
using ChatsService.DAL.DbModels;
using ChatsService.DAL.DbModels.Models;
using ChatsService.Dto;
using Common.ConvertParams.ChatsService;
using Common.SearchParams.ChatsService;

namespace ChatsService.DAL.Interfaces;

public interface IChatsDAL : IBaseDal<DefaultDbContext, Chat, ChatDto, Guid, ChatsSearchParams, ChatsConvertParams>
{
}
