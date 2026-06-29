using System.Runtime.Serialization;

namespace TripPlanner.Contracts.Dtos;

[DataContract]
public sealed class ValidationResultDto
{
    [DataMember]
    public bool IsValid { get; set; }

    [DataMember]
    public List<string> Errors { get; set; } = new();

    public static ValidationResultDto Success()
    {
        return new ValidationResultDto { IsValid = true };
    }

    public static ValidationResultDto Fail(IEnumerable<string> errors)
    {
        return new ValidationResultDto
        {
            IsValid = false,
            Errors = errors.ToList()
        };
    }
}
