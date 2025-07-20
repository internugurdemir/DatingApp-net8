namespace API.SignalR;

public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> OnlineUsers = [];
    // List<string>> OnlineUsers = new Dictionary<string, List<string>>();
    public Task<bool> UserConnected(string username, string connectionId)
    {
        bool isOnline = false;
        /*
        The lock keyword in C# is used to prevent multiple threads from changing the same data at the same time.
        This means:
        Only one thread can enter this block at a time.
        It protects the OnlineUsers dictionary from being used by many threads at once.
        Without lock, if two users connect at the same time, the program might crash or data might be wrong.
        */
        lock (OnlineUsers)
        {
            if (OnlineUsers.ContainsKey(username))
            {
                OnlineUsers[username].Add(connectionId);
            }
            else
            {
                OnlineUsers.Add(username, [connectionId]);
                isOnline = true;
            }
        }

        return Task.FromResult(isOnline);
    }

    public Task<bool> UserDisconnected(string username, string connectionId)
    {
        bool isOffline = false;
        lock (OnlineUsers)
        {
            if (!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOffline);

            OnlineUsers[username].Remove(connectionId);

            if (OnlineUsers[username].Count == 0)
            {
                OnlineUsers.Remove(username);
                isOffline = true;
            }
        }

        return Task.FromResult(isOffline);
    }

    public Task<string[]> GetOnlineUsers()
    {
        string[] onlineUsers;
        lock (OnlineUsers)
        {
            onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
        }

        return Task.FromResult(onlineUsers);
    }

    public static Task<List<string>> GetConnectionsForUser(string username)
    {
        List<string> connectionIds;
        if (OnlineUsers.TryGetValue(username, out var connections))
        {
            lock (connections)
            {
                connectionIds = connections.ToList();
            }
        }
        else
        {
            connectionIds = [];
        }

        return Task.FromResult(connectionIds);
    }
}
