using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CarChecker.Shared
{
    public class InspectionNote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonPropertyName("id")]
        public int InspectionNoteId { get; set; }

        [Required]
        public VehiclePart Location { get; set; }

        [Required]
        [StringLength(100)]
        public string Text { get; set; }

        public string PhotoUrl { get; set; }

        public void CopyFrom(InspectionNote other)
        {
            Location = other.Location;
            Text = other.Text;
            PhotoUrl = other.PhotoUrl;
        }
    }
}
