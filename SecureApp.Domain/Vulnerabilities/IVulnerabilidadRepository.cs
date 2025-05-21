using SecureApp.Domain.Users.ValueObjects;
using SecureApp.Domain.Vulnerabilities.ValueObjects;

namespace SecureApp.Domain.Vulnerabilities;

public interface IVulnerabilidadRepository
{
    Task<Vulnerabilidad?> GetByIdAsync(VulnerabilidadId id);
    Task<IEnumerable<Vulnerabilidad>> GetAllAsync();
    Task<IEnumerable<Vulnerabilidad>> GetByUsuarioIdAsync(UsuarioId usuarioId);
    Task AddAsync(Vulnerabilidad vulnerabilidad);
    Task UpdateAsync(Vulnerabilidad vulnerabilidad);
    Task DeleteAsync(VulnerabilidadId id);
} 