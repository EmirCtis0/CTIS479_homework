using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APP.Services
{
    public class UserService : Service<User>, IService<UserRequest, UserResponse>
    {
        public UserService(DbContext db) : base(db)
        {
        }

        public List<UserResponse> List()
        {
            // UserRoles ve Role'leri birlikte çekiyoruz
            return Query()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .OrderBy(entity => entity.Username)
                .Select(entity => new UserResponse
                {
                    Id = entity.Id,
                    Guid = entity.Guid,
                    Username = entity.Username,
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                    Gender = entity.Gender,
                    BirthDate = entity.BirthDate,
                    RegistrationDate = entity.RegistrationDate,
                    Score = entity.Score,
                    IsActive = entity.IsActive,
                    Address = entity.Address,

                    // Roles kolonunda gösterilecek metin
                    Roles = string.Join(", ",
                        entity.UserRoles.Select(ur => ur.Role.Name))
                })
                .ToList();
        }

        public UserResponse Item(int id)
        {
            var entity = Query()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .SingleOrDefault(c => c.Id == id);

            if (entity is null)
                return null;

            return new UserResponse
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Username = entity.Username,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                RegistrationDate = entity.RegistrationDate,
                Score = entity.Score,
                IsActive = entity.IsActive,
                Address = entity.Address,

                // Details sayfasında görünecek metin
                Roles = string.Join(", ",
                    entity.UserRoles.Select(ur => ur.Role.Name))
            };
        }

        public UserRequest Edit(int id)
        {
            var entity = Query()
                .Include(u => u.UserRoles)
                .SingleOrDefault(c => c.Id == id);

            if (entity is null)
                return null;

            return new UserRequest
            {
                Id = entity.Id,
                Username = entity.Username,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                IsActive = entity.IsActive,
                Address = entity.Address,

                // Mevcut roller Edit ekranındaki checkbox’larda işaretli gelsin
                RoleIds = entity.RoleIds
            };
        }

        public CommandResponse Create(UserRequest request)
        {
            if (Query().Any(c => c.Username == request.Username.Trim()))
                return Error("A user with this username already exists.");

            var entity = new User
            {
                Username = request.Username.Trim(),
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                IsActive = request.IsActive,
                Address = request.Address?.Trim(),
                Password = request.Password,
                RegistrationDate = DateTime.Now,
                Score = 0,

                // Seçilen rolleri User entity'sine geç
                RoleIds = request.RoleIds ?? new List<int>()
            };

            Create(entity);

            return Success("User created successfully.", entity.Id);
        }

        public CommandResponse Update(UserRequest request)
        {
            if (Query().Any(c => c.Id != request.Id && c.Username == request.Username.Trim()))
                return Error("A user with this username already exists.");

            var entity = Query()
                .Include(u => u.UserRoles)
                .SingleOrDefault(c => c.Id == request.Id);

            if (entity is null)
                return Error("User not found!");

            entity.Username = request.Username.Trim();
            entity.FirstName = request.FirstName?.Trim();
            entity.LastName = request.LastName?.Trim();
            entity.Gender = request.Gender;
            entity.BirthDate = request.BirthDate;
            entity.IsActive = request.IsActive;
            entity.Address = request.Address?.Trim();

            // Roller güncelleniyor
            entity.RoleIds = request.RoleIds ?? new List<int>();

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                entity.Password = request.Password;
            }

            Update(entity);

            return Success("User updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query().SingleOrDefault(c => c.Id == id);
            if (entity is null)
                return Error("User not found!");

            Delete(entity);

            return Success("User deleted successfully.", entity.Id);
        }
    }
}
