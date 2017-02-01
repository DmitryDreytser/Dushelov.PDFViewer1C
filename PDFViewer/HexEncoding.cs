// Decompiled with JetBrains decompiler
// Type: PDFViewer.HexEncoding
// Assembly: Dushelov.PDFViewer1C, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b409ed73a72e873d
// MVID: B4BB142D-624D-482F-81D7-26DCE4894A91
// Assembly location: L:\Projects\1C_PDF\Dushelov.PDFViewer1C.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace PDFViewer
{
  internal class HexEncoding
  {
    private static string[] encode1 = new string[19]
    {
      "G",
      "H",
      "I",
      "J",
      "K",
      "L",
      "M",
      "N",
      "O",
      "P",
      "Q",
      "R",
      "S",
      "T",
      "U",
      "V",
      "W",
      "X",
      "Y"
    };
    private static Dictionary<int, string> encode2 = new Dictionary<int, string>();

    public static int GetByteCount(string hexString)
    {
      int num = 0;
      for (int index = 0; index < hexString.Length; ++index)
      {
        if (HexEncoding.IsHexDigit(hexString[index]))
          ++num;
      }
      if (num % 2 != 0)
        --num;
      return num / 2;
    }

    public static byte[] GetBytes(string hexString, out int discarded)
    {
      discarded = 0;
      string str = "";
      for (int index = 0; index < hexString.Length; ++index)
      {
        char c = hexString[index];
        if (HexEncoding.IsHexDigit(c))
          str += (string) (object) c;
        else
          ++discarded;
      }
      if (str.Length % 2 != 0)
      {
        ++discarded;
        str = str.Substring(0, str.Length - 1);
      }
      byte[] numArray = new byte[str.Length / 2];
      int index1 = 0;
      for (int index2 = 0; index2 < numArray.Length; ++index2)
      {
        string hex = new string(new char[2]
        {
          str[index1],
          str[index1 + 1]
        });
        numArray[index2] = HexEncoding.HexToByte(hex);
        index1 += 2;
      }
      return numArray;
    }

    public static string ToString(byte[] bytes)
    {
      StringBuilder stringBuilder = new StringBuilder();
      for (int index = 0; index < bytes.Length; ++index)
        stringBuilder.Append(bytes[index].ToString("X2"));
      return stringBuilder.ToString();
    }

    public static bool InHexFormat(string hexString)
    {
      bool flag = true;
      foreach (char c in hexString)
      {
        if (!HexEncoding.IsHexDigit(c))
        {
          flag = false;
          break;
        }
      }
      return flag;
    }

    public static bool IsHexDigit(char c)
    {
      int int32_1 = Convert.ToInt32('A');
      int int32_2 = Convert.ToInt32('0');
      c = char.ToUpper(c);
      int int32_3 = Convert.ToInt32(c);
      return int32_3 >= int32_1 && int32_3 < int32_1 + 6 || int32_3 >= int32_2 && int32_3 < int32_2 + 10;
    }

    private static byte HexToByte(string hex)
    {
      if (hex.Length > 2 || hex.Length <= 0)
        throw new ArgumentException("hex must be 1 or 2 characters in length");
      return byte.Parse(hex, NumberStyles.HexNumber);
    }

    public static string CompressHex(string hexData)
    {
      if (HexEncoding.encode2.Count == 0)
      {
        HexEncoding.encode2.Add(20, "g");
        HexEncoding.encode2.Add(40, "h");
        HexEncoding.encode2.Add(60, "i");
        HexEncoding.encode2.Add(80, "j");
        HexEncoding.encode2.Add(100, "k");
        HexEncoding.encode2.Add(120, "l");
        HexEncoding.encode2.Add(140, "m");
        HexEncoding.encode2.Add(160, "n");
        HexEncoding.encode2.Add(180, "o");
        HexEncoding.encode2.Add(200, "p");
        HexEncoding.encode2.Add(220, "q");
        HexEncoding.encode2.Add(240, "r");
        HexEncoding.encode2.Add(260, "s");
        HexEncoding.encode2.Add(280, "t");
        HexEncoding.encode2.Add(300, "u");
        HexEncoding.encode2.Add(320, "v");
        HexEncoding.encode2.Add(340, "w");
        HexEncoding.encode2.Add(360, "x");
        HexEncoding.encode2.Add(380, "y");
        HexEncoding.encode2.Add(400, "z");
      }
      StringBuilder stringBuilder = new StringBuilder();
      string lastChar = hexData.Substring(0, 1);
      int countChar1 = 0;
      string str = "";
      for (int startIndex = 1; startIndex < hexData.Length; ++startIndex)
      {
        if (hexData.Substring(startIndex, 1) == lastChar)
          ++countChar1;
        else if (countChar1 > 0)
        {
          if (countChar1 > 419)
          {
            int num1 = 2;
            int num2 = countChar1;
            int countChar2 = 0;
            while (countChar1 / num1 > 419)
              ++num1;
            do
            {
              if (countChar2 == 0)
              {
                countChar2 = countChar1 / num1;
                num2 = countChar1 - num1;
              }
              else
              {
                if (countChar2 > num2)
                  countChar2 = num2;
                num2 -= countChar2;
              }
              str += HexEncoding.getEncodedString(countChar2, lastChar);
            }
            while (num2 > 0);
          }
          else
            str = HexEncoding.getEncodedString(countChar1, lastChar);
          stringBuilder.Append(str);
          countChar1 = 0;
          lastChar = hexData.Substring(startIndex, 1);
        }
      }
      return stringBuilder.ToString();
    }

    private static string getEncodedString(int countChar, string lastChar)
    {
      string str = "";
      int num = countChar % 20;
      if (countChar - num > 0)
        str = HexEncoding.encode2[countChar - num];
      if (num > 0)
        str += HexEncoding.encode1[num - 1];
      return str + lastChar;
    }
  }
}
