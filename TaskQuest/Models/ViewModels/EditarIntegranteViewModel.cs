using System.ComponentModel.DataAnnotations;

namespace TaskQuest.ViewModels
{
    public class EditarIntegranteViewModel
    {
    	[Required]
        public int UserId { get; set; }

        [Required]
        public int GrupoId { get; set; }
        
    }
}