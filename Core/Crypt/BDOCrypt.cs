using Core.Extension;
using Core.Utils;
using System;
using System.IO;

namespace Core.Crypt
{
    public class BDOCrypt
    {
        private readonly Random _random;

        private byte[] _sessionkey;
        private byte[] _xorKey;
        public byte[] AesKey { get; }

        private readonly byte[] _frameWorkData;

        public BDOCrypt(int seed)
        {
            _random = new Random(seed);

            _frameWorkData = new byte[119];
            AesKey = new byte[16];

            InitializeFramework();
            InitializeAes();
            InitializeXor();
        }

        private void InitializeXor()
        {
            _xorKey = new byte[]
            {
0xC2,0xAB,0xD4,0xF0,0x5F,0x0C,0x68,0xBC,0xBB,0x8D,0x5C,0x42,0x70,0xBF,0xB3,0x5C,0x53,0x58,
0x79,0x54,0x4C,0x07,0xDA,0xA9,0xD4,0x8B,0xA5,0xCD,0xAD,0x69,0x2E,0x8C,0x83,0x25,0x04,0xE9,
0x63,0x89,0x49,0x0F,0x03,0x7E,0xD0,0x00,0x72,0xBC,0x57,0xF9,0x69,0x87,0x47,0x6D,0xA4,0x22,
0x8F,0xBF,0x60,0x00,0xE9,0xAD,0xDB,0x9C,0x8A,0x52,0x4E,0xD3,0x34,0x4D,0x06,0x01,0xC7,0x3C,
0x83,0xB7,0xB3,0x2C,0x19,0xB6,0x0F,0x78,0xA5,0x6A,0xFD,0x31,0x4F,0x0F,0xAE,0x89,0xC3,0x75,
0x9C,0x57,0xE0,0x4C,0x01,0x89,0x98,0x8D,0x30,0xB6,0x56,0xBC,0x34,0xBB,0x53,0xB9,0xE7,0xD7,
0x65,0xC3,0x78,0x58,0x24,0x44,0x9A,0x3E,0x41,0xA1,0x57,0x4F,0x94,0x82,0xC1,0x0D,0x26,0x3E,
0xE6,0x5C,0x93,0xA9,0x50,0xEF,0x4D,0xD3,0x34,0x4D,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00
            };
        }

        private void InitializeFramework()
        {
            // Randomize framework data.
            _random.NextBytes(_frameWorkData);

            // Set first 2 bytes to 0.
            _frameWorkData[0] = 0;
            _frameWorkData[1] = 0;
        }

        private void InitializeAes()
        {
            // OK
            // This function generates the correct output.
            var index = 0;

            do
            {
                var v6 = (uint)Sub_F48550(index);
                var v7 = index % 3;
                byte v9;
                if (v7 != 0)
                {
                    if (v7 == 1)
                    {
                        var v10 = (byte)(index % 8);

                        if (v10 < 1)
                            v10 = 1;

                        v9 = (byte)(v6 << v10 | v6 >> 8 - v10);
                    }
                    else
                    {
                        v9 = (byte)(v6 >> 4 | 16 * v6);
                    }
                }
                else
                {
                    var v11 = index % 8;

                    if (v11 < 1)
                        v11 = 1;

                    v9 = (byte)(v6 >> v11 | v6 << 8 - v11);
                }

                var v12 = (uint)Sub_F48600(v9);
                byte result;
                byte v15;
                byte v16;
                if (v7 != 0)
                {
                    if (v7 != 1)
                    {
                        result = (byte)(v12 >> 4 | 16 * v12);
                        AesKey[index++] = result;
                        continue;
                    }

                    var v14 = index % 8;
                    if (v14 < 1)
                        v14 = 1;

                    v15 = (byte)(v12 << v14);
                    v16 = (byte)(v12 >> 8 - v14);
                }
                else
                {
                    var v17 = index % 8;
                    if (v17 < 1)
                        v17 = 1;

                    v15 = (byte)(v12 >> v17);
                    v16 = (byte)(v12 << 8 - v17);
                }

                result = (byte)(v15 | v16);
                AesKey[index++] = result;
            } while (index < AesKey.Length);
        }

        public byte[] GetFrameWorkData()
        {
            // OK
            var result = new byte[_frameWorkData.Length - 2];

            Array.Copy(_frameWorkData, 2, result, 0, result.Length);

            return EncryptBlock(result);
        }

        private byte[] EncryptBlock(byte[] data)
        {
            // OK
            if (data == null || data.Length == 0)
                throw new ArgumentNullException(nameof(data));

            const int offset = 2;

            using (var encryptor = new Cipher(AesKey))
            {
                // Cycle 1
                var result = encryptor.Encrypt(data, 64 - offset, 16);
                Array.Copy(result, 0, data, 64 - offset, 16);

                // Cycle 2
                result = encryptor.Encrypt(data, 17 - offset, 16);
                Array.Copy(result, 0, data, 17 - offset, 16);

                // Cycle 3
                result = encryptor.Encrypt(data, 40 - offset, 16);
                Array.Copy(result, 0, data, 40 - offset, 16);

                // Cycle 4
                result = encryptor.Encrypt(data, 87 - offset, 16);
                Array.Copy(result, 0, data, 87 - offset, 16);
            }

            return data;
        }

