namespace Coffee.Journal.Domain.Entities;

public sealed class ProfileRole : IAuditable
{
    public Guid ProfileId { get; set; }

    public Guid RoleId { get; set; }

    public Role? Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string CreatedBy { get; set; } = string.Empty;

    public string UpdatedBy { get; set; } = string.Empty;
}
