namespace Hospital.Domain.Interfaces;

public interface INationalRegistryService
{
    Task<bool> ValidateCpr(string cpr);
}
