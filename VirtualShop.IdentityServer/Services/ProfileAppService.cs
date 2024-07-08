using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VirtualShop.IdentityServer.Data;

namespace VirtualShop.IdentityServer.Services
{
    public class ProfileAppService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        public ProfileAppService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            //id do identityserver
            string id = context.Subject.GetSubjectId();

            //localizar por id
            ApplicationUser user = await _userManager.FindByIdAsync(id);

            //criar claimsprincipal para o user
            ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory
                                                .CreateAsync(user);

            //define coleção de claims para o user e inclui o sobrenome e nome
            List<Claim> claims = userClaims.Claims.ToList();
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));

            //verificar se o userManager suporta roles
            if(_userManager.SupportsUserRole)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);

            //percorrer lista
                foreach(string role in roles)
                {
                    //adicionar role na claim
                    claims.Add(new Claim(JwtClaimTypes.Role, role));

                    if (_roleManager.SupportsRoleClaims) 
                    { 
                        //localizar perfil
                        IdentityRole identityRole = await _roleManager
                                                    .FindByNameAsync(role);
                       
                        //incluir perfil
                        if(identityRole != null)
                        {
                            claims.AddRange(await _roleManager
                                            .GetClaimsAsync(identityRole));
                        }
                    }

                }
            }
            //retorna claims no contexto
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            //obtem o id do usuario no IS
            string userid = context.Subject.GetSubjectId();

            //localiza o usuário
            ApplicationUser user = await _userManager.FindByIdAsync(userid);

            //verifica se está ativo
            context.IsActive = user is not null;
        }
    }
}
