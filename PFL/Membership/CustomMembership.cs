using BLL;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using PFL.Entities.EntityModels;

namespace PFL.Membership
{
    public class CustomMembership : MembershipProvider
    {
        public override bool EnablePasswordRetrieval => throw new NotImplementedException();

        public override bool EnablePasswordReset => throw new NotImplementedException();

        public override bool RequiresQuestionAndAnswer => throw new NotImplementedException();

        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override int MaxInvalidPasswordAttempts => throw new NotImplementedException();

        public override int PasswordAttemptWindow => throw new NotImplementedException();

        public override bool RequiresUniqueEmail => throw new NotImplementedException();

        public override MembershipPasswordFormat PasswordFormat => throw new NotImplementedException();

        public override int MinRequiredPasswordLength => throw new NotImplementedException();

        public override int MinRequiredNonAlphanumericCharacters => throw new NotImplementedException();

        public override string PasswordStrengthRegularExpression => throw new NotImplementedException();

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {

            using (PFLContext db = new PFLContext())
            {
                User user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = username,
                    Password = SHAHelper.SHA512Generator(password),
                    Email = email,
                    CreationDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false
                };

                db.Users.Add(user);
                db.SaveChanges();

                status = MembershipCreateStatus.Success;

                return new CustomMembershipUser(user);
            }
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {

            using (PFLContext dbContext = new PFLContext())
            {
                var user = dbContext.Users
                    .Include(x=>x.Roles)
                    .FirstOrDefault(x => string.Compare(username, x.UserName, StringComparison.OrdinalIgnoreCase) == 0);

                if (user == null)
                {
                    return null;
                }
                var selectedUser = new CustomMembershipUser(user);

                return selectedUser;
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            string _password = SHAHelper.SHA512Generator(password);

            using (PFLContext dbContext = new PFLContext())
            {
                var user = (from us in dbContext.Users
                            where
                            string.Compare(username, us.UserName, StringComparison.OrdinalIgnoreCase) == 0
                            && us.IsActive && !us.IsDeleted
                            select us).FirstOrDefault();

                if (user != null)
                {
                    if ((string.Compare(_password, user.Password, StringComparison.OrdinalIgnoreCase) == 0))
                        return true;
                }

                return false;
            }
        }
    }
}