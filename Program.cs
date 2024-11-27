// MIT License
// 
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.IO;
using System.Text;

namespace ext_g1t_4_pssg
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Get all files in the current directory
            string[] files = Directory.GetFiles(Directory.GetCurrentDirectory());

            foreach (var file in files)
            {
                if (file.ToUpper().Contains(".PSSG"))
                {
                    try
                    {
                        Console.WriteLine(file);
                        using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                        using (BinaryReader reader = new BinaryReader(fs))
                        {
                            byte[] fileContent = reader.ReadBytes((int)fs.Length);
                            int index = FindIndex(fileContent, new byte[] { 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x04 });

                            // Loop while a matching identifier is found
                            while (index > -1)
                            {
                                fs.Seek(index + 8, SeekOrigin.Begin);
                                int size = BitConverter.ToInt32(ReverseBytes(fileContent, index + 8, 4), 0); // Big-endian unsigned integer

                                // Extract filename
                                fs.Seek(11, SeekOrigin.Current);
                                int nameLength = reader.ReadByte();
                                string name = Encoding.ASCII.GetString(reader.ReadBytes(nameLength));

                                // Ensure filename is valid
                                foreach (char c in Path.GetInvalidFileNameChars())
                                {
                                    name = name.Replace(c, '_');
                                }

                                // Extract data block
                                fs.Seek(12, SeekOrigin.Current);
                                byte[] data = reader.ReadBytes(size);

                                // Write extracted data to a new file
                                using (FileStream g1t = new FileStream(name, FileMode.Create, FileAccess.Write))
                                {
                                    g1t.Write(data, 0, data.Length);
                                }

                                // Find the next matching identifier
                                index = FindIndex(fileContent, new byte[] { 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x04 }, index + 8);
                            }
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"Failed to process file '{file}': {ex.Message}");
                    }
                }
            }
        }

        static int FindIndex(byte[] source, byte[] pattern, int startIndex = 0)
        {
            int maxIndex = source.Length - pattern.Length + 1;
            for (int i = startIndex; i < maxIndex; i++)
            {
                bool isMatch = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (source[i + j] != pattern[j])
                    {
                        isMatch = false;
                        break;
                    }
                }
                if (isMatch)
                {
                    return i;
                }
            }
            return -1;
        }

        static byte[] ReverseBytes(byte[] source, int startIndex, int length)
        {
            byte[] reversed = new byte[length];
            for (int i = 0; i < length; i++)
            {
                reversed[i] = source[startIndex + length - 1 - i];
            }
            return reversed;
        }
    }
}
