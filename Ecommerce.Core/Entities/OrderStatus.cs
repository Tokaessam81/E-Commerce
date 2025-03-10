using System.Runtime.Serialization;
namespace Ecommerce.Core.Entities
{
    public enum OrderStatus
    {
        [EnumMember(Value="Waiting")]
        Waiting,    
        [EnumMember(Value= "Pending")]
        Pending,   
        [EnumMember(Value= "Approved")]
        Approved,
        [EnumMember(Value= "Cancelled")]
        Cancelled
    }
}
