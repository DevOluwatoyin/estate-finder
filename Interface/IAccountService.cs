using System;

public interface IAccountService
{
    Task<Response<string>> RegisterAsync(RegisterRequest request);
    Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);

}
