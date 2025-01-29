namespace Application.Models;

public record PagedResultQueryBase(uint PageSize = 10, uint PageNumber = 1);
