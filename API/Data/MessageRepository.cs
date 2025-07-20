using System;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class MessageRepository(DataContext context, IMapper mapper) : IMessageRepository
{
    public void AddGroup(Group group)
    {
        context.Groups.Add(group);
    }

    public void AddMessage(Message message)
    {
        context.Messages.Add(message);
    }

    public void DeleteMessage(Message message)
    {
        context.Messages.Remove(message);
    }

    public async Task<Connection?> GetConnection(string connectionId)
    {
        return await context.Connections.FindAsync(connectionId);

    }

    public async Task<Group?> GetGroupForConnection(string connectionId)
    {
        return await context.Groups.Include(a => a.Connections)
                                    .Where(a => a.Connections.Any(c=>c.ConnectionId == connectionId))
                                    .FirstOrDefaultAsync();
    }

    public async Task<Message?> GetMessage(int id)
    {
        return await context.Messages.FindAsync(id);
    }

    public async Task<Group?> GetMessageGroup(string groupName)
    {
        return await context.Groups
                                .Include(a => a.Connections)
                                .FirstOrDefaultAsync(a => a.Name == groupName);
    }

    public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
    {
        var query = context.Messages
                            .OrderByDescending(a => a.MessageSent)
                            .AsQueryable();


        query = messageParams.Container switch
        {
            "Inbox" => query.Where(a => a.Recipient.UserName == messageParams.Username
                                        && a.RecipientDeleted == false),
            "Outbox" => query.Where(a => a.Sender.UserName == messageParams.Username
                                      && a.SenderDeleted == false),
            _ => query.Where(a => a.Recipient.UserName == messageParams.Username
                                && a.DateRead == null
                                && a.RecipientDeleted == false)
        };
        var messages = query.ProjectTo<MessageDto>(mapper.ConfigurationProvider);

        return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUsername, string recipientUsername)
    {
        var messages = await context.Messages
                                    .Where(a => a.RecipientUsername == currentUsername
                                                && a.RecipientDeleted == false
                                                && a.SenderUsername == recipientUsername
                                            || a.SenderUsername == currentUsername
                                                && a.SenderDeleted == false
                                                && a.RecipientUsername == recipientUsername)
                                    .OrderBy(m => m.MessageSent)
                                    .ProjectTo<MessageDto>(mapper.ConfigurationProvider)
                                    .ToListAsync();

        var unreadMessages = messages
                            .Where(a => a.DateRead == null && a.RecipientUsername == currentUsername)
                            .ToList();

        if (unreadMessages.Count != 0)
        {
            unreadMessages.ForEach(a => a.DateRead = DateTime.UtcNow);
            await context.SaveChangesAsync();
        }
        return messages;
    }

    public void RemoveConnection(Connection connection)
    {
        context.Connections.Remove(connection);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
