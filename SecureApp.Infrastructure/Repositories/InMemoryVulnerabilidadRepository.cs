using System.Collections.Concurrent;
using SecureApp.Domain.Users.ValueObjects;
using SecureApp.Domain.Vulnerabilities;
using SecureApp.Domain.Vulnerabilities.ValueObjects;

namespace SecureApp.Infrastructure.Repositories;

public class InMemoryVulnerabilidadRepository : IVulnerabilidadRepository
{
    private readonly ConcurrentDictionary<VulnerabilidadId, Vulnerabilidad> _vulnerabilidades = new();

    public Task<Vulnerabilidad?> GetByIdAsync(VulnerabilidadId id)
    {
        _vulnerabilidades.TryGetValue(id, out var vulnerabilidad);
        return Task.FromResult(vulnerabilidad);
    }

    public Task<IEnumerable<Vulnerabilidad>> GetAllAsync()
    {
        return Task.FromResult(_vulnerabilidades.Values.AsEnumerable());
    }

    public Task<IEnumerable<Vulnerabilidad>> GetByUsuarioIdAsync(UsuarioId usuarioId)
    {
        var vulnerabilidades = _vulnerabilidades.Values
            .Where(v => v.DescubiertaPor == usuarioId)
            .AsEnumerable();
        return Task.FromResult(vulnerabilidades);
    }

    public Task AddAsync(Vulnerabilidad vulnerabilidad)
    {
        if (!_vulnerabilidades.TryAdd(vulnerabilidad.Id, vulnerabilidad))
        {
            throw new InvalidOperationException("No se pudo agregar la vulnerabilidad");
        }
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Vulnerabilidad vulnerabilidad)
    {
        if (!_vulnerabilidades.TryUpdate(vulnerabilidad.Id, vulnerabilidad, _vulnerabilidades[vulnerabilidad.Id]))
        {
            throw new InvalidOperationException("No se pudo actualizar la vulnerabilidad");
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(VulnerabilidadId id)
    {
        if (!_vulnerabilidades.TryRemove(id, out _))
        {
            throw new InvalidOperationException("No se pudo eliminar la vulnerabilidad");
        }
        return Task.CompletedTask;
    }
} 