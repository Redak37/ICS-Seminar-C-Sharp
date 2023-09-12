using System;
using System.Net.Mail;
using System.Windows.Input;
using BLogic.Factories;
using BLogic.Messages;
using BLogic.Repositories;
using BLogic.Models;
using ViewModel.Commands;
using System.Text.RegularExpressions;

namespace ViewModel.ViewModels
{
    public class LoginRegisterViewModel : AbstractViewModel
    {
        private readonly MemberRepository _memberRepository;

        public MemberDetailModel ActiveMember { get; set; }

        public string PasswordFirst
        {
            get => _passwordFirst;
            set
            {
                _passwordFirst = value;
                NotifyPropertyChanged();
            }
        }

        public string PasswordAgain
        {
            get => _passwordAgain;
            set
            {
                _passwordAgain = value;
                NotifyPropertyChanged();
            }
        }

        public string ErrorMsg
        {
            get => _errorMsg;
            set
            {
                _errorMsg = value;
                NotifyPropertyChanged();
            }
        }

        public bool NotLoggedIn
        {
            get => _notLoggedIn;
            set
            {
                _notLoggedIn = value;
                NotifyPropertyChanged();
            }
        }

        private readonly Mediator _mediator;

        private string _errorMsg;
        private string _passwordAgain;
        private bool _notLoggedIn;
        private string _passwordFirst;

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginRegisterViewModel(Mediator mediator, IDbContextFactory dbContextFactory)
        {
            _memberRepository = new MemberRepository(dbContextFactory);
            _mediator = mediator;


            ActiveMember = new MemberDetailModel
            {
                Email = "admin@seznam.cz"
            };

            LoginCommand = new RelayCommand(Login, CanLogin);
            RegisterCommand = new RelayCommand(Register, CanRegister);
            ErrorMsg = "";
            NotLoggedIn = true;
            mediator.Register<MemberLogoutMessage>(Logout);
        }

        private void Login()
        {
            var member = _memberRepository.GetByEmail(ActiveMember.Email);

            if (member != null && Password.PasswordCheck(PasswordFirst, member.Password))
            {
                _mediator.Send(new MemberLoginMessage {LoggedInMember = member});
                NotLoggedIn = false;
                ErrorMsg = "";
            }
            else
            {
                ErrorMsg = "Špatný email nebo heslo.";
            }
            ResetPassword();
        }

        private bool CanLogin()
        {
            return !(string.IsNullOrWhiteSpace(ActiveMember.Email) || string.IsNullOrWhiteSpace(PasswordFirst));
        }

        public void Register()
        {
            if (!PasswordFirst.Equals(PasswordAgain))
            {
                ErrorMsg = "Zadaná hesla se neshodují!";
                ResetPassword();
                return;
            }
            
            if (new Regex(@"\d").Matches(PasswordFirst).Count == 0)
            {
                ErrorMsg = "Heslo musí obsahovat číslici";
                ResetPassword();
                return;
            }

            if (new Regex(@"[a-z]").Matches(PasswordFirst).Count == 0)
            {
                ErrorMsg = "Heslo musí obsahovat malé písmeno";
                ResetPassword();
                return;
            }

            if (new Regex(@"[A-Z]").Matches(PasswordFirst).Count == 0)
            {
                ErrorMsg = "Heslo musí obsahovat velké písmeno";
                ResetPassword();
                return;
            }

            if (PasswordFirst.Length <= 5)
            {
                ErrorMsg = "Heslo musí mít alespoň 5 znaků";
                ResetPassword();
                return;
            }
            
            if (!IsValidEmail(ActiveMember.Email))
            {
                ErrorMsg = "Neplatný formát emailu!";
                ResetPassword();
                return;
            }

            if (_memberRepository.GetByEmail(ActiveMember.Email) != null)
            {
                ErrorMsg = "Uživatel s tímto emailem již existuje!";
                ResetPassword();
                return;
            }

            if (string.IsNullOrWhiteSpace(ActiveMember.Nickname))
            {
                ActiveMember.Nickname = ActiveMember.FirstName + " " + ActiveMember.LastName;
            }

            ActiveMember.LastActionDate = DateTime.Now;
            ActiveMember.Password = Password.HashIt(PasswordFirst);
            _memberRepository.Create(ActiveMember);
            _mediator.Send(new MemberLoginMessage {LoggedInMember = ActiveMember});
            NotLoggedIn = false;
            ResetPassword();
            ErrorMsg = "";
        }

        private bool CanRegister()
        {
            return !(string.IsNullOrWhiteSpace(ActiveMember.Email) ||
                     string.IsNullOrWhiteSpace(ActiveMember.FirstName) ||
                     string.IsNullOrWhiteSpace(ActiveMember.LastName) ||
                     string.IsNullOrWhiteSpace(ActiveMember.Address) ||
                     string.IsNullOrWhiteSpace(PasswordAgain) ||
                     string.IsNullOrWhiteSpace(PasswordFirst));
        }

        private void ResetPassword()
        {
            PasswordAgain = "";
            PasswordFirst = "";
        }

        private static bool IsValidEmail(string emailAddress)
        {
            try
            {
                var unused = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public void Logout(MemberLogoutMessage logoutMessage)
        {
            ActiveMember = new MemberDetailModel {Email = logoutMessage.Email};
            NotifyPropertyChanged(nameof(ActiveMember));
            NotLoggedIn = true;
        }
    }
}