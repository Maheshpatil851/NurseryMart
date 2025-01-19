using NurseryMart.Entities;
using System.ComponentModel.DataAnnotations;

namespace NurseryMart.IRepository
{
    public interface IEntity
    {
        [Key]
        public int Id { set; get; }
        public Trail Trail { set; get; }
    }
}
