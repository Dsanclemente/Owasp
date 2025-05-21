using System.Collections.Concurrent;
using SecureApp.Domain.Users;
using SecureApp.Domain.Users.ValueObjects;

namespace SecureApp.Infrastructure.Repositories;

public class InMemoryUsuarioRepository : IUsuarioRepository
{
    private readonly ConcurrentDictionary<UsuarioId, Usuario> _usuarios = new();

    public Task<Usuario?> GetByIdAsync(UsuarioId id)
    {
        _usuarios.TryGetValue(id, out var usuario);
        return Task.FromResult(usuario);
    }

    public Task<Usuario?> GetByEmailAsync(string email)
    {
        var usuario = _usuarios.Values.FirstOrDefault(u => u.Email.Value == email);
        return Task.FromResult(usuario);
    }

    public Task<IEnumerable<Usuario>> GetAllAsync()
    {
        return Task.FromResult(_usuarios.Values.AsEnumerable());
    }

    public Task AddAsync(Usuario usuario)
    {
        if (!_usuarios.TryAdd(usuario.Id, usuario))
        {
            throw new InvalidOperationException("No se pudo agregar el usuario");
        }
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Usuario usuario)
    {
        if (!_usuarios.TryUpdate(usuario.Id, usuario, _usuarios[usuario.Id]))
        {
            throw new InvalidOperationException("No se pudo actualizar el usuario");
        }
        return Task.CompletedTask;
    }

    public Task DeleteAsync(UsuarioId id)
    {
        if (!_usuarios.TryRemove(id, out _))
        {
            throw new InvalidOperationException("No se pudo eliminar el usuario");
        }
        return Task.CompletedTask;
    }
} 