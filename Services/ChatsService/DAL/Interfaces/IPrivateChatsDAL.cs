using BaseDAL;
using ChatsService.DAL.DbModels;
using ChatsService.DAL.DbModels.Models;
using ChatsService.Dto;
using Common.ConvertParams.ChatsService;
using Common.SearchParams.ChatsService;

namespace ChatsService.DAL.Interfaces;

public interface IPrivateChatsDAL : IBaseDal<DefaultDbContext, PrivateChat, PrivateChatDto, Guid, PrivateChatsSearchParams, PrivateChatsConvertParams>
{
}
