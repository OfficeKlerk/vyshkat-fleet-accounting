namespace MyVyshKat.Application;

public record InspectionResult(bool Accepted, string? RejectionReason = null);