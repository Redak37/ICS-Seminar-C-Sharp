using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using System.Windows.Media;
using BLogic.Extensions;
using BLogic.Factories;
using BLogic.Messages;
using BLogic.Repositories;
using BLogic.Models;
using ViewModel.Commands;
using Color = System.Drawing.Color;

// ReSharper disable InvertIf

namespace ViewModel.ViewModels
{
    public class TeamViewModel : AbstractViewModel
    {
        private readonly TeamRepository _teamRepository;
        private readonly MemberRepository _memberRepository;
        private readonly MembershipRepository _membershipRepository;
        private readonly CommentRepository _commentRepository;
        private readonly PostRepository _postRepository;
        private readonly Mediator _mediator;

        public Color[] PickableColors { get; set; }

        public TeamDetailModel ActiveTeam { get; set; }
        public ObservableCollection<TeamDetailModel> Teams { get; set; } = new ObservableCollection<TeamDetailModel>();
        public ObservableCollection<MemberListModel> ActiveTeamMembers { get; set; } = new ObservableCollection<MemberListModel>();
        private MemberDetailModel _activeMember;
        private TeamDetailModel _newTeam;
        private MembershipModel _membershipOfActiveMember;
        private bool _canDestroyTeam;
        private bool _canEditTeam;
        private string _newMemberEmail;

        public MemberDetailModel ActiveMember
        {
            get => _activeMember;
            set
            {
                _activeMember = value;
                NotifyPropertyChanged();
            }
        }

        public TeamDetailModel NewTeam
        {
            get => _newTeam;
            set
            {
                _newTeam = value;
                NotifyPropertyChanged();
            }
        }

        public MembershipModel MembershipOfActiveMember
        {
            get => _membershipOfActiveMember;
            set
            {
                _membershipOfActiveMember = value;
                NotifyPropertyChanged();
            }
        }

