using NurseryMart.Entities;

namespace NurseryMart.IRepository
{
    public interface IEntity
    {
        public string? Id { set; get; }
        public Trail Trail { set; get; }
    }
}
