using System.ComponentModel.DataAnnotations;

namespace Domain.Primitives;

public abstract class Entity {
    [Key]
    public int Id { get; set; }
}