        private byte Sub_F48550(int a2)
        {
            // OK
            byte result;

            switch (a2)
            {
                case 1:
                    result = _frameWorkData[3];
                    break;
                case 2:
                    result = _frameWorkData[62];
                    break;
                case 3:
                    result = _frameWorkData[5];
                    break;
                case 4:
                    result = _frameWorkData[39];
                    break;
                case 5:
                    result = _frameWorkData[37];
                    break;
                case 6:
                    result = _frameWorkData[33];
                    break;
                case 7:
                    result = _frameWorkData[59];
                    break;
                case 8:
                    result = _frameWorkData[10];
                    break;
                case 9:
                    result = _frameWorkData[7];
                    break;
                case 10:
                    result = _frameWorkData[104];
                    break;
                case 11:
                    result = _frameWorkData[82];
                    break;
                case 12:
                    result = _frameWorkData[38];
                    break;
                case 13:
                    result = _frameWorkData[13];
                    break;
                case 14:
                    result = _frameWorkData[84];
                    break;
                case 15:
                    result = _frameWorkData[12];
                    break;
                default:
                    result = _frameWorkData[15];
                    break;
            }

            return result;
        }

        private byte Sub_F48600(int a2)
        {
            // OK
            byte result;
            switch (a2)
            {
                case 1:
                    result = _frameWorkData[81];
                    break;
                case 2:
                    result = _frameWorkData[9];
                    break;
                case 3:
                    result = _frameWorkData[107];
                    break;
                case 4:
                    result = _frameWorkData[36];
                    break;
                case 5:
                    result = _frameWorkData[106];
                    break;
                case 6:
                    result = _frameWorkData[14];
                    break;
                case 7:
                    result = _frameWorkData[11];
                    break;
                case 8:
                    result = _frameWorkData[34];
                    break;
                case 9:
                    result = _frameWorkData[56];
                    break;
                case 10:
                    result = _frameWorkData[6];
                    break;
                case 11:
                    result = _frameWorkData[60];
                    break;
                case 12:
                    result = _frameWorkData[85];
                    break;
                case 13:
                    result = _frameWorkData[35];
                    break;
                case 14:
                    result = _frameWorkData[61];
                    break;
                case 15:
                    result = _frameWorkData[16];
                    break;
                case 16:
                    result = _frameWorkData[8];
                    break;
                case 17:
                    result = _frameWorkData[63];
                    break;
                case 18:
                    result = _frameWorkData[57];
                    break;
                case 19:
                    result = _frameWorkData[2];
                    break;
                case 20:
                    result = _frameWorkData[80];
                    break;
                case 21:
                    result = _frameWorkData[105];
                    break;
                case 22:
                    result = _frameWorkData[58];
                    break;
                case 23:
                    result = _frameWorkData[86];
                    break;
                case 24:
                    result = _frameWorkData[103];
                    break;
                case 25:
                    result = _frameWorkData[83];
                    break;
                default:
                    result = _frameWorkData[4];
                    break;
            }

            return result;
        }

