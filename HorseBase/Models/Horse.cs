using System.ComponentModel.DataAnnotations;

namespace HorseBase.Models
{
    public class Horse
    {
        public int Id { get; set; }

        [Required, Display(Name = "Номер")]
        public int Number { get; set; }

        public int BreedId { get; set; }

        public Breed? Breed { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d M yyyy}")]
        [Required, Display(Name = "Година на раждане")]
        public DateTime BirhtYear { get; set; }

        [Required, Display(Name = "Пол")]
        public string Gender { get; set; }

        [Required, Display(Name = "Височина")]
        public int Height { get; set; }

        [DataType(DataType.Currency)]
        [Required, Display(Name = "Цена")]
        public double Price { get; set; }

        [Display(Name = "Снимки")]
        public string? PhotoPath { get; set; }
    }
}
