using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using size_t = System.UIntPtr;

#pragma warning disable IDE1006 // Intentionally violating naming conventions because this is meant to match the C API
namespace RocksDbSharp
{
    /*
    These wrappers provide translation from the error output of the C API into exceptions
    */
    public abstract partial class Native
    {
        public void rocksdb_put(
            /*rocksdb_t**/ IntPtr db,
            /*const rocksdb_writeoptions_t**/ IntPtr writeOptions,
            string key,
            string val,
            ColumnFamilyHandle cf,
            System.Text.Encoding? encoding = null)
        {
            rocksdb_put(db, writeOptions, key, val, out IntPtr errptr, cf, encoding);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public void rocksdb_put(
            /*rocksdb_t**/ IntPtr db,
            /*const rocksdb_writeoptions_t**/ IntPtr writeOptions,
            string key,
            string val,
            System.Text.Encoding? encoding = null)
        {
            rocksdb_put(db, writeOptions, key, val, out IntPtr errptr, encoding);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public void rocksdb_put(
            IntPtr db,
            IntPtr writeOptions,
            byte[] key,
            long keyLength,
            byte[] value,
            long valueLength,
            ColumnFamilyHandle cf)
        {
            IntPtr errptr;
            UIntPtr sklength = (UIntPtr)keyLength;
            UIntPtr svlength = (UIntPtr)valueLength;
            rocksdb_put_cf(db, writeOptions, cf.Handle, key, sklength, value, svlength, out errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public void rocksdb_put(
            IntPtr db,
            IntPtr writeOptions,
            byte[] key,
            long keyLength,
            byte[] value,
            long valueLength)
        {
            IntPtr errptr;
            UIntPtr sklength = (UIntPtr)keyLength;
            UIntPtr svlength = (UIntPtr)valueLength;
            rocksdb_put(db, writeOptions, key, sklength, value, svlength, out errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }


        public string rocksdb_get(
            /*rocksdb_t**/ IntPtr db,
            /*const rocksdb_readoptions_t**/ IntPtr read_options,
            string key,
            ColumnFamilyHandle cf,
            System.Text.Encoding encoding = null)
        {
            var result = rocksdb_get(db, read_options, key, out IntPtr errptr, cf, encoding);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public IntPtr rocksdb_get(
            IntPtr db,
            IntPtr read_options,
            byte[] key,
            long keyLength,
            out long vallen,
            ColumnFamilyHandle cf)
        {
            UIntPtr sklength = (UIntPtr)keyLength;
            var result = cf == null
                ? rocksdb_get(db, read_options, key, sklength, out UIntPtr valLength, out IntPtr errptr)
                : rocksdb_get_cf(db, read_options, cf.Handle, key, sklength, out valLength, out errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            vallen = (long)valLength;
            return result;
        }

        public byte[] rocksdb_get(
            IntPtr db,
            IntPtr read_options,
            byte[] key,
            long keyLength = 0,
            ColumnFamilyHandle cf = null)
        {
            var result = rocksdb_get(db, read_options, key, keyLength == 0 ? key.Length : keyLength, out IntPtr errptr, cf);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public System.Collections.Generic.KeyValuePair<string, string>[] rocksdb_multi_get(
            IntPtr db,
            IntPtr read_options,
            string[] keys,
            ColumnFamilyHandle[] cf = null,
            System.Text.Encoding encoding = null)
        {
            if (encoding == null)
                encoding = System.Text.Encoding.UTF8;
            IntPtr[] errptrs = new IntPtr[keys.Length];
            var result = rocksdb_multi_get(db, read_options, keys, cf: cf, errptrs: errptrs, encoding: encoding);
            foreach (var errptr in errptrs)
                if (errptr != IntPtr.Zero)
                    throw new RocksDbException(errptr);
            return result;
        }


        public System.Collections.Generic.KeyValuePair<byte[], byte[]>[] rocksdb_multi_get(
            IntPtr db,
            IntPtr read_options,
            byte[][] keys,
            ulong[] keyLengths = null,
            ColumnFamilyHandle[] cf = null)
        {
            IntPtr[] errptrs = new IntPtr[keys.Length];
            var result = rocksdb_multi_get(db, read_options, keys, keyLengths: keyLengths, cf: cf, errptrs: errptrs);
            foreach (var errptr in errptrs)
                if (errptr != IntPtr.Zero)
                    throw new RocksDbException(errptr);
            return result;
        }

        public void rocksdb_delete(
            /*rocksdb_t**/ IntPtr db,
            /*const rocksdb_writeoptions_t**/ IntPtr writeOptions,
            /*const*/ string key,
            ColumnFamilyHandle cf)
        {
            rocksdb_delete(db, writeOptions, key, out IntPtr errptr, cf);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public void rocksdb_delete(
            /*rocksdb_t**/ IntPtr db,
            /*const rocksdb_writeoptions_t**/ IntPtr writeOptions,
            /*const*/ string key)
        {
            rocksdb_delete(db, writeOptions, key, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        [Obsolete("Use UIntPtr version instead")]
        public void rocksdb_delete(
            /*rocksdb_t**/ IntPtr db,
            /*const rocksdb_writeoptions_t**/ IntPtr writeOptions,
            /*const*/ byte[] key,
            long keylen)
        {
            UIntPtr sklength = (UIntPtr)keylen;
            rocksdb_delete(db, writeOptions, key, sklength, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        [Obsolete("Use UIntPtr version instead")]
        public void rocksdb_delete_cf(
            /*rocksdb_t**/ IntPtr db,
            /*const rocksdb_writeoptions_t**/ IntPtr writeOptions,
            /*const*/ byte[] key,
            long keylen,
            ColumnFamilyHandle cf)
        {
            rocksdb_delete_cf(db, writeOptions, cf.Handle, key, (UIntPtr)keylen, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        [Obsolete("Use UIntPtr version instead")]
        public void rocksdb_ingest_external_file(IntPtr db, string[] file_list, ulong list_len, IntPtr opt)
        {
            UIntPtr llen = (UIntPtr)list_len;
            rocksdb_ingest_external_file(db, file_list, llen, opt, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        [Obsolete("Use UIntPtr version instead")]
        public void rocksdb_ingest_external_file_cf(IntPtr db, IntPtr handle, string[] file_list, ulong list_len, IntPtr opt)
        {
            UIntPtr llen = (UIntPtr)list_len;
            rocksdb_ingest_external_file_cf(db, handle, file_list, llen, opt, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        [Obsolete("Use UIntPtr version instead")]
        public void rocksdb_sstfilewriter_add(
            IntPtr writer,
            byte[] key,
            ulong keylen,
            byte[] val,
            ulong vallen)
        {
            UIntPtr sklength = (UIntPtr)keylen;
            UIntPtr svlength = (UIntPtr)vallen;
            rocksdb_sstfilewriter_add(writer, key, sklength, val, svlength, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public unsafe void rocksdb_sstfilewriter_add(
            IntPtr writer,
            string key,
            string val)
        {
            rocksdb_sstfilewriter_add(writer, key, val, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public string rocksdb_writebatch_wi_get_from_batch(
            IntPtr wb,
            IntPtr options,
            string key,
            ColumnFamilyHandle cf,
            System.Text.Encoding encoding = null)
        {
            var result = rocksdb_writebatch_wi_get_from_batch(wb, options, key, out IntPtr errptr, cf, encoding);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public IntPtr rocksdb_writebatch_wi_get_from_batch(
            IntPtr wb,
            IntPtr options,
            byte[] key,
            ulong keyLength,
            out ulong vallen,
            ColumnFamilyHandle cf)
        {
            UIntPtr sklength = (UIntPtr)keyLength;
            var result = cf == null
                ? rocksdb_writebatch_wi_get_from_batch(wb, options, key, sklength, out UIntPtr valLength, out IntPtr errptr)
                : rocksdb_writebatch_wi_get_from_batch_cf(wb, options, cf.Handle, key, sklength, out valLength, out errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            vallen = (ulong)valLength;
            return result;
        }

        public byte[] rocksdb_writebatch_wi_get_from_batch(
            IntPtr wb,
            IntPtr options,
            byte[] key,
            ulong keyLength = 0,
            ColumnFamilyHandle cf = null)
        {
            var result = rocksdb_writebatch_wi_get_from_batch(wb, options, key, keyLength == 0 ? (ulong)key.Length : keyLength, out IntPtr errptr, cf);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public string rocksdb_writebatch_wi_get_from_batch_and_db(
            IntPtr wb,
            IntPtr db,
            IntPtr read_options,
            string key,
            ColumnFamilyHandle cf,
            System.Text.Encoding encoding = null)
        {
            var result = rocksdb_writebatch_wi_get_from_batch_and_db(wb, db, read_options, key, out IntPtr errptr, cf, encoding);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public IntPtr rocksdb_writebatch_wi_get_from_batch_and_db(
            IntPtr wb,
            IntPtr db,
            IntPtr read_options,
            byte[] key,
            ulong keyLength,
            out ulong vallen,
            ColumnFamilyHandle cf)
        {
            UIntPtr sklength = (UIntPtr)keyLength;
            var result = cf == null
                ? rocksdb_writebatch_wi_get_from_batch_and_db(wb, db, read_options, key, sklength, out UIntPtr valLength, out IntPtr errptr)
                : rocksdb_writebatch_wi_get_from_batch_and_db_cf(wb, db, read_options, cf.Handle, key, sklength, out valLength, out errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            vallen = (ulong)valLength;
            return result;
        }

        public byte[] rocksdb_writebatch_wi_get_from_batch_and_db(
            IntPtr wb,
            IntPtr db,
            IntPtr read_options,
            byte[] key,
            ulong keyLength = 0,
            ColumnFamilyHandle cf = null)
        {
            var result = rocksdb_writebatch_wi_get_from_batch_and_db(wb, db, read_options, key, keyLength == 0 ? (ulong)key.Length : keyLength, out IntPtr errptr, cf);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public void rocksdb_flushwal(
            /*rocksdb_t**/ IntPtr db, bool sync)
        {
            rocksdb_flushwal(db, (byte)(sync ? 1 : 0), out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public (IntPtr, UIntPtr) rocksdb_get_ptr(
            IntPtr db,
            IntPtr read_options,
            byte[] key,
            long keyLength = 0,
            ColumnFamilyHandle cf = null)
        {
            var result = rocksdb_get_ptr(db, read_options, key, keyLength == 0 ? key.Length : keyLength, out IntPtr errptr, cf);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public (IntPtr, UIntPtr) rocksdb_writebatch_wi_get_from_batch_and_db_ptr(
            IntPtr wb,
            IntPtr db,
            IntPtr read_options,
            byte[] key,
            ulong keyLength = 0,
            ColumnFamilyHandle? cf = null)
        {
            var result = rocksdb_writebatch_wi_get_from_batch_and_db_ptr(wb, db, read_options, key, keyLength == 0 ? (ulong)key.Length : keyLength, out IntPtr errptr, cf);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public (IntPtr, UIntPtr) rocksdb_writebatch_wi_get_from_batch_and_db_ptr(
            IntPtr wb,
            IntPtr db,
            IntPtr read_options,
            ReadOnlySpan<byte> key,
            ColumnFamilyHandle cf)
        {
            var result = rocksdb_writebatch_wi_get_from_batch_and_db_ptr(wb, db, read_options, key, out IntPtr errptr, cf);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public (IntPtr, UIntPtr) rocksdb_writebatch_wi_get_from_batch_and_db_ptr(
            IntPtr wb,
            IntPtr db,
            IntPtr read_options,
            ReadOnlySpan<byte> key)
        {
            var result = rocksdb_writebatch_wi_get_from_batch_and_db_ptr(wb, db, read_options, key, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public unsafe Span<byte> rocksdb_iter_key_span(IntPtr iterator)
        {
            IntPtr buffer = rocksdb_iter_key(iterator, out UIntPtr length);

            // Do not free, this is owned by the iterator and will be freed there
            //rocksdb_free(buffer);
            return new Span<byte>(buffer.ToPointer(), (int)length);
        }

        public unsafe Span<byte> rocksdb_iter_value_span(IntPtr iterator)
        {
            IntPtr buffer = rocksdb_iter_value(iterator, out UIntPtr length);
            // Do not free, this is owned by the iterator and will be freed there
            //rocksdb_free(buffer);
            return new Span<byte>(buffer.ToPointer(), (int)length);
        }

        public (IntPtr, UIntPtr) rocksdb_writebatch_wi_get_from_batch_ptr(
            IntPtr wb,
            IntPtr read_options,
            byte[] key,
            ulong keyLength = 0,
            ColumnFamilyHandle? cf = null)
        {
            var result = rocksdb_writebatch_wi_get_from_batch_ptr(wb, read_options, key, keyLength == 0 ? (ulong)key.Length : keyLength, out IntPtr errptr, cf);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public void rocksdb_put(
            IntPtr db,
            IntPtr writeOptions,
            ReadOnlySpan<byte> key,
            ReadOnlySpan<byte> value,
            ColumnFamilyHandle cf)
        {
            IntPtr errptr;
            UIntPtr sklength = (UIntPtr)key.Length;
            UIntPtr svlength = (UIntPtr)value.Length;
            if (cf == null)
                rocksdb_put(db, writeOptions, key.GetPinnableReference(), sklength, value.GetPinnableReference(), svlength, out errptr);
            else
                rocksdb_put_cf(db, writeOptions, cf.Handle, key.GetPinnableReference(), sklength, value.GetPinnableReference(), svlength, out errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public void rocksdb_put(
            IntPtr db,
            IntPtr writeOptions,
            ReadOnlySpan<byte> key,
            ReadOnlySpan<byte> value)
        {
            IntPtr errptr;
            UIntPtr sklength = (UIntPtr)key.Length;
            UIntPtr svlength = (UIntPtr)value.Length;
            rocksdb_put(db, writeOptions, key.GetPinnableReference(), sklength, value.GetPinnableReference(), svlength, out errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public (IntPtr, UIntPtr) rocksdb_get_ptr(
            IntPtr db,
            IntPtr read_options,
            ReadOnlySpan<byte> key,
            ColumnFamilyHandle cf)
        {
            var result = rocksdb_get_ptr(db, read_options, key, out IntPtr errptr, cf);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public (IntPtr, UIntPtr) rocksdb_get_ptr(
            IntPtr db,
            IntPtr read_options,
            ReadOnlySpan<byte> key)
        {
            var result = rocksdb_get_ptr(db, read_options, key, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
            return result;
        }

        public void rocksdb_delete(
            /*rocksdb_t**/ IntPtr db,
            /*const rocksdb_writeoptions_t**/ IntPtr writeOptions,
            /*const*/ ReadOnlySpan<byte> key)
        {
            UIntPtr sklength = (UIntPtr)key.Length;
            rocksdb_delete(db, writeOptions, key.GetPinnableReference(), sklength, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }

        public void rocksdb_delete_cf(
            /*rocksdb_t**/ IntPtr db,
            /*const rocksdb_writeoptions_t**/ IntPtr writeOptions,
            /*const*/ ReadOnlySpan<byte> key,
            ColumnFamilyHandle cf)
        {
            rocksdb_delete_cf(db, writeOptions, cf.Handle, key.GetPinnableReference(), (UIntPtr)key.Length, out IntPtr errptr);
            if (errptr != IntPtr.Zero)
                throw new RocksDbException(errptr);
        }
    }
}