        private void ShuffleXorKey(int offset)
        {
            // This seems ok.
            var v2 = BufferUtil.GetInt(ref _xorKey, offset + 32);
            var v3 = BufferUtil.GetInt(ref _xorKey, offset + 64);
            var v4 = BufferUtil.GetInt(ref _xorKey, offset + 0);
            var v5 = v2 + v3;

            WriteInt(ref _xorKey, offset + 32, v5); // OK

            var v7 = -(BufferUtil.ToULong(v5) < BufferUtil.ToULong(v2) ? 1 : 0);
            var v8 = BufferUtil.ToULong(v5 + v4);
            v8 *= v8;
            var v9 = BufferUtil.GetInt(ref _xorKey, offset + 36);
            var v10 = BitUtils.LODWORD(v8) ^ BitUtils.HIDWORD(v8);
            v8 = BitUtils.LODWORD(v8, v9 + -v7 - 749914925);
            var v12 = BufferUtil.ToULong(BitUtils.LODWORD(v8)) < BufferUtil.ToULong(v9);

            var bitResult = BitUtils.LODWORD(v8);
            WriteInt(ref _xorKey, offset + 36, bitResult); // OK

            var v13 = BufferUtil.ToULong(BitUtils.LODWORD(v8) + BufferUtil.GetInt(ref _xorKey, offset + 4));
            v13 *= v13;
            var v14 = BufferUtil.GetInt(ref _xorKey, offset + 40);
            var v15 = BitUtils.LODWORD(v13) ^ BitUtils.HIDWORD(v13);
            v13 = BitUtils.LODWORD(v13, v14 + (v12 ? 1 : 0) + 886263092);
            v12 = BufferUtil.ToULong(BitUtils.LODWORD(v13)) < BufferUtil.ToULong(v14);

            WriteInt(ref _xorKey, offset + 40, BitUtils.LODWORD(v13)); // OK

            var v17 = (ulong)(BufferUtil.GetInt(ref _xorKey, offset + 8) + (uint)BitUtils.LODWORD(v13)) & 0xFFFFFFFF;
            v17 *= v17; // OK
            var v18 = BitUtils.RotateIntLeft(v15, 16); // OK
            var v19 = BitUtils.LODWORD(v17) ^ BitUtils.HIDWORD(v17); // OK
            v17 = BitUtils.LODWORD(v17, BitUtils.RotateIntLeft(v10, 16));
            v17 = BitUtils.LODWORD(v17, v19 + v18 + BitUtils.LODWORD(v17));
            var v20 = BufferUtil.GetInt(ref _xorKey, offset + 44);

            //Console.WriteLine($"{v17:X2}");
            WriteInt(ref _xorKey, offset + 8, BitUtils.LODWORD(v17)); // OK -> F279E01F1DF14844

            v17 = BitUtils.LODWORD(v17, v20 + (v12 ? 1 : 0) + 1295307597);
            v12 = BufferUtil.ToULong(BitUtils.LODWORD(v17)) < BufferUtil.ToULong(v20);

            //Console.WriteLine($"{v17:X2}");
            WriteInt(ref _xorKey, offset + 44, BitUtils.LODWORD(v17)); // OK -> F279E01F6626E464

            var v21 = BufferUtil.ToULong(BitUtils.LODWORD(v17) + BufferUtil.GetInt(ref _xorKey, offset + 12));
            v21 *= v21;
            var v22 = BitUtils.LODWORD(v21) ^ BitUtils.HIDWORD(v21);
            v21 = BitUtils.LODWORD(v21, BitUtils.RotateIntLeft(v19, 8));
            v21 = BitUtils.LODWORD(v21, v15 + v22 + BitUtils.LODWORD(v21));
            var v24 = BufferUtil.GetInt(ref _xorKey, offset + 48);

            WriteInt(ref _xorKey, offset + 12, BitUtils.LODWORD(v21)); // OK

            v21 = BitUtils.LODWORD(v21, v24 + (v12 ? 1 : 0) - 749914925);

            WriteInt(ref _xorKey, offset + 48, BitUtils.LODWORD(v21)); // OK

            var v25 = BufferUtil.ToULong(BitUtils.LODWORD(v21)) < BufferUtil.ToULong(v24) ? 1 : 0;
            v19 = BitUtils.RotateIntLeft(v19, 16);
            var v26 = BufferUtil.ToULong(BitUtils.LODWORD(v21) + BufferUtil.GetInt(ref _xorKey, offset + 16));
            v26 *= v26;
            var v27 = BufferUtil.GetInt(ref _xorKey, offset + 52);
            v26 = BitUtils.HIDWORD(v26, BitUtils.HIDWORD(v26) ^ BitUtils.LODWORD(v26));
            v26 = BitUtils.LODWORD(v26, BitUtils.RotateIntLeft(v22, 16));
            var v28 = BitUtils.HIDWORD(v26);
            var v29 = BitUtils.LODWORD((ulong)BitUtils.HIDWORD(v26) + v26 + (ulong)v19);
            v26 = BitUtils.LODWORD(v26, v27 + v25 + 886263092);
            v12 = BufferUtil.ToULong(BitUtils.LODWORD(v26)) < BufferUtil.ToULong(v27);

            WriteInt(ref _xorKey, offset + 52, BitUtils.LODWORD(v26)); // TODO
            WriteInt(ref _xorKey, offset + 16, v29); // OK

            var call1 = BufferUtil.GetInt(ref _xorKey, offset + 20);
            var call2 = v27 + v25 + 886263092;
            var calc3 = call1 + call2;


            var test = BufferUtil.ToULong(calc3);
            test *= test;
            var v30 = BitUtils.LODWORD(test) ^ BitUtils.HIDWORD(test);
            v26 = BitUtils.LODWORD(v26, BitUtils.RotateIntLeft(BitUtils.HIDWORD(v26), 8));
            var v31 = BufferUtil.GetInt(ref _xorKey, offset + 56);

            var value = BitUtils.LODWORD((ulong)(v22 + v30) + v26);
            WriteInt(ref _xorKey, offset + 20, value); // OK

            v26 = BitUtils.LODWORD(v26, v31 + (v12 ? 1 : 0) + 1295307597);
            v12 = BufferUtil.ToULong(BitUtils.LODWORD(v26)) < BufferUtil.ToULong(v31);

            value = BitUtils.LODWORD(v26);
            WriteInt(ref _xorKey, offset + 56, value); // OK

            var subCalc = BitUtils.LODWORD(v26) + BufferUtil.GetInt(ref _xorKey, offset + 24);
            var v32 = BufferUtil.ToULong(subCalc);
            v32 *= v32;
            var v33 = BitUtils.RotateIntLeft(v28, 16);

            //Console.WriteLine($"{v32:X2}");
            var v34 = BitUtils.LODWORD(v32) ^ BitUtils.HIDWORD(v32);
            v32 = BufferUtil.ToULong(BitUtils.RotateIntLeft(v30, 16));

            value = v34 + BitUtils.LODWORD(v32) + v33;
            WriteInt(ref _xorKey, offset + 24, value); // OK

            var v35 = BufferUtil.GetInt(ref _xorKey, offset + 60);
            var v36 = BitUtils.LODWORD(BufferUtil.ToULong(v35) + (ulong)(v12 ? 1 : 0) - 749914925L);

            WriteInt(ref _xorKey, offset + 60, v36); // OK

            var v37 = BufferUtil.ToULong(v36 + BufferUtil.GetInt(ref _xorKey, offset + 28));
            v37 *= v37;
            v37 = BitUtils.HIDWORD(v37, BitUtils.HIDWORD(v37) ^ BitUtils.LODWORD(v37));

            //Console.WriteLine($"{v37:X2}");

            v37 = BitUtils.LODWORD(v37, BitUtils.RotateIntLeft(v34, 8)); // v37 = OK -> v34 = BIASED
            v34 = BitUtils.RotateIntLeft(v34, 16);

            value = v30 + BitUtils.HIDWORD(v37) + BitUtils.LODWORD(v37);
            WriteInt(ref _xorKey, offset + 28, value); // OK

            v37 = BitUtils.LODWORD(v37, BitUtils.RotateIntLeft(BitUtils.HIDWORD(v37), 16));
            var v38 = BitUtils.RotateIntLeft(v10, 8);

            value = v10 + BitUtils.LODWORD(v37) + v34;
            WriteInt(ref _xorKey, offset, value); // BIASED

            value = v15 + BitUtils.HIDWORD(v37) + v38;

            WriteInt(ref _xorKey, offset + 4, value); // OK

            var result = (BufferUtil.ToULong(v36) < BufferUtil.ToULong(v35) ? 1 : 0) + 1295307597;

            WriteInt(ref _xorKey, offset + 64, result); // OK
        }

        private static void WriteInt(ref byte[] buffer, int offset, int value)
        {
            var bytes = BitConverter.GetBytes(value);

            Array.Copy(bytes, 0, buffer, offset, 4);
        }

        public byte[] Xor(byte[] buffer)
        {
            return Xor(buffer, 0, buffer.Length);
        }

