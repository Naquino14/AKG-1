using System;
using c = System.Console;
using System.Buffers.Binary;

namespace ADIS
{
    public static class AKG1
    {
        /// ppl reading this must think im crazy or someth
        /// so, heres a disclaimer:
        /// Im not some top teir mathemetician
        /// im just some crazy highschooler with crazy ideas that arent backed by solid proof
        /// anyways, on with the brain vomit
        /// Rules:
        /// AKG Must not be used for encrypting data
        /// Generated keys must be 16 bytes (128 bits) in length
        /// The algorithm must generate exactly two keys that are ONLY related mathematically
        /// Foreword: This is probably going to be the most difficult algorithm to create and implement. 
        /// The fact that it took 3 MIT students and an entire year to do it is a bit scary.
        /// Even if its a little less secure, I want to try my best to NOT base this off of anything, at least for the time being.
        /// 
        /// Food for thought
        /// Finding keys from a specified input (usually random, and large) should be easy
        /// Finding the private key from the public key should be computationally difficult
        /// Finding the specified input from the public key should be computationally difficult
        /// 
        /// From what I understood from grazing over RSA is that it uses a bunch of, in terms
        /// of reversibility, computationally difficult mathematics. 
        /// 
        /// The enceyption and deceyption part of AKG-1 only happens once. 
        /// Speed is not a topic of worry for me.
        /// 
        /// Ideadump:
        /// > find a large input salt and mix it somehow with a difficult to reverse function
        /// > Look around, ideas can come from anywhere
        /// > calculus?
        /// 
        /// The basic idea is as such:
        /// If i perform a mathematical operation with the first key in a pair on data d, the same or similar mathematical
        /// operation with the 2nd key pair on data d gets the data d decrypted. 
        /// In RSA this mathematical operation is an exponent and its inverse.
        /// 
        /// prvk := private key
        /// pubk := public key
        /// cssk := ciphered shared secret key (gets thrown into arc)
        /// pssk =: plaintext shared secret key
        /// s := salt
        /// > p * pubk = c => encryption
        /// > c * prvk = p => decryption
        /// the problem here is that cssk / pubk = pssk
        /// 
        /// Other potential solutions:
        /// A key generation algorithm based on https://en.wikipedia.org/wiki/Diffie%E2%80%93Hellman_key_exchange
        /// Ok I said I wouldnt base this off of anything but this is much simpler than RSA.
        /// But this idea of 4 keys in total instead of 3 is a lot nicer. 
        /// https://upload.wikimedia.org/wikipedia/commons/thumb/4/46/Diffie-Hellman_Key_Exchange.svg/375px-Diffie-Hellman_Key_Exchange.svg.png
        /// This image illustrates the genius idea
        /// 
        /// Demo 1: Public key structure: 
        ///  0: mod byte 1
        ///  1: mod byte 2
        ///  2: mod byte 3
        ///  3: mod byte 4
        ///  4: exponent byte 1 | floating point
        ///  5: exponent byte 2
        ///  6: exponent byte 3
        ///  7: exponent byet 4
        ///  8: salty byte
        ///  9: coef 1 byte 1 | poly 1
        /// 10: coef 1 byte 2
        /// 11: coef 1 byte 3
        /// 12: coef 1 byte 4
        /// 13: coef 2 byte 1
        /// 14: coef 2 byte 2
        /// 15: coef 2 byte 3
        /// 16: coef 2 byte 4
        /// 17: coef 3 byte 1
        /// 18: coef 3 byte 2
        /// 19: coef 3 byte 3
        /// 20: coef 3 byte 4
        /// 21: coef 4 byte 1
        /// 22: coef 4 byte 2
        /// 23: coef 4 byte 3
        /// 24: coef 4 byte 4
        /// 25: coef 1 byte 1 | poly 2
        /// 26: coef 1 byte 2
        /// 27: coef 1 byte 3
        /// 28: coef 1 byte 4
        /// 29: coef 2 byte 1
        /// 30: coef 2 byte 2
        /// 31: coef 2 byte 3
        /// 32: coef 2 byte 4
        /// 33: coef 3 byte 1
        /// 34: coef 3 byte 2
        /// 35: coef 3 byte 3
        /// 36: coef 3 byte 4
        /// 37: coef 4 byte 1
        /// 38: coef 4 byte 2
        /// 39: coef 4 byte 3
        /// 40: coef 4 byte 4
        /// ok coming up with a base idea took alot less time than I thought LMFAO
        /// https://www.desmos.com/calculator/xnv1ng9lz4
        /// I can already tell its gonna be slow asf but i just want to get a working model for now
        /// as a side note im prob also not gonna bother with regions
        /// 
        /// Small update, to try to grasp the idea of public key cryptography,
        /// I have decided to implement a very basic diffie - hellman exchange first.

