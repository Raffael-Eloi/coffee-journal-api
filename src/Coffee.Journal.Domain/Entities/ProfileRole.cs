namespace Coffee.Journal.Domain.Entities;

public sealed class ProfileRole
{
    public Guid ProfileId { get; set; }

    public Guid RoleId { get; set; }

    public Role? Role { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