        public byte[] Xor(byte[] buffer, int offset, int size)
        {
            var v6 = 0;
            var v5 = size;

            if (size >= 16) // Xors blocks of 16 bytes?
            {
                var var5 = size >> 4;

                bool v10;
                do
                {
                    ShuffleXorKey(68);

                    v5 -= 16;

                    var baseOffset = v6 + offset;

                    WriteInt(ref buffer, baseOffset,
                        BufferUtil.GetInt(ref _xorKey, 68) ^ BufferUtil.GetInt(ref buffer, baseOffset) ^ BufferUtil.GetInt(ref _xorKey, 80) << 16 ^
                        BitConverter.ToUInt16(_xorKey, 90));

                    WriteInt(ref buffer, v6 + 4 + offset,
                        BufferUtil.GetInt(ref _xorKey, 76) ^ BitConverter.ToInt32(buffer, v6 + 4 + offset) ^
                        BufferUtil.GetInt(ref _xorKey, 88) << 16 ^
                        BitConverter.ToUInt16(_xorKey, 98));

                    WriteInt(ref buffer, v6 + 8 + offset,
                        BufferUtil.GetInt(ref _xorKey, 84) ^ BitConverter.ToInt32(buffer, v6 + 8 + offset) ^
                        BufferUtil.GetInt(ref _xorKey, 96) << 16 ^
                        BitConverter.ToUInt16(_xorKey, 74));

                    WriteInt(ref buffer, v6 + 12 + offset,
                        BufferUtil.GetInt(ref _xorKey, 92) ^ BitConverter.ToInt32(buffer, v6 + 12 + offset) ^
                        BufferUtil.GetInt(ref _xorKey, 72) << 16 ^
                        BitConverter.ToUInt16(_xorKey, 82));

                    v6 += 16;
                    v10 = var5-- == 1;
                } while (!v10);
            }

            if (v5 != 0)
            {
                ShuffleXorKey(68);

                WriteInt(ref _xorKey, 136,
                    BufferUtil.GetInt(ref _xorKey, 68) ^ BufferUtil.GetInt(ref _xorKey, 80) << 16 ^
                    BitUtils.LODWORD((uint)BufferUtil.GetInt(ref _xorKey, 88) >> 16));
                WriteInt(ref _xorKey, 140,
                    BufferUtil.GetInt(ref _xorKey, 76) ^ BufferUtil.GetInt(ref _xorKey, 88) << 16 ^
                    BitUtils.LODWORD((uint)BufferUtil.GetInt(ref _xorKey, 96) >> 16));
                WriteInt(ref _xorKey, 144,
                    BufferUtil.GetInt(ref _xorKey, 84) ^ BufferUtil.GetInt(ref _xorKey, 96) << 16 ^
                    BitUtils.LODWORD((uint)BufferUtil.GetInt(ref _xorKey, 72) >> 16));
                WriteInt(ref _xorKey, 148,
                    BufferUtil.GetInt(ref _xorKey, 92) ^ BufferUtil.GetInt(ref _xorKey, 72) << 16 ^
                    BitUtils.LODWORD((uint)BufferUtil.GetInt(ref _xorKey, 80) >> 16));

                var result = 0;

                while (result < v5)
                {
                    var pos = v6 + offset + result;

                    buffer[pos] ^= _xorKey[136 + result];
                    ++result;
                }
            }

            // Note this seems like it is called AFTER the first packet has been sent.
            // This means that the first packet is not xored with this?
            if (_sessionkey == null)
            {
                Logging.Info($"Call {nameof(_sessionkey)}");
                _sessionkey = new byte[16];

                Array.Copy(_frameWorkData, 64, _sessionkey, 0, 16);

                ShuffleXorKeyBaseOnSessionKeyA();
                ShuffleXorKeyBaseOnSessionKeyB();
            }

            return buffer;
        }

