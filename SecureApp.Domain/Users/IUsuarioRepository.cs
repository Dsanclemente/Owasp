using SecureApp.Domain.Users.ValueObjects;

namespace SecureApp.Domain.Users;

public interface IUsuarioRepository
{
    Task<Usuario?> GetByIdAsync(UsuarioId id);
    Task<Usuario?> GetByEmailAsync(string email);
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task AddAsync(Usuario usuario);
    Task UpdateAsync(Usuario usuario);
    Task DeleteAsync(UsuarioId id);
} 