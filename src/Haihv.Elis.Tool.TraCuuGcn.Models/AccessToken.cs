namespace Haihv.Elis.Tool.TraCuuGcn.Models;

public record AccessToken(
    string Token, 
    string TokenId,
    string RefreshToken, 
    DateTime Expiry);