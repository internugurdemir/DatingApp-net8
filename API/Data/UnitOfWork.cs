using API.Interfaces;
using AutoMapper;

namespace API.Data
{
    public class UnitOfWork(DataContext context,
                            IUserRepository userRepository,
                            ILikesRepository likesRepository,
                            IMessageRepository messageRepository,
                            IMapper mapper) : IUnitOfWork
    {


        public IUserRepository UserRepository => userRepository;

        public IMessageRepository MessageRepository => messageRepository;

        public ILikesRepository LikesRepository => likesRepository;

        public async Task<bool> Complete()
        {
            return await context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            // context.ChangeTracker.DetectChanges();
            // var changes = context.ChangeTracker.HasChanges();
            return context.ChangeTracker.HasChanges();

            // return changes;
        }
    }
}