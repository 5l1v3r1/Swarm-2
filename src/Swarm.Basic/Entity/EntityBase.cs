using System.ComponentModel.DataAnnotations;

namespace Swarm.Basic.Entity
{
    public abstract class EntityBase<T> : IEntity<T>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public T Id { get; set; }
    }
}