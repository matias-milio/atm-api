using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ATM.Api.RequestModels
{
    public class LoginRequest
    {
        [Description("16 Digit Card Number")]
        [Required]
        public string CardNumber { get; set; }

        [Description("4 Digit PIN Number")]
        [Required]
        public string PIN { get; set; }
    }
}
