using System;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using EscapeRoom.Models;
using EscapeRoom.Helpers;
using EscapeRoom.Services;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.Generic;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;


//PYTANIE CZY ROBIMY PO ANGIELSKU
namespace EscapeRoom.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private User _currentUser;
        private string _firstName;
        private string _lastName;
        private string _email;
        private string _phoneNumber;
        private string _currentPassword;
        private string _newPassword;
        private string _confirmPassword;
        private string _statusMessage;
        private bool _isBusy;


        public AccountViewModel()
        {
            // _userService = new UserService(connectionString);
            // LoadUserData();
        }

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    OnPropertyChanged();
                    if (_currentUser != null)
                    {
                        FirstName = _currentUser.FirstName;
                        LastName = _currentUser.LastName;
                        Email = _currentUser.Email;
                        PhoneNumber = _currentUser.PhoneNumber;
                    }
                }
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                if (_phoneNumber != value)
                {
                    _phoneNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CurrentPassword
        {
            get => _currentPassword;
            set
            {
                if (_currentPassword != value)
                {
                    _currentPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                if (_newPassword != value)
                {
                    _newPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                if (_confirmPassword != value)
                {
                    _confirmPassword = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged();
                }
            }
        }

        private ICommand _updateProfileCommand;
        public ICommand UpdateProfileCommand
        {
            get
            {
                return _updateProfileCommand ?? (_updateProfileCommand = new RelayCommand(_ => UpdateProfile(), _ => CanUpdateProfile()));
            }
        }

        private ICommand _changePasswordCommand;
        public ICommand ChangePasswordCommand
        {
            get
            {
                return _changePasswordCommand ?? (_changePasswordCommand = new RelayCommand(_ => ChangePassword(), _ => CanChangePassword()));
            }
        }

        private bool CanUpdateProfile()
        {

            return !IsBusy && !string.IsNullOrWhiteSpace(FirstName) && !string.IsNullOrWhiteSpace(LastName) && !string.IsNullOrWhiteSpace(Email);
        }

        private async void UpdateProfile()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Updating profile...";


                if (CurrentUser != null)
                {
                    CurrentUser.FirstName = FirstName;
                    CurrentUser.LastName = LastName;
                    CurrentUser.Email = Email;
                    CurrentUser.PhoneNumber = PhoneNumber;
                    StatusMessage = "Profile updated successfully.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error updating profile: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool CanChangePassword()
        {
            return !IsBusy &&
                   !string.IsNullOrWhiteSpace(CurrentPassword) &&
                   !string.IsNullOrWhiteSpace(NewPassword) &&
                   !string.IsNullOrWhiteSpace(ConfirmPassword) &&
                   NewPassword == ConfirmPassword &&
                   NewPassword.Length >= 6;
        }

        private async void ChangePassword()
        {
            try
            {
                IsBusy = true;
                StatusMessage = "Changing password...";
                bool success = true;
                bool success = true; 

                if (success)
                {
                    StatusMessage = "Password changed.";
                    CurrentPassword = string.Empty;
                    NewPassword = string.Empty;
                    ConfirmPassword = string.Empty;
                }
                else
                {
                    StatusMessage = "Failed to change password. Please check your current password.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error changing password: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadUserData()
        {
            try
            {
                IsBusy = true;

            }
                int userId = 1; //dozmiany 
        }

            catch (Exception ex)
            {
                StatusMessage = $"Error loading user data: {ex.Message}";
            }
            finally
            {
                IsBusy = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
