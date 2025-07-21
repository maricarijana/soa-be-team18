using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using AutoMapper;
using FluentResults;
using System.ComponentModel;
using Explorer.Stakeholders.Core.Domain.Problems;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class NotificationService : CrudService<NotificationDto, Notification>, INotificationService
    {
        public readonly INotificationRepository _notificationRepository;
        public readonly IMapper _mapper;

        public NotificationService(ICrudRepository<Notification> repository, INotificationRepository notificationRepository, IMapper mapper) : base(repository, mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public Result<PagedResult<NotificationDto>> GetUnreadByUser(long userId, int page, int pageSize)
        {
            // var unreadNotifications= _notificationRepository.GetUnreadByUser(userId,page,pageSize);
            //var notificationDtos = _mapper.Map<PagedResult<NotificationDto>>(unreadNotifications);
            //return Result.Ok(notificationDtos);
            var unreadResults = _notificationRepository.GetUnreadByUser(userId);
            var paged = new PagedResult<Notification>(unreadResults, unreadResults.Count());
            return MapToDto(paged);

        }
        public Result<PagedResult<NotificationDto>> GetAllByUser(long userId, int page, int pageSize)
        {
            //var allNotifications = _notificationRepository.GetAllByUser(userId,page,pageSize);
            //var notificationDtos = _mapper.Map<PagedResult<NotificationDto>>(allNotifications);
            //return Result.Ok(notificationDtos);
            var allResults = _notificationRepository.GetAllByUser(userId);
            var paged = new PagedResult<Notification>(allResults, allResults.Count());
            return MapToDto(paged);
        }



    }
}