using System.Collections.Generic;
using System.Linq;
using MyFace.Models.Database;
using MyFace.Models.Request;
using MyFace.Repositories;
using System.Web;
using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace MyFace.Repositories
{
    public interface IUsersRepo
    {
        IEnumerable<User> Search(UserSearchRequest search);
        int Count(UserSearchRequest search);
        User GetById(int id);
        User Create(CreateUserRequest newUser);
        User Update(int id, UpdateUserRequest update);
        void Delete(int id);
        bool HasAccess(string authHeader);
    }

    public class UsersRepo : IUsersRepo
    {
        private readonly MyFaceDbContext _context;

        public UsersRepo(MyFaceDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> Search(UserSearchRequest search)
        {
            return _context.Users
                .Where(p => search.Search == null ||
                            (
                                p.FirstName.ToLower().Contains(search.Search) ||
                                p.LastName.ToLower().Contains(search.Search) ||
                                p.Email.ToLower().Contains(search.Search) ||
                                p.Username.ToLower().Contains(search.Search)
                            ))
                .OrderBy(u => u.Username)
                .Skip((search.Page - 1) * search.PageSize)
                .Take(search.PageSize);
        }

        public int Count(UserSearchRequest search)
        {
            return _context.Users
                .Count(p => search.Search == null ||
                            (
                                p.FirstName.ToLower().Contains(search.Search) ||
                                p.LastName.ToLower().Contains(search.Search) ||
                                p.Email.ToLower().Contains(search.Search) ||
                                p.Username.ToLower().Contains(search.Search)
                            ));
        }

        public bool HasAccess(string authHeader)
        {
            string username;
            string password;
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');
                username = usernamePassword.Substring(0, seperatorIndex);
                password = usernamePassword.Substring(seperatorIndex + 1);
            }
            else
            {
                //Handle what happens if that isn't the case
                throw new Exception("The authorization header is either empty or isn't Basic.");
            }

            // checking header username against exisisting db username
            var dbUser = _context.Users.FirstOrDefault(u => u.Username == username);
            var dbpasswordSalt = dbUser.PasswordSalt;
            // to hash header password
            var inputHashedPassword = HashGenerator.GetHashedPassword(dbpasswordSalt, password);

            // var dbuser = dbUser.Username;
            var dbPassword = dbUser.PasswordHash;
            if (inputHashedPassword == dbPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //an easier endpoint to add basic auth;
        public User GetById(int id)
        {
            //  var authHeader = HttpContext.Request.Headers["Authorization"];
            // HttpContext httpContext = HttpContext.Current;
            // string authHeader = this.httpContext.Request.Headers["Authorization"];
            return _context.Users
                .Single(user => user.Id == id);
        }

        public User Create(CreateUserRequest newUser)
        {
            var passwordSalt = SaltGenerator.GetSalt();
            var insertResponse = _context.Users.Add(new User
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                PasswordSalt = passwordSalt,
                PasswordHash = HashGenerator.GetHashedPassword(passwordSalt, newUser.Password),
                Username = newUser.Username,
                ProfileImageUrl = newUser.ProfileImageUrl,
                CoverImageUrl = newUser.CoverImageUrl,
            });
            _context.SaveChanges();

            return insertResponse.Entity;
        }

        public User Update(int id, UpdateUserRequest update)
        {
            var user = GetById(id);

            user.FirstName = update.FirstName;
            user.LastName = update.LastName;
            user.Username = update.Username;
            user.Email = update.Email;
            user.ProfileImageUrl = update.ProfileImageUrl;
            user.CoverImageUrl = update.CoverImageUrl;
            _context.Users.Update(user);
            _context.SaveChanges();

            return user;
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}