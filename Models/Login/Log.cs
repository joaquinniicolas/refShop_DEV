using System.ComponentModel.DataAnnotations;

namespace refShop_DEV.Models.Login
{
    public class Log
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TableName { get; set; }

        [Required]
        public int RecordId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Operation { get; set; }

        [Required]
        public int UserId { get; set; }

        [MaxLength(50)]
        public string ColumnName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public virtual User User { get; set; }
    }


}
