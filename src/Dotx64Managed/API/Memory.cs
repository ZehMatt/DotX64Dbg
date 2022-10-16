using System;

namespace Dotx64Dbg
{
    public static partial class Memory
    {
        [Flags]
        public enum Protection
        {
            Invalid = 0,
            NoAccess = 0x01,
            ReadOnly = 0x02,
            ReadWrite = 0x04,
            WriteCopy = 0x08,
            Execute = 0x10,
            ExecuteRead = 0x20,
            ExecuteWriteRead = 0x40,
            ExecuteWriteCopy,
            // Extra flags.
            Guard = 0x100,
            NoCache = 0x200,
            WriteCombine = 0x400,
            // CFG Settings.
            PageTargetsInvalid = 0x40000000,
            PageTargetsNoUpdate = 0x40000000,
        }

        /// <summary>
        /// Read memory from the debugged process.
        /// </summary>
        /// <param name="addr">Virtual address in the debugged process space</param>
        /// <param name="length">Amount of bytes to read</param>
        /// <returns>The bytes read from the process, null if the read failed.</returns>
        public static byte[] Read(nuint addr, int length)
        {
            return Native.Memory.Read(addr, length);
        }
        /// <see cref="Read(nuint, int)"/>
        public static byte[] Read(nuint addr, nuint length)
        {
            return Native.Memory.Read(addr, (int)length);
        }
        /// <see cref="Read(nuint, int)"/>
        public static byte[] Read(ulong addr, int length)
        {
            return Native.Memory.Read((nuint)addr, length);
        }

        /// <summary>
        /// Writes the bytes passed in data by length to the memory in the debugged process.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <param name="data">The bytes to be written</param>
        /// <param name="length">The maximum amount of bytes to write, can not be bigger than `data`</param>
        /// <returns>The amount of bytes written</returns>
        public static int Write(nuint address, byte[] data, int length)
        {
            return Native.Memory.Write(address, data, length);
        }
        /// <see cref="Write(nuint, byte[], int)"/>
        public static int Write(nuint address, byte[] data, nuint length)
        {
            return Native.Memory.Write(address, data, (int)length);
        }
        /// <see cref="Write(nuint, byte[], int)"/>
        public static int Write(ulong address, byte[] data, int length)
        {
            return Native.Memory.Write((nuint)address, data, length);
        }

        /// <summary>
        /// Writes all bytes passed in data to the memory in the debugged process.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <param name="data">The bytes to be written</param>
        /// <returns>The amount of bytes written</returns>
        public static int Write(nuint address, byte[] data)
        {
            return Write(address, data, data.Length);
        }
        /// <see cref="Write(nuint, byte[])"/>
        public static int Write(ulong address, byte[] data)
        {
            return Write((nuint)address, data, data.Length);
        }

        public static nuint GetSize(nuint address)
        {
            return (nuint)Native.Memory.GetSize(address);
        }
        public static nuint GetSize(ulong address)
        {
            return GetSize((nuint)address);
        }

        public static nuint GetBase(nuint address)
        {
            return Native.Memory.GetBase(address);
        }
        public static nuint GetBase(ulong address)
        {
            return GetBase((nuint)address);
        }

        /// <summary>
        /// Gets the protection of the memory, if the cache is used this is the last queried page info.
        /// It is normally safe to use the cache for performance, when the cache is used the internal
        /// API will not use a syscall to determine the protection.
        /// </summary>
        /// <param name="address">Address of the page to query</param>
        /// <param name="useCache">If this is true it will use the last queried page information</param>
        /// <returns>In case of failure the result is Protection.Invalid otherwise actual protection</returns>
        public static Protection GetProtection(nuint address, bool useCache)
        {
            return (Protection)Native.Memory.GetProtection(address, useCache);
        }
        /// <see cref="Memory.GetProtection(nuint, bool)"/>
        public static Protection GetProtection(ulong address, bool useCache) => GetProtection((nuint)address, useCache);

        /// <summary>
        /// Sets a new protection on the specified address, the address will be aligned to page
        /// boundaries and sets the entire page which is by 4 KiB. This may split up
        /// an existing range from the memory map.
        /// Internally the size will be always aligned to a minimum of a single page, if the size
        /// spans more than two pages then both pages will be modified.
        /// <note>This will also update the cached protection info</note>
        /// </summary>
        /// <param name="address">Address of the page</param>
        /// <param name="protect">New protection</param>
        /// <param name="size">The size of the range</param>
        /// <returns>True on success</returns>
        public static bool SetProtection(nuint address, Protection protect, int size)
        {
            return Native.Memory.SetProtection(address, (UInt32)protect, size);
        }
        /// <see cref="Memory.SetProtection(nuint, Protection, int)"/>
        public static bool SetProtection(ulong address, Protection protect, int size) => SetProtection((nuint)address, protect, size);

