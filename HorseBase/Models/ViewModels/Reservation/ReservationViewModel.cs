using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HorseBase.Models.ViewModels
{
    public class ReservationViewModel
    {
        [Display(Name = "Потребител")]
        public string? UserId { get; set; }

        [Required, Display(Name = "Кон")]
        [ForeignKey("Horse")]
        public int HorseId { get; set; }
        public Horse? Horse { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy - H:mm}")]
        [Required, Display(Name = "Дата и час на вземане")]
        public DateTime TakeHour { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d/M/yyyy - H:mm}")]
        [Required, Display(Name = "Дата и час на връщане")]
        public DateTime ReturnHour { get; set; }

        [Required, Display(Name = "Цена")]
        public decimal Price { get; set; }
    }
}
