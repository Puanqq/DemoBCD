using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.EntityFramework.Entities
{
    public class Entity<TPrimaryKey> : IEntity<TPrimaryKey> where TPrimaryKey : struct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TPrimaryKey Id { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? ModifiedTime { get; set; }

        public Guid? CreatedUser { get; set; }

        public Guid? ModifiedUser { get; set; }
    }

    public interface IEntity<TPrimaryKey> where TPrimaryKey : struct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TPrimaryKey Id { get; set; }

        public Guid? CreatedUser { get; set; }

        public Guid? ModifiedUser { get; set; }
    }
}
