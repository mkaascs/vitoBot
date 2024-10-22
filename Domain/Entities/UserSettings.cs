using System.ComponentModel.DataAnnotations.Schema;

using Domain.Primitives;

namespace Domain.Entities;

public class UserSettings : Entity {
    public ulong ChatId { get; set; }
    
    [Column(TypeName = "decimal(6,5)")]
    public decimal DefaultChanceToSendMessage { get; set; }
    
    [Column(TypeName = "decimal(6,5)")]
    public decimal ChanceToSaveMessage { get; set; }
    
    [Column(TypeName = "decimal(6,5)")]
    public decimal ChanceToSaveTextMessage { get; set; }
}