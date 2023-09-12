using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using BLogic.Extensions;
using BLogic.Factories;
using BLogic.Messages;
using BLogic.Models;
using BLogic.Repositories;
using ViewModel.Commands;

namespace ViewModel.ViewModels
{
    public class PostCommentViewModel : AbstractViewModel
    {
        private readonly CommentRepository _commentRepository;
        private readonly PostRepository _postRepository;
        private readonly MembershipRepository _membershipRepository;
        private readonly MemberRepository _memberRepository;
        private PostDetailModel _newPost;
        private MemberDetailModel _activeMember;
        private TeamDetailModel _activeTeam;
        private PostDetailModel _selectedPost;
        private CommentModel _newComment;

        public ObservableCollection<PostListModel> Posts { get; set; } = new ObservableCollection<PostListModel>();
        public ObservableCollection<CommentModel> Comments { get; set; } = new ObservableCollection<CommentModel>();

        public PostDetailModel NewPost
        {
            get => _newPost;
            set
            {
                _newPost = value;
                NotifyPropertyChanged();
            }
        }

        public CommentModel NewComment
        {
            get => _newComment;
            set
            {
                _newComment = value;
                NotifyPropertyChanged();
            }
        }

        public TeamDetailModel ActiveTeam
        {
            get => _activeTeam;
            set
            {
                _activeTeam = value;
                NotifyPropertyChanged();
            }
        }

        public PostDetailModel SelectedPost
        {
            get => _selectedPost;
            set
            {
                _selectedPost = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanDeletePost
        {
            get
            {
                var membership = _membershipRepository.GetByTeamAndMemberId(ActiveMember.Id, ActiveTeam.Id);
                if (membership == null || SelectedPost == null)
                    return false;
                return (ActiveMember.IsAdmin || membership.IsAdmin || membership.IsFounder || SelectedPost.Author.Id == ActiveMember.Id);
            }
        }

        public MemberDetailModel ActiveMember
        {
            get => _activeMember;
            set
            {
                _activeMember = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand PostSelectedCommand { get; }
        public ICommand PostCreatedCommand { get; }
        public ICommand AddCommentCommand { get; }
        public ICommand PostDeleteCommand { get; }
        public ICommand CommentDeleteCommand { get; }

        public PostCommentViewModel(Mediator mediator, IDbContextFactory dbContextFactory)
        {
            var mediator1 = mediator;
            _postRepository = new PostRepository(dbContextFactory);
            _commentRepository = new CommentRepository(dbContextFactory);
            _membershipRepository = new MembershipRepository(dbContextFactory);
            _memberRepository = new MemberRepository(dbContextFactory);

            NewPost = new PostDetailModel();
            NewComment = new CommentModel();

            mediator1.Register<ActiveTeamChangedMessage>(TeamChanged);
            mediator1.Register<MemberLoginMessage>(LoggingIn);

            PostCreatedCommand = new RelayCommand(CreatePost, CanCreatePost);
            PostDeleteCommand = new RelayCommand(DeletePost);
            PostSelectedCommand = new RelayCommand<PostListModel>(SelectPost);
            AddCommentCommand = new RelayCommand(AddComment, CanAddComment);
            CommentDeleteCommand = new RelayCommand<CommentModel>(DeleteComment, CanDeleteComment);
 
        }

        public void LoggingIn(MemberLoginMessage memberLoginMessage)
        {
            Posts.Clear();
            Comments.Clear();
            ActiveMember = memberLoginMessage.LoggedInMember;
        }

        public void TeamChanged(ActiveTeamChangedMessage activeTeamChangedMessage)
        {
            ActiveTeam = activeTeamChangedMessage.ActiveTeam;
            GetAllPostsInTeam();
        }

        private void GetAllPostsInTeam()
        {
            Posts.Clear();
            if (ActiveTeam != null) { 
                var posts = _postRepository.GetAllInTeam(ActiveTeam).OrderBy(p => p.LastActivityDate);
                Posts.AddRange(posts);
            }
        }

        private void GetAllCommentsInPost()
        {
            Comments.Clear();
            if (SelectedPost != null) { 
                var comments = _commentRepository.GetAllInPost(SelectedPost).OrderBy(p => p.Date);
                Comments.AddRange(comments);
            }
        }

        public void CreatePost()
        {
            NewPost.TeamWithThisPost = ActiveTeam;
            NewPost.Author = ActiveMember;
            NewPost.Date = DateTime.Now;
            NewPost.LastActivityDate = NewPost.Date;
            _postRepository.Create(NewPost);
            GetAllPostsInTeam();
            NewPost = new PostDetailModel();
            ActiveMember.LastActionDate = DateTime.Now;
            _memberRepository.Update(ActiveMember);
        }

        public void DeletePost()
        {
            var commentList = _commentRepository.GetAllInPost(SelectedPost);
            foreach (var comm in commentList)
            {
                _commentRepository.Delete(comm.Id);
            }
            _postRepository.Delete(SelectedPost.Id);
            GetAllPostsInTeam();
            ActiveMember.LastActionDate = DateTime.Now;
            _memberRepository.Update(ActiveMember);
        }

        public bool CanCreatePost()
        {
            return !(string.IsNullOrWhiteSpace(NewPost.Title) || string.IsNullOrWhiteSpace(NewPost.Text));
        }

        public void SelectPost(PostListModel postListModel)
        {
            Comments.Clear();
            SelectedPost = _postRepository.GetById(postListModel.Id);
            NewComment = new CommentModel();
            GetAllCommentsInPost();
            NotifyPropertyChanged(nameof(CanDeletePost));
        }

        public void AddComment()
        {
            NewComment.Author = ActiveMember;
            NewComment.Date = DateTime.Now;
            SelectedPost.LastActivityDate = NewComment.Date;
            NewComment.ParentPost = SelectedPost;
            _commentRepository.Create(NewComment);
            GetAllCommentsInPost();
            NewComment = new CommentModel();
            ActiveMember.LastActionDate = DateTime.Now;
            _memberRepository.Update(ActiveMember);
        }

        public bool CanAddComment()
        {
            return !string.IsNullOrWhiteSpace(NewComment.Text);
        }

        public void DeleteComment(CommentModel comment)
        {
            _commentRepository.Delete(comment.Id);
            GetAllCommentsInPost();
            ActiveMember.LastActionDate = DateTime.Now;
            _memberRepository.Update(ActiveMember);
        }

        public bool CanDeleteComment(CommentModel comment)
        {
            var membership = _membershipRepository.GetByTeamAndMemberId(ActiveMember.Id, ActiveTeam.Id);
            if (membership == null || SelectedPost == null)
                return false;
            return (ActiveMember.IsAdmin || membership.IsAdmin || membership.IsFounder || comment?.Author?.Id == ActiveMember?.Id);
        }
    }
}