        public string NewMemberEmail
        {
            get => _newMemberEmail;
            set
            {
                _newMemberEmail = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanDestroyTeam
        {
            get => _canDestroyTeam;
            set
            {
                _canDestroyTeam = value;
                NotifyPropertyChanged();
            }
        }

        public bool CanEditTeam
        {
            get => _canEditTeam;
            set
            {
                _canEditTeam = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand TeamCreatedCommand { get; }
        public ICommand TeamSelectedCommand { get; }
        public ICommand LeaveTeamCommand { get; }
        public ICommand DestroyTeamCommand { get; }
        public ICommand MemberSelectedCommand { get; }
        public ICommand ChangeMemberAdminCommand { get; }
        public ICommand KickMemberFromTeamCommand { get; }
        public ICommand AddMemberCommand { get; }
        public ICommand ShowProfileCommand { get; }
        
        public TeamViewModel(Mediator mediator, IDbContextFactory dbContextFactory)
        {
            _teamRepository = new TeamRepository(dbContextFactory);
            _memberRepository = new MemberRepository(dbContextFactory);
            _membershipRepository = new MembershipRepository(dbContextFactory);
            _postRepository = new PostRepository(dbContextFactory);
            _commentRepository = new CommentRepository(dbContextFactory);
            _mediator = mediator;

            PropertyInfo[] colors = typeof(Colors).GetProperties();
            PickableColors = Array.ConvertAll(colors, PropertyInfoToColor);

            ActiveTeam = null;
            NewTeam = new TeamDetailModel()
            {
                RGB = Color.Red
            };

            mediator.Register<MemberLoginMessage>(LoggingIn);
            mediator.Register<MemberLogoutMessage>(Logout);
            mediator.Register<ProfileClosedMessage>(Comeback);

            TeamCreatedCommand = new RelayCommand(CreateTeam, CanCreateTeam);
            TeamSelectedCommand = new RelayCommand<TeamDetailModel>(ChangeActiveTeam);
            LeaveTeamCommand = new RelayCommand(LeaveTeam);
            DestroyTeamCommand = new RelayCommand(DestroyTeam);
            KickMemberFromTeamCommand = new RelayCommand<MemberListModel>(RemoveMemberFromTeam, CanKickMember);
            ChangeMemberAdminCommand = new RelayCommand<MemberListModel>(ChangeMemberAdmin, CanChangeMemberAdmin);
            AddMemberCommand = new RelayCommand(AddMemberToTeam, CanAddMemberToTeam);
            MemberSelectedCommand = new RelayCommand<MemberListModel>(ShowMemberInfo);
            ShowProfileCommand = new RelayCommand(ShowProfile);
        }

        private static Color PropertyInfoToColor(PropertyInfo input)
        {
            return Color.FromName(input.Name);
        }

        public void LoggingIn(MemberLoginMessage memberLoginMessage)
        {
            ActiveMember = memberLoginMessage.LoggedInMember;
            GetAllTeamsOfMember();
        }

        public void GetAllTeamsOfMember()
        {
            var teams = _teamRepository.GetAllOfMember(ActiveMember);
            Teams.Clear();
            Teams.AddRange(teams);
        }

        public void GetAllMembersInTeam()
        {
            ActiveTeamMembers.Clear();
            if (ActiveTeam != null)
            {
                var members = _memberRepository.GetAllInTeam(ActiveTeam);
                ActiveTeamMembers.AddRange(members);
            }
        }

        public void AddMemberToTeam()
        {
            
            var memberBeingChanged = _memberRepository.GetByEmail(NewMemberEmail);
            var newMembership = new MembershipModel
            {
                Team = ActiveTeam,
                Member = memberBeingChanged,
                IsAdmin = false,
                IsFounder = false
            };
            _membershipRepository.Create(newMembership);
            NewMemberEmail = "";
            GetAllMembersInTeam();
        }

        public bool CanAddMemberToTeam()
        {
            var member = _memberRepository.GetByEmail(NewMemberEmail);
            return !string.IsNullOrWhiteSpace(NewMemberEmail) && member != null && _membershipRepository.GetByTeamAndMemberId(member.Id, ActiveTeam.Id) == null;
        }

        public void ChangeMemberAdmin(MemberListModel member)
        {
            var membershipOfMemberBeingChanged = _membershipRepository.GetByTeamAndMemberId(member.Id, ActiveTeam.Id);
            membershipOfMemberBeingChanged.IsAdmin = !membershipOfMemberBeingChanged.IsAdmin;
            _membershipRepository.Update(membershipOfMemberBeingChanged);
        }

        public bool CanChangeMemberAdmin(MemberListModel member)
        {
            if (ActiveTeam != null)
            {
                var membership = _membershipRepository.GetByTeamAndMemberId(member.Id, ActiveTeam.Id);
                return ActiveMember.Id != member.Id && membership != null && !membership.IsFounder;
            }
            return false;
        }

        public bool CanKickMember(MemberListModel member)
        {
            if (ActiveTeam != null)
            {
                var membership = _membershipRepository.GetByTeamAndMemberId(member.Id, ActiveTeam.Id);
                return membership != null && !membership.IsFounder;
            }
            return false;
        }

        public void LeaveTeam()
        {
            _membershipRepository.Delete(MembershipOfActiveMember.Id);
            if (!_membershipRepository.GetAllInTeam(ActiveTeam).Any())
            {
                _teamRepository.Delete(ActiveTeam.Id);
            }
            GetAllTeamsOfMember();
            ChangeActiveTeam(null);
        }

        public void RemoveMemberFromTeam(MemberListModel member)
        {
            var membershipOfMemberBeingChanged = _membershipRepository.GetByTeamAndMemberId(member.Id, ActiveTeam.Id);
            _membershipRepository.Delete(membershipOfMemberBeingChanged.Id);
            if (!_membershipRepository.GetAllInTeam(ActiveTeam).Any())
            {
                _teamRepository.Delete(ActiveTeam.Id);
            }
            if (ActiveMember.Id == member.Id)
            {
                GetAllTeamsOfMember();
                ChangeActiveTeam(null);
            }
            else
            {
                GetAllMembersInTeam();
            }
        }

        public void DestroyTeam()
        {
            foreach (var membership in _membershipRepository.GetAllInTeam(ActiveTeam))
            {
                _membershipRepository.Delete(membership.Id);
            }

            foreach (var post in _postRepository.GetAllInTeam(ActiveTeam))
            {
                var detailPost = _postRepository.GetById(post.Id);
                foreach (var comment in _commentRepository.GetAllInPost(detailPost))
                {
                    _commentRepository.Delete(comment.Id);
                }
                _postRepository.Delete(post.Id);
            }
            _teamRepository.Delete(ActiveTeam.Id);
            GetAllTeamsOfMember();
            ChangeActiveTeam(null);
        }

        public void ChangeActiveTeam(TeamDetailModel teamDetailModel)
        {
            ActiveTeam = teamDetailModel;
            GetAllMembersInTeam();
            MembershipOfActiveMember = ActiveTeam != null ? _membershipRepository.GetByTeamAndMemberId(ActiveMember.Id, ActiveTeam.Id) : null;
            CanDestroyTeam = MembershipOfActiveMember != null && (ActiveMember.IsAdmin || MembershipOfActiveMember.IsFounder);
            CanEditTeam = MembershipOfActiveMember != null && (ActiveMember.IsAdmin || MembershipOfActiveMember.IsFounder || MembershipOfActiveMember.IsAdmin);
            NewMemberEmail = "";
            _mediator.Send(new ActiveTeamChangedMessage {ActiveTeam = ActiveTeam});
        }

        public void CreateTeam()
        {
            _teamRepository.Create(NewTeam);
            var newFoundership = new MembershipModel
            {
                IsAdmin = true,
                IsFounder = true,
                Member = ActiveMember,
                Team = NewTeam
            };
            _membershipRepository.Create(newFoundership);
            GetAllTeamsOfMember();
            NewTeam = new TeamDetailModel()
            {
                RGB = Color.Red
            };
        }

        private bool CanCreateTeam()
        {
            return !string.IsNullOrWhiteSpace(NewTeam.Name);
        }

        public void ShowProfile()
        {
            _mediator.Send(new ShowMemberInfoMessage
            {
                ActiveMember = ActiveMember,
                ViewedMember = ActiveMember
            });
        }

        public void ShowMemberInfo(MemberListModel memberListModel)
        {
            var member = _memberRepository.GetById(memberListModel.Id);
            _mediator.Send(new ShowMemberInfoMessage
            {
                ActiveMember = ActiveMember,
                ViewedMember = member
            });
        }
        public void Logout(MemberLogoutMessage logoutMessage)
        {
            ActiveMember = null;
            ActiveTeam = null;
            Teams.Clear();
            ActiveTeamMembers.Clear();
        }
        public void Comeback(ProfileClosedMessage profileClosed)
        {
            NotifyPropertyChanged(nameof(ActiveMember));
            if (ActiveTeam != null)
            {
                GetAllMembersInTeam();
            }
        }
    }
}