        public struct PublicKey
        {
            #region expirimental
            //public int mod; 
            //public float exponent; 
            //public byte salt; 
            //public int[] poly1Coefs; 
            //public int[] poly2Coefs;
            #endregion
            public int _modulus;
            public int _base;
        }

        private static byte[] GetSharedSecret(byte[] pmKey)
        {
            throw new NotImplementedException();
        }

        public static int SolvePoly(in int[] cs, int x)
        {
            // this can probably be made to be much faster
            int result = 0;
            for (int i = 3, j = 0; i > 0; i--, j++)
                result += cs[j] * EBSMK(x, i, 0xFF);
            result += cs[3];
            return result;
        }

        public static PublicKey DeMarshalPublicKey(byte[] incoming) // todo: new structure :(
        {
            var key = new PublicKey();
            #region expiremental
            //key.mod = B2I(incoming[0], incoming[1], incoming[2], incoming[3]);
            //key.exponent = B2F(in incoming);
            //key.salt = incoming[8];
            //key.poly1Coefs = new int[] // its 11 pm i cant be bothered to toss this in a loop rn
            //{
            //    B2I(incoming[9], incoming[10], incoming[11], incoming[12]),
            //    B2I(incoming[13], incoming[14], incoming[15], incoming[16]),
            //    B2I(incoming[17], incoming[18], incoming[19], incoming[20]),
            //    B2I(incoming[21], incoming[22], incoming[23], incoming[24])
            //};
            //key.poly2Coefs = new int[]
            //{
            //    B2I(incoming[25], incoming[26], incoming[27], incoming[28]),
            //    B2I(incoming[29], incoming[30], incoming[31], incoming[32]),
            //    B2I(incoming[33], incoming[34], incoming[35], incoming[36]),
            //    B2I(incoming[37], incoming[38], incoming[39], incoming[40])
            //};
            #endregion
            try
            {
                key._modulus = B2I(incoming[0], incoming[1], incoming[2], incoming[3]);
                key._base = B2I(incoming[4], incoming[5], incoming[6], incoming[7]);
            } catch (Exception ex)
            { throw new InvalidKeyStreamException(ex.Message, ex); }
            return key;
        }

        public static byte[] MarshalPublicKey(PublicKey key)
        {
            var buf = new byte[8];
            var m = I2B(key._modulus);
            var b = I2B(key._base);
            for (int i = 0; i < m.Length; i++)
            {
                buf[i] = m[i];
                buf[i + 4] = b[i];
            }
            return buf;
        }

        public static PublicKey ComputePublicKey(int seed = 1)
        {
            
            throw new NotImplementedException();
        }

        public static PublicKey GenerateKey()
        {
            c.WriteLine("Generating key...");
            Random random = new Random();
            var res = new PublicKey();
            for (;;)
            {
                int ctx = random.Next(1073741823, 2147483647);
                if (IsPrime(ctx))
                {
                    for(;;)
                    {
                        int primRoot = random.Next(1073741823, 2147483647);
                        if (PMMN(ctx, primRoot))
                        {
                            res._modulus = ctx;
                            res._base = primRoot;
                            return res;
                        }
                    }
                }
            }
        }

        public static bool PMMN(int p, int r)
        {
            var res = new int[p];
            for (int i = 1; i <= p - 1; i++)
                res[i - 1] = EBSMK(r, i, p);
            if (res.GroupBy(x => x).Any(g => g.Count() > 1))
                return false;
            else
                return true;
        }

        public static bool IsPrime(int n)
        {
            var res = n >= 2 && (n % 2 == 0) && (n == 2);
            for (int i = 3; i <= (int)MathF.Sqrt(n); i += 2)
                res = !(n % i == 0);
            return res;
        }

        /// <summary>
        /// Exponentation By Squaring Mod K
        /// </summary>
        public static int EBSMK(int x, int n, int k)
        {
            int res = 1;
            x %= k;
            if (x == 0)
                return 0;
            while (n > 0)
            {
                if ((n & 1) != 0)
                    res = res * x % k;
                n >>= 1;
                x = x * x % k;
            }
            return res;
        }

        public static int B2I(byte a, byte b, byte c, byte d)
        { return BinaryPrimitives.ReadInt32LittleEndian(new byte[] { a, b, c, d }); }

        public static float B2F(in byte[] a)
        {
            return (a.Length < 4)
                ? throw new ArgumentException()
                : BitConverter.ToSingle(a, 4);
        }

        public static byte[] I2B(int a)
        { return BitConverter.GetBytes(a); }
    }

    public class InvalidKeyStreamException : Exception
    {
        public InvalidKeyStreamException() { }
        public InvalidKeyStreamException(string msg) : base(msg) { }
        public InvalidKeyStreamException(string msg, Exception inner) : base(msg, inner) { }
    }
}
