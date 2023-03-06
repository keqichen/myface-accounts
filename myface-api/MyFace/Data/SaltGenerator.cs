using System;
using MyFace.Models.Database;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace MyFace.Data;

public static class SaltGenerator
{
    public static byte[] GetSalt()
    {
        byte[] salt = new byte[128 / 8];
        using (var rngCsp = new RNGCryptoServiceProvider())
        {
            rngCsp.GetNonZeroBytes(salt);
        }
        
        return(salt);

        //byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
    }

}

