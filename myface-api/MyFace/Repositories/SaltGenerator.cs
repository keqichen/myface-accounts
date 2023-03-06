using System;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using MyFace.Models.Database;

namespace MyFace.Repositories;

public static class SaltGenerator
{
    public static byte[] GetSalt()
    {
        // byte[] salt = new byte[128 / 8];
        // using (var rngCsp = new RNGCryptoServiceProvider())
        // {
        //     rngCsp.GetNonZeroBytes(salt);
        // }
        
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        return(salt);
    }

}

