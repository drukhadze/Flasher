﻿namespace MicroFlasher.STKv1 {
    public struct StkV1DeviceParameters {

        public StkDeviceCode DeviceCode;
        public byte Revision;
        public byte ProgType;
        public byte ParMode;
        public byte Polling;
        public byte SelfTimed;
        public byte LockBytes;
        public byte FuseBytes;
        public byte FlashPollVal1;
        public byte FlashPollVal2;
        public byte EepromPollVal1;
        public byte EepromPollVal2;
        public ushort PageSize;
        public ushort EepromSize;
        public uint FlashSize;

        public byte PageSizeHigh {
            get { return (byte)(PageSize >> 8); }
        }

        public byte PageSizeLow {
            get { return (byte)(PageSize & 0xff); }
        }

        public byte EepromSizeHigh {
            get { return (byte)(EepromSize >> 8); }
        }

        public byte EepromSizeLow {
            get { return (byte)(EepromSize & 0xff); }
        }

        public byte FlashSize4 {
            get { return (byte)(FlashSize >> 24); }
        }

        public byte FlashSize3 {
            get { return (byte)(FlashSize >> 16); }
        }

        public byte FlashSize2 {
            get { return (byte)(FlashSize >> 8); }
        }

        public byte FlashSize1 {
            get { return (byte)(FlashSize >> 0); }
        }
    }
}