        /// <summary>
        /// Reads a byte from the specified address, throws if the read failed.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <returns>The value if successful</returns>
        public static byte ReadByte(nuint address)
        {
            byte[] data = Read(address, sizeof(byte));
            if (data == null)
                throw new Exception($"Unable to read byte at address {address:X}");
            return data[0];
        }
        /// <see cref="Memory.ReadByte(nuint)"/>
        public static byte ReadByte(ulong address) => ReadByte((nuint)address);

        /// <summary>
        /// Writes a byte to the specified address.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <param name="value">Value to write</param>
        /// <returns>Returns true if the write was successful</returns>
        public static bool WriteByte(nuint address, byte value)
        {
            return Write(address, new byte[] { value }, sizeof(byte)) == sizeof(byte);
        }
        /// <see cref="Memory.WriteByte(nuint, byte)"/>
        public static bool WriteByte(ulong address, byte value) => WriteByte((nuint)address, value);

        /// <summary>
        /// Reads a word from the specified address, throws if the read failed.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <returns>The value if successful</returns>
        public static UInt16 ReadWord(nuint address)
        {
            byte[] data = Read(address, sizeof(UInt16));
            if (data == null)
                throw new Exception($"Unable to read word at address {address:X}");
            return BitConverter.ToUInt16(data);
        }
        /// <see cref="Memory.ReadWord(nuint)"/>
        public static UInt16 ReadWord(ulong address) => ReadWord((nuint)address);

        /// <summary>
        /// Writes a word to the specified address.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <param name="value">Value to write</param>
        /// <returns>Returns true if the write was successful</returns>
        public static bool WriteWord(nuint address, UInt16 value)
        {
            return Write(address, BitConverter.GetBytes(value), sizeof(UInt16)) == sizeof(UInt16);
        }
        /// <see cref="Memory.WriteWord(nuint, UInt16)"/>
        public static bool WriteWord(ulong address, UInt16 value) => WriteWord((nuint)address, value);

        /// <summary>
        /// Reads a dword from the specified address, throws if the read failed.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <returns>The value if successful</returns>
        public static UInt32 ReadDword(nuint address)
        {
            byte[] data = Read(address, sizeof(UInt32));
            if (data == null)
                throw new Exception($"Unable to read dword at address {address:X}");
            return BitConverter.ToUInt32(data);
        }
        /// <see cref="Memory.ReadDword(nuint)"/>
        public static UInt32 ReadDword(ulong address) => ReadDword((nuint)address);

        /// <summary>
        /// Writes a dword to the specified address.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <param name="value">Value to write</param>
        /// <returns>Returns true if the write was successful</returns>
        public static bool WriteDword(nuint address, UInt32 value)
        {
            return Write(address, BitConverter.GetBytes(value), sizeof(UInt32)) == sizeof(UInt32);
        }
        /// <see cref="Memory.WriteDword(nuint, UInt32)"/>
        public static bool WriteDword(ulong address, UInt32 value) => WriteDword((nuint)address, value);

        /// <summary>
        /// Reads a qword from the specified address, throws if the read failed.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <returns>The value if successful</returns>
        public static UInt64 ReadQword(nuint address)
        {
            byte[] data = Read(address, sizeof(UInt64));
            if (data == null)
                throw new Exception($"Unable to read dword at address {address:X}");
            return BitConverter.ToUInt64(data);
        }
        /// <see cref="Memory.ReadQword(nuint)"/>
        public static UInt64 ReadQword(ulong address) => ReadQword((nuint)address);

        /// <summary>
        /// Writes a qword to the specified address.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <param name="value">Value to write</param>
        /// <returns>Returns true if the write was successful</returns>
        public static bool WriteQword(nuint address, UInt64 value)
        {
            return Write(address, BitConverter.GetBytes(value), sizeof(UInt64)) == sizeof(UInt64);
        }
        /// <see cref="Memory.WriteQword(nuint, UInt64)"/>
        public static bool WriteQword(ulong address, UInt64 value) => WriteQword((nuint)address, value);

        /// <summary>
        /// Reads a pointer from the specified address, throws if the read failed.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <returns>The value if successful</returns>
        public static nuint ReadPtr(nuint address)
        {
#if _X64_
            var size = 8;
#else
            var size = 4;
#endif
            byte[] data = Read(address, size);
            if (data == null)
                throw new Exception($"Unable to read ptr at address {address:X}");
#if _X64_
            return (nuint)BitConverter.ToUInt64(data);
#else
            return (nuint)BitConverter.ToUInt32(data);
#endif
        }

        /// <summary>
        /// Writes a pointer with pointer size to the specified address.
        /// </summary>
        /// <param name="address">Virtual address in the debugged process space</param>
        /// <param name="value">Value to write</param>
        /// <returns>Returns true if the write was successful</returns>
        public static bool WritePtr(nuint address, nuint value)
        {
#if _X64_
            var size = 8;
#else
            var size = 4;
#endif
            return Write(address, BitConverter.GetBytes(value), size) == size;
        }
    };
}
