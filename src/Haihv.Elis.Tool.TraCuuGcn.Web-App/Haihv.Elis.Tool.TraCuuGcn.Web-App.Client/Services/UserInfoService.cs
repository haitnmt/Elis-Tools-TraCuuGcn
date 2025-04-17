using System.Net.Http.Json;

namespace Haihv.Elis.Tool.TraCuuGcn.WebLib.Services;

   public class UserInfoService(HttpClient httpClient)
   {
       public async Task<UserInfo?> GetUserInfoAsync()
       {
           return await httpClient.GetFromJsonAsync<UserInfo>("authentication/userInfos");
       }
   }
   
   public class UserInfo
   {
       public string Name { get; set; } = string.Empty;
       public string PreferredUsername { get; set; } = string.Empty;
       public string Email { get; set; } = string.Empty;
       public string FamilyName { get; set; } = string.Empty;
       public string DisplayName { get; set; } = string.Empty;
       public bool EmailVerified { get; set; }
       
   }