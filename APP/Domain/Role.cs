using CORE.APP.Domain;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace APP.Domain
{
    public class Role : Entity
    {
        [Required, StringLength(200)]
        public string Name { get; set; }

        // Many-to-many'nin bir ucu: bir rolün birden fazla UserRole kaydı olabilir
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();

        // Kolay kullanım için: hangi user'lara atanmış?
        [NotMapped]
        public List<int> UserIds
        {
            get => UserRoles.Select(ur => ur.UserId).ToList();
            set => UserRoles = value?.Select(userId => new UserRole { UserId = userId }).ToList()
                              ?? new List<UserRole>();
        }
    }
}
