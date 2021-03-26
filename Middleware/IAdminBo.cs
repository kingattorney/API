using System;
using System.Threading.Tasks;
using WebAdmin_KingAnttorny.ViewModels;
using API.KingAttorney.ViewModels;
namespace API.KingAttorney.Middleware
{
    public interface IAdminBo
    {
        // Help check authentication
        Task<VMToken> Authentication(string username, string password);

        // Help refresh token
        ResponseApi RefeshToken(string refreshToken);
    }
}
