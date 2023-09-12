using BLogic.Factories;
using BLogic.Messages;

namespace ViewModel.ViewModels
{
    public class ViewModelLocator
    {
        private readonly IDbContextFactory _dbContextFactory;
        private readonly Mediator _mediator;

        public ViewModelLocator()
        {
            _dbContextFactory = new DbContextFactory();
#if DEBUG
            using (var dbx = _dbContextFactory.CreateDbContext())
            {
                dbx.Database.EnsureCreated();
            }
#endif

            _mediator = new Mediator();
        }
        
        public TeamViewModel TeamViewModel => new TeamViewModel(_mediator, _dbContextFactory);
        public LoginRegisterViewModel LoginRegisterViewModel => new LoginRegisterViewModel(_mediator, _dbContextFactory);
        public PostCommentViewModel PostCommentViewModel => new PostCommentViewModel(_mediator, _dbContextFactory);
        public MemberDetailViewModel MemberDetailViewModel => new MemberDetailViewModel(_mediator, _dbContextFactory);
    }
}