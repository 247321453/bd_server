using Core.Enums;
using System.Collections.Concurrent;
using System.Text;

namespace Core.Extension
{
    public static class HashUtil
    {
        private static readonly ConcurrentDictionary<ErrorStrings, uint> ErrorStringsCache;

        static HashUtil()
        {
            ErrorStringsCache = new ConcurrentDictionary<ErrorStrings, uint>();
            ErrorStringsCache.TryAdd(ErrorStrings.NONE, 0);
        }

        public static uint GetErrorHash(ErrorStrings errorString)
        {
            if (!ErrorStringsCache.TryGetValue(errorString, out var result))
            {
                result = GenerateHashC(errorString.ToString());
                ErrorStringsCache.TryAdd(errorString, result);
            }

            return result;
        }

        public static uint GenerateHashC(string stringValue)
        {
            var stringBuffer = Encoding.Unicode.GetBytes(stringValue);

            return GenerateHash(stringBuffer, 3);
        }

        private static uint GenerateHash(byte[] inputData, int type)
        {
            int length = inputData.Length;

            int v3 = length - 558228019;
            int v4 = length - 558228019;
            int v5 = length - 558228019;
            int offSet = 0;
            int remainingSize;
            int blockCount;

            if (type == 1)
            {
                if (length > 12)
                {
                    blockCount = (length - 13) / 12 + 1;

                    do
                    {
                        int v6 = BufferUtil.GetInt(ref inputData, 8 + offSet) + v3;
                        int v7 = BufferUtil.GetInt(ref inputData, 4 + offSet) + v5;
                        int v8 = BitUtils.RotateIntLeft(v6, 4);
                        int v9 = v4 + BufferUtil.GetInt(ref inputData, offSet) - v6 ^ v8;
                        int v10 = v7 + v6;
                        int v11 = v7 - v9;
                        int v12 = BitUtils.RotateIntLeft(v9, 6);
                        int v13 = v10 + v9;
                        int v14 = v11 ^ v12;
                        int v15 = v10 - v14;
                        int v16 = BitUtils.RotateIntLeft(v14, 8);
                        int v17 = v13 + v14;
                        int v18 = v15 ^ v16;
                        int v19 = v13 - v18;
                        int v20 = BitUtils.RotateIntLeft(v18, 16);
                        int v21 = v17 + v18;
                        int v22 = v19 ^ v20;
                        int v23 = v17 - v22;
                        int v24 = BitUtils.RotateIntRight(v22, 13);
                        v4 = v21 + v22;
                        int v25 = v23 ^ v24;
                        int v26 = BitUtils.RotateIntLeft(v25, 4);
                        v3 = v21 - v25 ^ v26;
                        v5 = v4 + v25;
                        length -= 12;
                        offSet += 12;
                        --blockCount;
                    } while (blockCount > 0);
                }

                remainingSize = inputData.Length - offSet;
                switch (remainingSize)
                {
                    case 0:
                        return (uint)v3 & 0xFFFFFFFF;
                    case 1:
                        v4 += inputData[offSet];
                        break;
                    case 2:
                        v4 += BufferUtil.GetShort(ref inputData, offSet); // BufferUtil.GetShort(ref inputData, offSet);
                        break;
                    case 3:
                        v4 += BufferUtil.ReadD3(ref inputData, offSet);
                        break;
                    case 5:
                        v5 += inputData[offSet + 4];
                        v4 += BufferUtil.GetInt(ref inputData, offSet);
                        break;
                    case 6:
                        v5 += BufferUtil.GetShort(ref inputData, offSet + 4);
                        v4 += BufferUtil.GetInt(ref inputData, offSet);
                        break;
                    case 7:
                        v5 += BufferUtil.ReadD3(ref inputData, offSet + 4);
                        v4 += BufferUtil.GetInt(ref inputData, offSet);
                        break;
                    case 9:
                        v5 += BufferUtil.GetInt(ref inputData, offSet + 4);
                        v3 += inputData[offSet + 8];
                        v4 += BufferUtil.GetInt(ref inputData, offSet);
                        break;
                    case 10:
                        v5 += BufferUtil.GetInt(ref inputData, offSet + 4);
                        v3 += BufferUtil.GetShort(ref inputData, offSet + 8);
                        v4 += BufferUtil.GetInt(ref inputData, offSet);
                        break;
                    case 11:
                        v5 += BufferUtil.GetInt(ref inputData, offSet + 4);
                        v3 += BufferUtil.ReadD3(ref inputData, offSet + 8);
                        v4 += BufferUtil.GetInt(ref inputData, offSet);
                        break;
                    case 12:
                        v3 += BufferUtil.GetInt(ref inputData, offSet + 8);
                        goto case 8;
                    case 8:
                        v5 += BufferUtil.GetInt(ref inputData, offSet + 4);
                        goto case 4;
                    case 4:
                        v4 += BufferUtil.GetInt(ref inputData, offSet);
                        break;
                }
            }
            else if (type == 2)
            {
                if (length > 12)
                {
                    blockCount = (length - 13) / 12 + 1;

                    do
                    {
                        int v28 = v5 + BufferUtil.GetShort(ref inputData, 4 + offSet);
                        int v29 = (BufferUtil.GetShort(ref inputData, 10 + offSet) << 16) + v3 +
                                  BufferUtil.GetShort(ref inputData, 8 + offSet);
                        int v30 = v28 + (BufferUtil.GetShort(ref inputData, 6 + offSet) << 16);
                        int v31 = BufferUtil.GetShort(ref inputData, offSet);
                        int v32 = BufferUtil.GetShort(ref inputData, 2 + offSet) << 16;
                        int v33 = BitUtils.RotateIntLeft(v29, 4);
                        int v34 = v4 + v32 - v29;
                        int v35 = v30 + v29;
                        int v36 = v34 + v31 ^ v33;
                        int v37 = v30 - v36;
                        int v38 = BitUtils.RotateIntLeft(v36, 6);
                        int v39 = v35 + v36;
                        int v40 = v37 ^ v38;
                        int v41 = v35 - v40;
                        int v42 = BitUtils.RotateIntLeft(v40, 8);
                        int v43 = v39 + v40;
                        int v44 = v41 ^ v42;
                        int v45 = v39 - v44;
                        int v46 = BitUtils.RotateIntLeft(v44, 16);
                        int v47 = v43 + v44;
                        int v48 = v45 ^ v46;
                        int v49 = v43 - v48;
                        int v50 = BitUtils.RotateIntRight(v48, 13);
                        v4 = v47 + v48;
                        int v51 = v49 ^ v50;
                        int v52 = BitUtils.RotateIntLeft(v51, 4);
                        v3 = v47 - v51 ^ v52;
                        v5 = v4 + v51;
                        length -= 12;
                        offSet += 12;
                        --blockCount;
                    } while (blockCount > 0);
                }

                remainingSize = inputData.Length - offSet;
                switch (remainingSize)
                {
                    case 0:
                        return (uint)v3 & 0xFFFFFFFF;
                    case 1:
                        v4 += inputData[offSet];
                        break;
                    case 3:
                        v4 += inputData[2 + offSet] << 16;
                        goto case 2;
                    case 2:
                        v4 += BufferUtil.GetShort(ref inputData, offSet);
                        break;
                    case 5:
                        v5 += inputData[4 + offSet];
                        v4 += (BufferUtil.GetShort(ref inputData, 2 + offSet) << 16) +
                              BufferUtil.GetShort(ref inputData, offSet);
                        break;
                    case 7:
                        v5 += inputData[6 + offSet] << 16;
                        goto case 6;
                    case 6:
                        v5 += BufferUtil.GetShort(ref inputData, 4 + offSet);
                        v4 += (BufferUtil.GetShort(ref inputData, 2 + offSet) << 16) +
                              BufferUtil.GetShort(ref inputData, offSet);
                        break;
                    case 9:
                        v3 += inputData[8 + offSet];
                        v5 += (BufferUtil.GetShort(ref inputData, 6 + offSet) << 16) +
                              BufferUtil.GetShort(ref inputData, 4 + offSet);
                        v4 += (BufferUtil.GetShort(ref inputData, 2 + offSet) << 16) +
                              BufferUtil.GetShort(ref inputData, offSet);
                        break;
                    case 11:
                        v3 += inputData[10 + offSet] << 16;
                        goto case 10;
                    case 10:
                        v3 += BufferUtil.GetShort(ref inputData, 8 + offSet);
                        v5 += (BufferUtil.GetShort(ref inputData, 6 + offSet) << 16) +
                              BufferUtil.GetShort(ref inputData, 4 + offSet);
                        v4 += (BufferUtil.GetShort(ref inputData, 2 + offSet) << 16) +
                              BufferUtil.GetShort(ref inputData, offSet);
                        break;
                    case 12:
                        v3 += (BufferUtil.GetShort(ref inputData, 10 + offSet) << 16) +
                              BufferUtil.GetShort(ref inputData, 8 + offSet);
                        goto case 8;
                    case 8:
                        v5 += (BufferUtil.GetShort(ref inputData, 6 + offSet) << 16) +
                              BufferUtil.GetShort(ref inputData, 4 + offSet);
                        goto case 4;
                    case 4:
                        v4 += (BufferUtil.GetShort(ref inputData, 2 + offSet) << 16) +
                              BufferUtil.GetShort(ref inputData, offSet);

                        break;
                }
            }
            else if (type == 3)
            {
                if (length > 12)
                {
                    blockCount = (length - 13) / 12 + 1;

                    do
                    {
                        int v53 = (inputData[5 + offSet] +
                                   (inputData[6 + offSet] + (inputData[7 + offSet] << 8) << 8) << 8) +
                                  inputData[4 + offSet] + v5;
                        int v54 = (inputData[9 + offSet] +
                                   (inputData[10 + offSet] + (inputData[11 + offSet] << 8) << 8) << 8) +
                                  v3 + inputData[8 + offSet];
                        int v55 = BitUtils.RotateIntLeft(v54, 4);
                        int v56 = inputData[offSet];
                        int v57 = inputData[1 + offSet] +
                                  (inputData[2 + offSet] + (inputData[3 + offSet] << 8) << 8) << 8;
                        int v58 = v56 + v57 - v54;
                        int v59 = v53 + v54;
                        int v60 = v4 + v58 ^ v55;
                        int v61 = v53 - v60;
                        int v62 = BitUtils.RotateIntLeft(v60, 6);
                        int v63 = v59 + v60;
                        int v64 = v61 ^ v62;
                        int v65 = v59 - v64;
                        int v66 = BitUtils.RotateIntLeft(v64, 8);
                        int v67 = v63 + v64;
                        int v68 = v65 ^ v66;
                        int v69 = v63 - v68;
                        int v70 = BitUtils.RotateIntLeft(v68, 16);
                        int v71 = v67 + v68;
                        int v72 = v69 ^ v70;
                        int v73 = v67 - v72;
                        int v74 = BitUtils.RotateIntRight(v72, 13);
                        v4 = v71 + v72;
                        int v75 = v73 ^ v74;
                        int v76 = BitUtils.RotateIntLeft(v75, 4);
                        v3 = v71 - v75 ^ v76;
                        v5 = v4 + v75;
                        length -= 12;
                        offSet += 12;
                        --blockCount;
                    } while (blockCount > 0);
                }

                remainingSize = inputData.Length - offSet;
                switch (remainingSize)
                {
                    case 0:
                        return (uint)v3 & 0xFFFFFFFF;
                    case 12:
                        v3 += inputData[11 + offSet] << 24;
                        goto case 11;
                    case 11:
                        v3 += inputData[10 + offSet] << 16;
                        goto case 10;
                    case 10:
                        v3 += inputData[9 + offSet] << 8;
                        goto case 9;
                    case 9:
                        v3 += inputData[8 + offSet];
                        goto case 8;
                    case 8:
                        v5 += inputData[7 + offSet] << 24;
                        goto case 7;
                    case 7:
                        v5 += inputData[6 + offSet] << 16;
                        goto case 6;
                    case 6:
                        v5 += inputData[5 + offSet] << 8;
                        goto case 5;
                    case 5:
                        v5 += inputData[4 + offSet];
                        goto case 4;
                    case 4:
                        v4 += inputData[3 + offSet] << 24;
                        goto case 3;
                    case 3:
                        v4 += inputData[2 + offSet] << 16;
                        goto case 2;
                    case 2:
                        v4 += inputData[1 + offSet] << 8;
                        goto case 1;
                    case 1:
                        v4 += inputData[offSet];
                        break;
                }
            }

            int v77 = BitUtils.RotateIntLeft(v5, 14);
            int v78 = (v5 ^ v3) - v77;
            int v79 = BitUtils.RotateIntLeft(v78, 11);
            int v80 = (v4 ^ v78) - v79;
            int v81 = BitUtils.RotateIntRight(v80, 7);
            int v82 = (v80 ^ v5) - v81;
            int v83 = BitUtils.RotateIntLeft(v82, 16);
            int v84 = (v82 ^ v78) - v83;
            int v85 = BitUtils.RotateIntLeft(v84, 4);
            int v86 = (v80 ^ v84) - v85;
            int v87 = BitUtils.RotateIntLeft(v86, 14);
            int v88 = (v86 ^ v82) - v87;
            int v89 = BitUtils.RotateIntRight(v88, 8);

            return (uint)((v88 ^ v84) - v89) & 0xFFFFFFFF;
        }
    }
}