        private void ShuffleXorKeyBaseOnSessionKeyA()
        {
            Logging.Info($"Call {nameof(ShuffleXorKeyBaseOnSessionKeyA)}");
            // this.xorKey, bufferSessionKey

            // bufferSessionKey = copy of session key;
            // bufferKey is the actually _xorKey that is writable.
            int v3, v4, v5, v2, v6;

            var bufferCopy = _sessionkey;
            using (var memStream = new MemoryStream(bufferCopy))
            using (var reader = new BinaryReader(memStream))
            {
                v3 = reader.ReadInt32(); // 0
                v4 = reader.ReadInt32(); // 4
                v5 = reader.ReadInt32(); // 8
                v2 = reader.ReadInt32(); // 12

                reader.BaseStream.Seek(8, SeekOrigin.Begin);
                v6 = BitUtils.LODWORD(reader.ReadUInt64() >> 16);
            }

            WriteInt(ref _xorKey, 0, v3);
            WriteInt(ref _xorKey, 4, v6);
            WriteInt(ref _xorKey, 8, v4);
            WriteInt(ref _xorKey, 12, BitUtils.LODWORD(BufferUtil.ToULong(v3) << 16) | BitUtils.LODWORD(BufferUtil.ToULong(v2) >> 16));
            WriteInt(ref _xorKey, 16, v5);
            WriteInt(ref _xorKey, 20, BitUtils.LODWORD(BufferUtil.ToULong(v3) >> 16) | BitUtils.LODWORD(BufferUtil.ToULong(v4) << 16));
            WriteInt(ref _xorKey, 24, v2);
            WriteInt(ref _xorKey, 28, BitUtils.LODWORD(BufferUtil.ToULong(v4) >> 16) | BitUtils.LODWORD(BufferUtil.ToULong(v5) << 16));
            int v7 = BitUtils.RotateIntLeft(v5, 16);
            WriteInt(ref _xorKey, 32, v7);
            int v8 = BitUtils.RotateIntLeft(v2, 16);
            WriteInt(ref _xorKey, 40, v8);
            int v9 = BitUtils.RotateIntLeft(v3, 16);
            WriteInt(ref _xorKey, 48, v9);
            int v10 = BitUtils.RotateIntLeft(v4, 16);
            int v57 = v4;

            WriteInt(ref _xorKey, 56, v10);
            WriteInt(ref _xorKey, 64, 1295307597);
            WriteInt(ref _xorKey, 36, v3 ^ ((v3 ^ v4) & 0xFFFF));
            WriteInt(ref _xorKey, 44, v4 ^ ((v4 ^ v5) & 0xFFFF));
            WriteInt(ref _xorKey, 52, v5 ^ ((v5 ^ v2) & 0xFFFF));
            WriteInt(ref _xorKey, 60, v2 ^ ((v3 ^ v2) & 0xFFFF));

            int v11 = BufferUtil.GetInt(ref _xorKey, 32);
            int v12 = BufferUtil.GetInt(ref _xorKey, 36);
            int v13 = BufferUtil.GetInt(ref _xorKey, 4);
            int v66 = BufferUtil.GetInt(ref _xorKey, 64);
            int v56 = BufferUtil.GetInt(ref _xorKey, 40);
            int v58 = BufferUtil.GetInt(ref _xorKey, 44);
            int v59 = BufferUtil.GetInt(ref _xorKey, 12);
            int v60 = BufferUtil.GetInt(ref _xorKey, 48);
            int v62 = BufferUtil.GetInt(ref _xorKey, 20);
            int v64 = BufferUtil.GetInt(ref _xorKey, 24);
            int v61 = BufferUtil.GetInt(ref _xorKey, 52);
            int v14 = BufferUtil.GetInt(ref _xorKey, 56);
            int v55 = BufferUtil.GetInt(ref _xorKey, 60);
            int v15 = BufferUtil.GetInt(ref _xorKey, 28);
            int v16 = BufferUtil.GetInt(ref _xorKey, 16);
            int v63 = v14;
            int v65 = v15;
            int magicCounter = 4;

            while (true)
            {
                int v18 = v66 + v11;
                int v19 = -(BufferUtil.ToULong(v18) < BufferUtil.ToULong(v11) ? 1 : 0);
                ulong v20 = BufferUtil.ToULong(v3 + v18);
                v20 *= v20;
                int v21 = BitUtils.LODWORD(v20) ^ BitUtils.HIDWORD(v20);
                v20 = BitUtils.LODWORD(v20, v12);
                int v22 = -v19 - 749914925 + v12;
                int v24 = BufferUtil.ToULong(v22) < BufferUtil.ToULong(BitUtils.LODWORD(v20)) ? 1 : 0;
                ulong test = BufferUtil.ToULong(v13 + v22);
                test *= test;
                int v25 = BitUtils.LODWORD(test) ^ BitUtils.HIDWORD(test);
                v20 = BitUtils.HIDWORD(v20, v24 + 886263092 + v56);
                bool v27 = BufferUtil.ToULong(BitUtils.HIDWORD(v20)) < BufferUtil.ToULong(v56);
                v56 += v24 + 886263092;
                ulong v28 = BufferUtil.ToULong(v57 + BitUtils.HIDWORD(v20));
                v28 *= v28;
                int v29 = BitUtils.LODWORD(v28) ^ BitUtils.HIDWORD(v28);
                v28 = BitUtils.HIDWORD(v28, BitUtils.RotateIntLeft(v21, 16));
                v28 = BitUtils.LODWORD(v28, BitUtils.RotateIntLeft(v25, 16));
                v57 = BitUtils.LODWORD((ulong)v29 + v28 + (ulong)BitUtils.HIDWORD(v28));
                v28 = BitUtils.HIDWORD(v28, (v27 ? 1 : 0) + 1295307597 + v58);
                v27 = BufferUtil.ToULong(BitUtils.HIDWORD(v28)) < BufferUtil.ToULong(v58);
                v58 = BitUtils.HIDWORD(v28);
                ulong v30 = BufferUtil.ToULong(v59 + v58);
                v30 *= v30;
                int v31 = BitUtils.LODWORD(v30) ^ BitUtils.HIDWORD(v30);
                v30 = BitUtils.LODWORD(v30, BitUtils.RotateIntLeft(v29, 8));
                v59 = BitUtils.LODWORD((ulong)(v25 + v31) + v30);
                v30 = BitUtils.HIDWORD(v30, (v27 ? 1 : 0) - 749914925 + v60);
                v27 = BufferUtil.ToULong(BitUtils.HIDWORD(v30)) < BufferUtil.ToULong(v60);
                v60 = BitUtils.HIDWORD(v30);
                v29 = BitUtils.RotateIntLeft(v29, 16);
                ulong v32 = BufferUtil.ToULong(v16 + BitUtils.HIDWORD(v30));
                v32 *= v32;
                int v33 = BitUtils.RotateIntLeft(v31, 16);
                int v34 = BitUtils.LODWORD(v32) ^ BitUtils.HIDWORD(v32);
                v16 = v29 + (BitUtils.LODWORD(v32) ^ BitUtils.HIDWORD(v32)) + v33;
                v32 = BitUtils.HIDWORD(v32, (v27 ? 1 : 0) + 886263092 + v61);
                v27 = BufferUtil.ToULong(BitUtils.HIDWORD(v32)) < BufferUtil.ToULong(v61);
                v61 = BitUtils.HIDWORD(v32);
                ulong v35 = BufferUtil.ToULong(v62 + BitUtils.HIDWORD(v32));
                v35 *= v35;
                int v36 = BitUtils.LODWORD(v35) ^ BitUtils.HIDWORD(v35);
                v35 = BitUtils.LODWORD(v35, BitUtils.RotateIntLeft(v34, 8));
                v62 = BitUtils.LODWORD((ulong)(v31 + v36) + v35);
                v35 = BitUtils.HIDWORD(v35, (v27 ? 1 : 0) + 1295307597 + v63);
                v27 = BufferUtil.ToULong(BitUtils.HIDWORD(v35)) < BufferUtil.ToULong(v63);
                v63 = BitUtils.HIDWORD(v35);
                ulong v37 = BufferUtil.ToULong(v64 + BitUtils.HIDWORD(v35));
                v37 *= v37;
                int v38 = BitUtils.LODWORD(v37) ^ BitUtils.HIDWORD(v37);
                v37 = BitUtils.HIDWORD(v37, BitUtils.RotateIntLeft(v34, 16));
                v37 = BitUtils.LODWORD(v37, BitUtils.RotateIntLeft(v36, 16));
                int v39 = v55;
                v64 = BitUtils.LODWORD((ulong)v38 + v37 + (ulong)BitUtils.HIDWORD(v37));
                v55 += (v27 ? 1 : 0) - 749914925;
                ulong v40 = BufferUtil.ToULong(v55 + v65);
                v40 *= v40;
                v40 = BitUtils.HIDWORD(v40, BitUtils.HIDWORD(v40) ^ BitUtils.LODWORD(v40));
                v40 = BitUtils.LODWORD(v40, BitUtils.RotateIntLeft(v38, 8));
                v38 = BitUtils.RotateIntLeft(v38, 16);
                v40 = BitUtils.LODWORD(v40, BitUtils.LODWORD((ulong)(v36 + BitUtils.HIDWORD(v40)) + v40));
                v12 = v22;
                v65 = BitUtils.LODWORD(v40);
                v40 = BitUtils.LODWORD(v40, BitUtils.RotateIntLeft(BitUtils.HIDWORD(v40), 16));
                int v41 = BitUtils.RotateIntLeft(v21, 8);
                v3 = BitUtils.LODWORD((ulong)v21 + v40 + (ulong)v38);
                int v42 = v25 + BitUtils.HIDWORD(v40) + v41;
                v11 = v18;
                int v43 = (BufferUtil.ToULong(v55) < BufferUtil.ToULong(v39) ? 1 : 0) + 1295307597;
                v66 = v43;
                if (magicCounter-- == 1)
                {
                    WriteInt(ref _xorKey, 32, v18);
                    WriteInt(ref _xorKey, 36, v22);
                    WriteInt(ref _xorKey, 4, v42);
                    WriteInt(ref _xorKey, 40, v56);
                    WriteInt(ref _xorKey, 0, v3);
                    WriteInt(ref _xorKey, 60, v55);
                    WriteInt(ref _xorKey, 8, v57);
                    WriteInt(ref _xorKey, 64, v43);
                    WriteInt(ref _xorKey, 44, v58);
                    WriteInt(ref _xorKey, 12, v59);
                    WriteInt(ref _xorKey, 48, v60);
                    WriteInt(ref _xorKey, 16, v16);
                    WriteInt(ref _xorKey, 52, v61);
                    WriteInt(ref _xorKey, 56, v63);
                    WriteInt(ref _xorKey, 20, v62);
                    WriteInt(ref _xorKey, 24, v64);
                    WriteInt(ref _xorKey, 28, v65);
                    WriteInt(ref _xorKey, 32, BufferUtil.GetInt(ref _xorKey, 32) ^ v16);
                    WriteInt(ref _xorKey, 84, v16);
                    WriteInt(ref _xorKey, 100, BufferUtil.GetInt(ref _xorKey, 32));
                    int v47 = BufferUtil.GetInt(ref _xorKey, 20);
                    WriteInt(ref _xorKey, 36, BufferUtil.GetInt(ref _xorKey, 36) ^ v47);
                    WriteInt(ref _xorKey, 88, v47);
                    WriteInt(ref _xorKey, 104, BufferUtil.GetInt(ref _xorKey, 36));
                    int v48 = BufferUtil.GetInt(ref _xorKey, 24);
                    WriteInt(ref _xorKey, 40, BufferUtil.GetInt(ref _xorKey, 40) ^ v48);
                    WriteInt(ref _xorKey, 92, v48);
                    WriteInt(ref _xorKey, 108, BufferUtil.GetInt(ref _xorKey, 40));
                    int v49 = BufferUtil.GetInt(ref _xorKey, 28);
                    WriteInt(ref _xorKey, 44, BufferUtil.GetInt(ref _xorKey, 44) ^ v49);
                    WriteInt(ref _xorKey, 96, v49);
                    WriteInt(ref _xorKey, 112, BufferUtil.GetInt(ref _xorKey, 44));
                    int v50 = BufferUtil.GetInt(ref _xorKey, 0);
                    WriteInt(ref _xorKey, 48, BufferUtil.GetInt(ref _xorKey, 48) ^ v50);
                    WriteInt(ref _xorKey, 68, v50);
                    WriteInt(ref _xorKey, 116, BufferUtil.GetInt(ref _xorKey, 48));
                    int v51 = BufferUtil.GetInt(ref _xorKey, 4);
                    WriteInt(ref _xorKey, 52, BufferUtil.GetInt(ref _xorKey, 52) ^ v51);
                    WriteInt(ref _xorKey, 72, v51);
                    WriteInt(ref _xorKey, 120, BufferUtil.GetInt(ref _xorKey, 52));
                    int v52 = BufferUtil.GetInt(ref _xorKey, 8);
                    WriteInt(ref _xorKey, 56, BufferUtil.GetInt(ref _xorKey, 56) ^ v52);
                    WriteInt(ref _xorKey, 76, v52);
                    WriteInt(ref _xorKey, 124, BufferUtil.GetInt(ref _xorKey, 56));
                    int v53 = BufferUtil.GetInt(ref _xorKey, 12);
                    WriteInt(ref _xorKey, 60, BufferUtil.GetInt(ref _xorKey, 60) ^ v53);
                    WriteInt(ref _xorKey, 80, v53);
                    WriteInt(ref _xorKey, 128, BufferUtil.GetInt(ref _xorKey, 60));
                    int result = BufferUtil.GetInt(ref _xorKey, 64);
                    WriteInt(ref _xorKey, 132, result);
                    return;
                }

                v13 = v42;
            }
        }

