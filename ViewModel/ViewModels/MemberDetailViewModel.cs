using System.Windows.Input;
using BLogic.Factories;
using BLogic.Messages;
using BLogic.Models;
using BLogic.Repositories;
using ViewModel.Commands;

namespace ViewModel.ViewModels
{
    public class MemberDetailViewModel : AbstractViewModel
    {
        private readonly MemberRepository _memberRepository;
        private readonly MembershipRepository _membershipRepository;
        private readonly PostRepository _postRepository;
        private readonly CommentRepository _commentRepository;
        private readonly Mediator _mediator;
        private MemberDetailModel _activeMember;
        private MemberDetailModel _viewedMember;
        private string _newNickname;
        private bool _canAdministrate;
        private bool _isThisMe;

        public MemberDetailModel ActiveMember
        {
            get => _activeMember;
            set
            {
                _activeMember = value;
                NotifyPropertyChanged();
            }
        }

        public MemberDetailModel ViewedMember
        {
            get => _viewedMember;
            set
            {
                _viewedMember = value;
                NotifyPropertyChanged();
            }
        }

        public string NewNickname
        {
            get => _newNickname;
            set
            {
                _newNickname = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanAdministrate
        {
            get => _canAdministrate;
            set
            {
                _canAdministrate = value;
                NotifyPropertyChanged();
            }
        }

        public bool IsThisMe
        {
            get => _isThisMe;
            set
            {
                _isThisMe = value;
                NotifyPropertyChanged();
            }
        }
        
        public ICommand ChangeNicknameCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand DeleteProfileCommand { get; }
        public ICommand BackCommand { get; }

        public MemberDetailViewModel(Mediator mediator, IDbContextFactory dbContextFactory)
        {
            _memberRepository = new MemberRepository(dbContextFactory);
            _membershipRepository = new MembershipRepository(dbContextFactory);
            _postRepository = new PostRepository(dbContextFactory);
            _commentRepository = new CommentRepository(dbContextFactory);
            _mediator = mediator;

            _mediator.Register<ShowMemberInfoMessage>(ShowInfo);
            ViewedMember = null;

            ChangeNicknameCommand = new RelayCommand(ChangeNickname);
            LogoutCommand = new RelayCommand(Logout);
            DeleteProfileCommand = new RelayCommand(EraseMember);
            BackCommand = new RelayCommand(CloseMe);
        }

        public void ShowInfo(ShowMemberInfoMessage showMemberInfoMessage)
        {
            ActiveMember = showMemberInfoMessage.ActiveMember;
            ViewedMember = showMemberInfoMessage.ViewedMember;
            IsThisMe = ActiveMember.Id == ViewedMember.Id;
            CanAdministrate = ActiveMember.IsAdmin || IsThisMe;
            NewNickname = "";
        }

        public void EraseMember()
        {
            foreach (var post in _postRepository.GetAllByMember(ViewedMember))
            {
                var detailPost = _postRepository.GetById(post.Id);
                foreach (var comment in _commentRepository.GetAllInPost(detailPost))
                {
                    _commentRepository.Delete(comment.Id);
                }
                _postRepository.Delete(post.Id);
            }

            foreach (var comment in _commentRepository.GetAllByMember(ViewedMember))
            {
                _commentRepository.Delete(comment.Id);
            }

            foreach (var membership in _membershipRepository.GetAllByMember(ViewedMember))
            {
                _membershipRepository.Delete(membership.Id);
            }
            _memberRepository.Delete(ViewedMember.Id);
            Logout();
        }

        public void ChangeNickname()
        {
            if (string.IsNullOrWhiteSpace(NewNickname))
            {
                ViewedMember.Nickname = ViewedMember.FirstName + " " + ViewedMember.LastName;
            }
            else
            {
                ViewedMember.Nickname = NewNickname;
            }
            _memberRepository.Update(ViewedMember);
            NotifyPropertyChanged(nameof(ViewedMember));
        }

        public void Logout()
        {
            _mediator.Send(new MemberLogoutMessage {Email = ActiveMember.Email});
            CloseMe();
        }

        private void CloseMe()
        {
            _mediator.Send(new ProfileClosedMessage());
            ViewedMember = null;
            ActiveMember = null;
        }
    }
}