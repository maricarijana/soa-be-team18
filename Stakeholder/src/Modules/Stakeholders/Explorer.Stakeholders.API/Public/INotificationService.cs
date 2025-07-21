using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.API.Public
{
    public interface INotificationService
    {
        Result<NotificationDto> Create(NotificationDto dto);
        Result<NotificationDto> Update(NotificationDto dto);
        Result<PagedResult<NotificationDto>> GetPaged(int page, int pageSize);
        Result Delete(int id);
        Result<PagedResult<NotificationDto>> GetUnreadByUser(long userId, int page, int pageSize);
        Result<PagedResult<NotificationDto>> GetAllByUser(long userId, int page, int pageSize);
    }
}