        private void ShuffleXorKeyBaseOnSessionKeyB()
        {
            Logging.Info($"Call {nameof(ShuffleXorKeyBaseOnSessionKeyB)}");
            // bufferSessionKey = copy memoryStream of _sessionkey;
            // bufferKey is the actually _xorKey that is writable.

            int v2, v3;

            var bufferCopy = _sessionkey;
            using (var memStream = new MemoryStream(bufferCopy))
            using (var reader = new BinaryReader(memStream))
            {
                // TODO
                reader.ReadUInt16(); // Skip 2 bytes.
                v3 = reader.ReadInt32();
                reader.BaseStream.Seek(6, SeekOrigin.Begin);
                v2 = reader.ReadInt32();
            }

            int v4 = v2 & -65536 | BitUtils.LODWORD(BufferUtil.ToULong(v3) >> 16);
            int v5 = (v3 & 0xFFFF) | v2 << 16;
            WriteInt(ref _xorKey, 100, v3 ^ BufferUtil.GetInt(ref _xorKey, 32));
            WriteInt(ref _xorKey, 104, v4 ^ BufferUtil.GetInt(ref _xorKey, 36));
            WriteInt(ref _xorKey, 108, v2 ^ BufferUtil.GetInt(ref _xorKey, 40));
            WriteInt(ref _xorKey, 112, v5 ^ BufferUtil.GetInt(ref _xorKey, 44));
            WriteInt(ref _xorKey, 116, v3 ^ BufferUtil.GetInt(ref _xorKey, 48));
            WriteInt(ref _xorKey, 120, v4 ^ BufferUtil.GetInt(ref _xorKey, 52));
            WriteInt(ref _xorKey, 124, v2 ^ BufferUtil.GetInt(ref _xorKey, 56));
            WriteInt(ref _xorKey, 128, v5 ^ BufferUtil.GetInt(ref _xorKey, 60));
            WriteInt(ref _xorKey, 68, BufferUtil.GetInt(ref _xorKey, 0));
            WriteInt(ref _xorKey, 72, BufferUtil.GetInt(ref _xorKey, 4));
            WriteInt(ref _xorKey, 76, BufferUtil.GetInt(ref _xorKey, 8));
            WriteInt(ref _xorKey, 80, BufferUtil.GetInt(ref _xorKey, 12));
            WriteInt(ref _xorKey, 84, BufferUtil.GetInt(ref _xorKey, 16));
            WriteInt(ref _xorKey, 88, BufferUtil.GetInt(ref _xorKey, 20));
            WriteInt(ref _xorKey, 92, BufferUtil.GetInt(ref _xorKey, 24));
            WriteInt(ref _xorKey, 96, BufferUtil.GetInt(ref _xorKey, 28));
            int v6 = BufferUtil.GetInt(ref _xorKey, 64);
            WriteInt(ref _xorKey, 132, v6);
            int v7 = BufferUtil.GetInt(ref _xorKey, 100);
            int v8 = BufferUtil.GetInt(ref _xorKey, 104);
            int v48 = BufferUtil.GetInt(ref _xorKey, 72);
            int v37 = BufferUtil.GetInt(ref _xorKey, 108);
            int v38 = BufferUtil.GetInt(ref _xorKey, 76);
            int v39 = BufferUtil.GetInt(ref _xorKey, 112);
            int v40 = BufferUtil.GetInt(ref _xorKey, 80);
            int v41 = BufferUtil.GetInt(ref _xorKey, 116);
            int v42 = BufferUtil.GetInt(ref _xorKey, 84);
            int v43 = BufferUtil.GetInt(ref _xorKey, 120);
            int v44 = BufferUtil.GetInt(ref _xorKey, 88);
            int v45 = BufferUtil.GetInt(ref _xorKey, 124);
            int v46 = BufferUtil.GetInt(ref _xorKey, 92);
            int v36 = BufferUtil.GetInt(ref _xorKey, 128);
            int v47 = BufferUtil.GetInt(ref _xorKey, 96);
            int v49 = 4;

            int v10;
            int v14;
            int v34;
            do
            {
                v10 = v6 + v7;
                int v11 = -(BufferUtil.ToULong(v10) < BufferUtil.ToULong(v7) ? 1 : 0);
                ulong v12 = BufferUtil.ToULong(v10 + BufferUtil.GetInt(ref _xorKey, 68));
                v12 *= v12;
                int v13 = BitUtils.LODWORD(v12) ^ BitUtils.HIDWORD(v12);
                v12 = BitUtils.LODWORD(v12, v8);
                v14 = -v11 - 749914925 + v8;
                ulong test = BufferUtil.ToULong(v14 + v48);
                test *= test;
                int v16 = BitUtils.LODWORD(test) ^ BitUtils.HIDWORD(test);
                v12 = BitUtils.HIDWORD(v12,
                    (BufferUtil.ToULong(v14) < BufferUtil.ToULong(BitUtils.LODWORD(v12)) ? 1 : 0) + 886263092 +
                    v37);
                bool v18 = BufferUtil.ToULong(BitUtils.HIDWORD(v12)) < BufferUtil.ToULong(v37);
                v37 += (BufferUtil.ToULong(v14) < BufferUtil.ToULong(BitUtils.LODWORD(v12)) ? 1 : 0) +
                       886263092;
                ulong v19 = BufferUtil.ToULong(v38 + BitUtils.HIDWORD(v12));
                v19 *= v19;
                int v20 = BitUtils.LODWORD(v19) ^ BitUtils.HIDWORD(v19);
                v19 = BitUtils.HIDWORD(v19, BitUtils.RotateIntLeft(v13, 16));
                v19 = BitUtils.LODWORD(v19, BitUtils.RotateIntLeft(v16, 16));
                v38 = BitUtils.LODWORD((ulong)v20 + v19 + (ulong)BitUtils.HIDWORD(v19));
                v19 = BitUtils.HIDWORD(v19, (v18 ? 1 : 0) + 1295307597 + v39);
                v18 = BufferUtil.ToULong(BitUtils.HIDWORD(v19)) < BufferUtil.ToULong(v39);
                v39 = BitUtils.HIDWORD(v19);
                ulong v21 = BufferUtil.ToULong(v40 + BitUtils.HIDWORD(v19));
                v21 *= v21;
                int v22 = BitUtils.LODWORD(v21) ^ BitUtils.HIDWORD(v21);
                v21 = BitUtils.LODWORD(v21, BitUtils.RotateIntLeft(v20, 8));
                v40 = BitUtils.LODWORD((ulong)(v16 + v22) + v21);
                v21 = BitUtils.HIDWORD(v21, (v18 ? 1 : 0) - 749914925 + v41);
                v18 = BufferUtil.ToULong(BitUtils.HIDWORD(v21)) < BufferUtil.ToULong(v41);
                v41 = BitUtils.HIDWORD(v21);
                ulong v24 = BufferUtil.ToULong(v42 + BitUtils.HIDWORD(v21));
                v24 *= v24;
                int v25 = BitUtils.LODWORD(v24) ^ BitUtils.HIDWORD(v24);
                v24 = BitUtils.LODWORD(v24, BitUtils.RotateIntLeft(v22, 16));
                v24 = BitUtils.HIDWORD(v24, BitUtils.RotateIntLeft(v20, 16));
                v42 = BitUtils.LODWORD((ulong)v25 + v24 + (ulong)BitUtils.HIDWORD(v24));
                v24 = BitUtils.HIDWORD(v24, (v18 ? 1 : 0) + 886263092 + v43);
                v18 = BufferUtil.ToULong(BitUtils.HIDWORD(v24)) < BufferUtil.ToULong(v43);
                v43 = BitUtils.HIDWORD(v24);
                ulong v27 = BufferUtil.ToULong(v44 + BitUtils.HIDWORD(v24));
                v27 *= v27;
                int v28 = BitUtils.LODWORD(v27) ^ BitUtils.HIDWORD(v27);
                v27 = BitUtils.LODWORD(v27, BitUtils.RotateIntLeft(v25, 8));
                v44 = BitUtils.LODWORD((ulong)(v22 + v28) + v27);
                v27 = BitUtils.HIDWORD(v27, (v18 ? 1 : 0) + 1295307597 + v45);
                v18 = BufferUtil.ToULong(BitUtils.HIDWORD(v27)) < BufferUtil.ToULong(v45);
                v45 = BitUtils.HIDWORD(v27);
                ulong v29 = BufferUtil.ToULong(v46 + BitUtils.HIDWORD(v27));
                v29 *= v29;
                int v30 = BitUtils.LODWORD(v29) ^ BitUtils.HIDWORD(v29);
                v29 = BitUtils.HIDWORD(v29, BitUtils.RotateIntLeft(v25, 16));
                v29 = BitUtils.LODWORD(v29, BitUtils.RotateIntLeft(v28, 16));
                int v31 = v36;
                v46 = BitUtils.LODWORD((ulong)v30 + v29 + (ulong)BitUtils.HIDWORD(v29));
                v36 += (v18 ? 1 : 0) - 749914925;
                ulong v32 = BufferUtil.ToULong(v36 + v47);
                v32 *= v32;
                v32 = BitUtils.HIDWORD(v32, BitUtils.LODWORD(v32) ^ BitUtils.HIDWORD(v32));
                v32 = BitUtils.LODWORD(v32, BitUtils.RotateIntLeft(v30, 8));
                v30 = BitUtils.RotateIntLeft(v30, 16);
                v47 = BitUtils.LODWORD((ulong)(v28 + BitUtils.HIDWORD(v32)) + v32);
                int v33 = BitUtils.RotateIntLeft(v13, 8);
                v32 = BitUtils.LODWORD(v32, BitUtils.RotateIntLeft(BitUtils.HIDWORD(v32), 16));
                v34 = v16 + BitUtils.HIDWORD(v32) + v33;
                v18 = BufferUtil.ToULong(v36) < BufferUtil.ToULong(v31);
                v8 = v14;
                WriteInt(ref _xorKey, 68, BitUtils.LODWORD((ulong)v13 + v32 + (ulong)v30));
                v7 = v10;
                v6 = (v18 ? 1 : 0) + 1295307597;
                v48 = v34;
                --v49;
            } while (v49 != 0);

            WriteInt(ref _xorKey, 128, v36);
            WriteInt(ref _xorKey, 100, v10);
            WriteInt(ref _xorKey, 104, v14);
            WriteInt(ref _xorKey, 108, v37);
            WriteInt(ref _xorKey, 76, v38);
            WriteInt(ref _xorKey, 112, v39);
            WriteInt(ref _xorKey, 80, v40);
            WriteInt(ref _xorKey, 116, v41);
            WriteInt(ref _xorKey, 84, v42);
            WriteInt(ref _xorKey, 120, v43);
            WriteInt(ref _xorKey, 88, v44);
            WriteInt(ref _xorKey, 124, v45);
            WriteInt(ref _xorKey, 132, v6);
            WriteInt(ref _xorKey, 72, v34);
            WriteInt(ref _xorKey, 92, v46);
            WriteInt(ref _xorKey, 96, v47);
        }
    }
}
