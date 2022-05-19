namespace Loan2022.ViewModels;

public class MetaDataConfigViewModel
{
    public MetaDataHeaderModel MetaDataHeader { get; set; }

    public string CanonicalUrl { get; set; }

    public string AlternateUrl { get; set; }
        
    public int PageNumber { get; set; } = 1;
}
    
public class MetaDataHeaderModel
{
    public string Title { get; set; } = "Money 24";

    public string MetaTitle { get; set; } = "Money 24";

    public string MetaKeyword { get; set; } =
        "money, 24, 2022";

    public string MetaDescription { get; set; } =
        "Money 24";

    public string MetaImage { get; set; }

    public string MetaRobots { get; set; } = "noindex,nofollow";

    // public string MetaViewport { get; set; } = "width=device-width, initial-scale=1.0, viewport-fit=cover";
}