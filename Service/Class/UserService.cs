﻿using System.Collections.Generic;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using HubUfpr.Service.Interface;

namespace HubUfpr.Service.Class
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetUserList()
        {
            var obj = new List<User>();
            return obj;
        }

        public User GetToken(string username, string password)
        {
            var passwordHash = Utils.HashUtil.GetSha256FromString(password);

            var ret = _userRepository.ValidateUser(username, passwordHash);

            return ret;
        }

        public void InsertUser(string name, string senha, string email, string grr, bool isVendedor)
        {
            var passwordHash = Utils.HashUtil.GetSha256FromString(senha);

            _userRepository.InsertUser(name, passwordHash, email, grr, isVendedor);
        }

        public bool IsEmailInUse(string email)
        {
            return _userRepository.IsEmailInUse(email);
        }

        public bool IsGRRInUse(string grr)
        {
            return _userRepository.IsGRRInUse(grr);
        }

        public void UpdateLastLoginTime(int id)
        {
            _userRepository.UpdateLastLoginTime(id);
        }
    }
}