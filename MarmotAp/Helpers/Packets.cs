using System;
using System.Collections.Generic;
using System.Text;

namespace MarmotAp.Helpers
{
    public class Packet10hz
    {
        /*
			//CDC_Device_Flush(&VirtualSerial_CDC_Interface);
			fputc(0x90, &USBSerialStream); //header packet mark1  
			fputc(0xEB, &USBSerialStream); //header packet mark2 
			fputc(0xEB, &USBSerialStream); //header packet mark3 
            fputc(0x90, &USBSerialStream); //header packet mark4  
            fputc(0x0A, &USBSerialStream); //header packet 10hz 
      
            statuspacket[5] = (int)oss_rpm & (0xff); //lo byte oss_rpm
            statuspacket[6] = oss_rpm >> 8; //hi byte oss_rpm
			statuspacket[7] = mph; //mph = 
			statuspacket[8] = (PINF & (1 << 5)); //LU = 
			statuspacket[9] = (PINF & (1 << 4)); //OD =
         * 
			statuspacket[10] = (int)set_psi & (0xff);	//set= 
			statuspacket[11] = feedback_psi;
            statuspacket[12] = (int)gov_pct & (0xff);	//gpo= 	
			statuspacket[13] = realoutput; //out= 
			statuspacket[14] = (int)error;	//error
         * 
			statuspacket[15] = leds; //i2c status packet
			statuspacket[16] = transmodifier; 
			statuspacket[17] = TPS; 
			statuspacket[18] = odUnlock; 
			statuspacket[19] = odLockout; 
         * 
			statuspacket[20] = TPS10; 
			statuspacket[21] = pressure12; 
			statuspacket[22] = pressure23; 
			statuspacket[23] = (int)eeprom_read_word(&_nvEEPROMBuildNumber) & (0xff); //lo byte; 
			statuspacket[24] = eeprom_read_word(&_nvEEPROMBuildNumber) >> 8; //hi byte; 
         * 
			statuspacket[25] = (int)codeBuildNumber & (0xff); //lo byte; 
			statuspacket[26] = codeBuildNumber >> 8; //hi byte; 
			statuspacket[27] = odlongpress; 
			statuspacket[28] = tunepress; 
			statuspacket[29] = tuneNum; 
         * 
			statuspacket[30] = loadedTune; 
        	statuspacket[31] = up12[TPS10]; 
		    statuspacket[32] = up23[TPS10]; 
		    statuspacket[33] = transtemp; 
		    statuspacket[34] = gear;  
         * 
            statuspacket[35] = gearup; 
		    statuspacket[36] = geardn; 
		    statuspacket[37] = manualmode; 
		    statuspacket[38] = 0; 
		    statuspacket[39] = (0xff);	//retard packet

         */

        public int oss_rpm { get; set; }
        public int mph { get; set; }
        public bool LU { get; set; }
        public bool OD { get; set; }

        public int setPSI { get; set; }
        public int feedbackPSI { get; set; }
        public int gpDutyPct { get; set; }
        public int realout { get; set; }
        public int error { get; set; }

        public byte i2cStatus { get; set; }
        public byte TransTempMod { get; set; }
        public byte TPS { get; set; }
        public byte odUnlock { get; set; }
        public bool odLockout { get; set; }

        public byte TPS10 { get; set; }
        public byte pressure12 { get; set; }
        public byte pressure23 { get; set; }
        public int EepromBuildNumber { get; set; }

        public int CodeBuildNumber { get; set; }
        public byte ODButtonPress { get; set; }
        public byte TuneButtonPress { get; set; }
        public byte requestedTune { get; set; }

        public byte loadedTune { get; set; }
        public byte speed12 { get; set; }
        public byte speed23 { get; set; }
        public int transtemp { get; set; }
        public int gear { get; set; }

        public bool gearup { get; set; }
        public bool geardn { get; set; }
        public bool manualmode { get; set; }
    }

    static class Parse
    {
        static public Packet10hz ToPacket10hz(byte[] raw)
        {
            Packet10hz p = new Packet10hz();

            //FileStream fs = new FileStream("_raw.bin", FileMode.Append);
            //BinaryWriter bw = new BinaryWriter(fs);
            //bw.Write(raw);
            //bw.Close();

            //raw[0]; //discard 10hz packet marker
            p.oss_rpm = Conv.ToUInt16(new byte[2] { raw[1], raw[2] });
            p.mph = Conv.ToUInt16(raw[3]);
            p.LU = raw[4] > 0;
            p.OD = raw[5] > 0;

            p.setPSI = Conv.ToUInt16(raw[6]);
            p.feedbackPSI = Conv.ToUInt16(raw[7]);
            p.gpDutyPct = Conv.ToUInt16(raw[8]) > 100 ? 100 : Conv.ToUInt16(raw[8]);
            p.realout = (sbyte)raw[9];
            p.error = Conv.ToUInt16(raw[10]);

            p.i2cStatus = raw[11];
            p.TransTempMod = raw[12];
            p.TPS = raw[13];
            p.odUnlock = raw[14];
            p.odLockout = raw[15] > 0;

            p.TPS10 = raw[16];
            p.pressure12 = raw[17];
            p.pressure23 = raw[18];
            p.EepromBuildNumber = Conv.ToUInt16(new byte[2] { raw[19], raw[20] });

            p.CodeBuildNumber = Conv.ToUInt16(new byte[2] { raw[21], raw[22] });
            p.ODButtonPress = raw[23];
            p.TuneButtonPress = raw[24];
            p.requestedTune = raw[25];

            p.loadedTune = raw[26];
            p.speed12 = raw[27];
            p.speed23 = raw[28];
            p.transtemp = raw[29];
            p.gear = raw[30];

            p.gearup = raw[31] > 0;
            p.geardn = raw[32] > 0;
            p.manualmode = raw[33] > 0;

            return p;
        }
    }


    public static class Conv
    {
        public static UInt16 ToUInt16(byte[] byteUInt16)
        {
            return BitConverter.ToUInt16(new byte[2] { byteUInt16[0], byteUInt16[1] }, 0);
        }

        public static UInt16 ToUInt16(byte byteUInt16)
        {
            return BitConverter.ToUInt16(new byte[2] { byteUInt16, 0x00 }, 0);
        }
    }
}
