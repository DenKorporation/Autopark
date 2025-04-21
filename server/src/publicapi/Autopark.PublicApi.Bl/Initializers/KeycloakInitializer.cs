using Autopark.Common.Attributes;
using Autopark.Common.Bl.Services.Keycloak.Interfaces;
using Autopark.Common.Constants;
using FS.Keycloak.RestApiClient.Model;

namespace Autopark.PublicApi.Bl.Initializers;

[ServiceAsSelf]
public class KeycloakInitializer(IUserService userService, IGroupService groupService)
{
    private readonly UserRepresentation[] _users =
    [
        new()
        {
            Email = "test@example.com",
            FirstName = "Test",
            LastName = "User",
            Enabled = true,
            EmailVerified = true,
            Credentials = [
            new CredentialRepresentation()
            {
                Temporary = false,
                Type = "password",
                Value = "Pass123$"
            }]
        }
    ];

    public async Task<IList<Guid>> SeedAsync(CancellationToken cancellationToken = default)
    {
        var result = new List<Guid>();

        foreach (var user in _users)
        {
            var extGroup = await groupService.GetGroupByNameAsync(user.Email, deepLoad: true, cancellationToken);

            if (extGroup is null)
            {
                await groupService.CreateGroupAsync(user.Email, cancellationToken);

                extGroup = await groupService.GetGroupByNameAsync(user.Email, deepLoad: true, cancellationToken);
            }

            var adminGroup = extGroup!.SubGroups.First(x => x.Name.EndsWith(Roles.Administrator));
            var adminGroupId = Guid.Parse(adminGroup.Id);
            
            var extUser = await userService.GetUserByEmailAsync(user.Email, cancellationToken);

            if (extUser is null)
            {
                user.Groups = [adminGroup!.Path];

                await userService.CreateUserAsync(user, cancellationToken);
            }
            else
            {
                var userId = Guid.Parse(extUser.Id);
                var userGroups = await userService.GetUserGroupByPrefixAsync(userId, user.Email, cancellationToken);

                if (userGroups.Any())
                {
                    foreach (var group in userGroups)
                    {
                        await userService.RemoveUserFromGroupAsync(userId, Guid.Parse(group.Id), cancellationToken);
                    }
                }

                await userService.AddUserToGroupAsync(userId, adminGroupId, cancellationToken);
            }

            result.Add(Guid.Parse(extGroup!.Id));
        }

        return result;
    }
}