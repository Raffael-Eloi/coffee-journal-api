namespace Coffee.Journal.Domain.Entities;

public interface IAuditable
{
    string CreatedBy { get; set; }

    string UpdatedBy { get; set; }
}
