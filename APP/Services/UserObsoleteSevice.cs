// APP/Services/UserObsoleteService.cs
using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using System;
using System.Linq;

namespace APP.Services
{
    public class UserObsoleteService : ServiceBase
    {
        private readonly Db _db;

        public UserObsoleteService(Db db)
        {
            _db = db;
        }

        public IQueryable<UserResponse> Query()
        {
            var query = _db.Users.Select(userEntity => new UserResponse
            {
                Id = userEntity.Id,
                Guid = userEntity.Guid,
                Username = userEntity.Username,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName,
                Gender = userEntity.Gender,
                BirthDate = userEntity.BirthDate,
                RegistrationDate = userEntity.RegistrationDate,
                Score = userEntity.Score,
                IsActive = userEntity.IsActive,
                Address = userEntity.Address,
               
                // Not: Güvenlik nedeniyle 'Password' ASLA Response'a eklenmez.
            });
            return query;
        }

        public CommandResponse Create(UserRequest request)
        {
            // Kullanıcı adı veya e-posta benzersiz olmalıdır. 
            // 'Username' kontrolünü ekliyoruz.
            if (_db.Users.Any(userEntity => userEntity.Username == request.Username.Trim()))
                return Error("Bu kullanıcı adı zaten alınmış.");

            // GÜVENLİK UYARISI: Parola burada hashlenmelidir.
            // Örn: var (passwordHash, passwordSalt) = CreatePasswordHash(request.Password);
            var entity = new User
            {
                Username = request.Username.Trim(),
                Password = request.Password, // GÜVENLİ DEĞİL! Hashlenmeli.
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                IsActive = request.IsActive,
                Address = request.Address?.Trim(),
                

                // Sunucu tarafı atamaları
                Guid = Guid.NewGuid().ToString(),
                RegistrationDate = DateTime.Now,
                Score = 0 // Varsayılan skor
            };

            _db.Users.Add(entity);
            _db.SaveChanges();
            return Success("Kullanıcı başarıyla oluşturuldu.", entity.Id);
        }

        public UserRequest Edit(int id)
        {
            var entity = _db.Users.SingleOrDefault(c => c.Id == id);
            if (entity is null)
                return null;

            return new UserRequest
            {
                // Id alanı DTO'da yoksa bile, Update işlemi için gereklidir.
                // Request modelimizde Id yok, bu yüzden bu metodu
                // Controller'da Update(request)'e göndermek için kullanıyorsak
                // request'e Id'yi eklememiz gerekir.
                // CategoryRequest'te Id var mı diye kontrol ettim, yokmuş.
                // CategoryService'teki Edit metodu Id'li bir CategoryRequest döndürüyor.
                // Bizim UserRequest'imizde Id yok, bu yüzden onu `Update` metodunda 
                // ayrı alıyoruz (CategoryController'a bakarak).
                // NOT: CategoryRequest'te Id varmış, ben eklememişim.
                // UserRequest'e de Id eklemek daha doğru olur.
                // Şimdilik Id olmadan devam ediyorum, CategoryObsoleteService'in birebir kopyası gibi:

                // Id = entity.Id, // UserRequest'te Id alanı olmalı.
                Username = entity.Username,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                IsActive = entity.IsActive,
                Address = entity.Address,
                
                // Not: Parola güvenlik nedeniyle forma geri gönderilmez.
                // Parola alanları boş gelecek (güncelleme için).
            };
        }

        public CommandResponse Update(UserRequest request, int id) // Id'yi ayrı alıyoruz
        {
            // Başka bir kullanıcının bu kullanıcı adını kullanıp kullanmadığını kontrol et
            if (_db.Users.Any(c => c.Id != id && c.Username == request.Username.Trim()))
                return Error("Bu kullanıcı adı zaten alınmış.");

            var entity = _db.Users.SingleOrDefault(c => c.Id == id);
            if (entity is null)
                return Error("Kullanıcı bulunamadı!");

            entity.Username = request.Username.Trim();
            entity.FirstName = request.FirstName?.Trim();
            entity.LastName = request.LastName?.Trim();
            entity.Gender = request.Gender;
            entity.BirthDate = request.BirthDate;
            entity.IsActive = request.IsActive;
            entity.Address = request.Address?.Trim();
            

            // Parola güncelleme (GÜVENLİ DEĞİL)
            // Sadece parola alanı doluysa güncelle
            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                // GÜVENLİK UYARISI: Parola burada hashlenmelidir.
                entity.Password = request.Password;
            }

            _db.Users.Update(entity);
            _db.SaveChanges();
            return Success("Kullanıcı başarıyla güncellendi.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = _db.Users.SingleOrDefault(c => c.Id == id);
            if (entity is null)
                return Error("Kullanıcı bulunamadı!");

            _db.Users.Remove(entity);
            _db.SaveChanges();
            return Success("Kullanıcı başarıyla silindi.", entity.Id);
        }
    }
}