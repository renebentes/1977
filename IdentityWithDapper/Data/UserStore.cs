using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using IdentityWithDapper.Models;
using Microsoft.AspNetCore.Identity;

namespace IdentityWithDapper.Data
{
    public class UserStore :
        IUserStore<ApplicationUser>,
        IUserEmailStore<ApplicationUser>,
        IUserPhoneNumberStore<ApplicationUser>
    {
        private readonly string _connectionString;

        public UserStore(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);

                var query = $@"INSERT INTO [ApplicationUser]
                                ([UserName],
                                    [NormalizedUserName],
                                    [Email],
                                    [NormalizedEmail],
                                    [EmailConfirmed],
                                    [PasswordHash],
                                    [PhoneNumber],
                                    [PhoneNumberConfirmed],
                                    [TwoFactorEnabled])
                                VALUES (
                                    @{nameof(ApplicationUser.UserName)},
                                    @{nameof(ApplicationUser.NormalizedUserName)},
                                    @{nameof(ApplicationUser.Email)},
                                    @{nameof(ApplicationUser.NormalizedEmail)},
                                    @{nameof(ApplicationUser.EmailConfirmed)},
                                    @{nameof(ApplicationUser.PasswordHash)},
                                    @{nameof(ApplicationUser.PhoneNumber)},
                                    @{nameof(ApplicationUser.PhoneNumberConfirmed)},
                                    @{nameof(ApplicationUser.TwoFactorEnabled)});
                                SELECT CAST(SCOPE_IDENTITY() as int)";

                user.Id = await connection.QuerySingleAsync<int>(query, user);
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($"DELETE FROM [ApplicationUser] WHERE [Id] = @{nameof(ApplicationUser.Id)}", user);
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            // Nothing to dispose
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM [ApplicationUser]
                    WHERE [NormalizedEmail] = @{nameof(normalizedEmail)}", new { normalizedEmail });
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM [ApplicationUser]
                    WHERE [Id] = @{nameof(userId)}", new { userId });
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken);
            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"SELECT * FROM [ApplicationUser]
                    WHERE [NormalizedUserName] = @{nameof(normalizedUserName)}", new { normalizedUserName });
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPhoneNumberAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(cancellationToken);
                await connection.ExecuteAsync($@"UPDATE [ApplicationUser] SET
                    [UserName] = @{nameof(ApplicationUser.UserName)},
                    [NormalizedUserName] = @{nameof(ApplicationUser.NormalizedUserName)},
                    [Email] = @{nameof(ApplicationUser.Email)},
                    [NormalizedEmail] = @{nameof(ApplicationUser.NormalizedEmail)},
                    [EmailConfirmed] = @{nameof(ApplicationUser.EmailConfirmed)},
                    [PasswordHash] = @{nameof(ApplicationUser.PasswordHash)},
                    [PhoneNumber] = @{nameof(ApplicationUser.PhoneNumber)},
                    [PhoneNumberConfirmed] = @{nameof(ApplicationUser.PhoneNumberConfirmed)},
                    [TwoFactorEnabled] = @{nameof(ApplicationUser.TwoFactorEnabled)}
                    WHERE [Id] = @{nameof(ApplicationUser.Id)}", user);
            }

            return IdentityResult.Success;
        }
    }
}