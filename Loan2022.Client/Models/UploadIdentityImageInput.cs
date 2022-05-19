namespace Loan2022.Client.Models;

public class UploadIdentityImageInput
{
    public IFormFile IdentityCardFrontFace { get; set; }
    public IFormFile IdentityCardBackFace { get; set; }
    public IFormFile IdentityAvatar { get; set; }

    public bool CheckNull()
    {
        return IdentityAvatar == null || IdentityCardBackFace == null || IdentityCardFrontFace == null;
    }
}