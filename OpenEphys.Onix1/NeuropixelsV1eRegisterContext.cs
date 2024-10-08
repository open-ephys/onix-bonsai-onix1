﻿using System;
using System.Collections;
using System.IO;
using System.Linq;

namespace OpenEphys.Onix1
{
    class NeuropixelsV1eRegisterContext : I2CRegisterContext
    {
        public double ApGainCorrection { get; }
        public double LfpGainCorrection { get; }
        public ushort[] AdcThresholds { get; }
        public ushort[] AdcOffsets { get; }

        const int ShankConfigurationBitCount = 968;
        const int BaseConfigurationBitCount = 2448;
        const int BaseConfigurationConfigOffset = 576;
        const uint ShiftRegisterSuccess = 1 << 7;

        readonly NeuropixelsV1eAdc[] Adcs = new NeuropixelsV1eAdc[NeuropixelsV1e.AdcCount];
        readonly BitArray ShankConfig = new(ShankConfigurationBitCount, false);
        readonly BitArray[] BaseConfigs = { new(BaseConfigurationBitCount, false),   // Ch 0, 2, 4, ...
                                            new(BaseConfigurationBitCount, false) }; // Ch 1, 3, 5, ...

        public NeuropixelsV1eRegisterContext(DeviceContext deviceContext, uint i2cAddress, ulong probeSerialNumber,
            NeuropixelsV1eProbeConfiguration probeConfiguration, string gainCalibrationFile, string adcCalibrationFile)
            : base(deviceContext, i2cAddress)
        {
            if (!File.Exists(gainCalibrationFile))
            {
                throw new ArgumentException($"A gain calibration file must be specified for the probe with serial number " +
                    $"{probeSerialNumber}");
            }
            
            if (!File.Exists(adcCalibrationFile))
            {
                throw new ArgumentException($"An ADC calibration file must be specified for the probe with serial number " +
                    $"{probeSerialNumber}");
            }

            var adcCalibration = NeuropixelsV1Helper.TryParseAdcCalibrationFile(adcCalibrationFile);

            if (!adcCalibration.HasValue)
            {
                throw new ArgumentException($"The calibration file \"{adcCalibrationFile}\" is invalid.");
            }

            if (adcCalibration.Value.SerialNumber != probeSerialNumber)
            {
                throw new ArgumentException($"The probe serial number ({probeSerialNumber}) does not " +
                    $"match the ADC calibration file serial number: {adcCalibration.Value.SerialNumber}.");
            }

            var gainCorrection = NeuropixelsV1Helper.TryParseGainCalibrationFile(gainCalibrationFile, 
                probeConfiguration.SpikeAmplifierGain, probeConfiguration.LfpAmplifierGain, NeuropixelsV1e.ElectrodeCount);

            if (!gainCorrection.HasValue)
            {
                throw new ArgumentException($"The calibration file \"{gainCalibrationFile}\" is invalid.");
            }

            if (gainCorrection.Value.SerialNumber != probeSerialNumber)
            {
                throw new ArgumentException($"The probe serial number ({probeSerialNumber}) does not " +
                    $"match the gain calibration file serial number: {gainCorrection.Value.SerialNumber}.");
            }

            ApGainCorrection = gainCorrection.Value.ApGainCorrectionFactor;
            LfpGainCorrection = gainCorrection.Value.LfpGainCorrectionFactor;

            Adcs = adcCalibration.Value.Adcs;
            AdcThresholds = Adcs.ToList().Select(a => (ushort)a.Threshold).ToArray();
            AdcOffsets = Adcs.ToList().Select(a => (ushort)a.Offset).ToArray();

            // Update active channels
            ShankConfig = MakeShankBits(probeConfiguration);

            // create base shift-register bit arrays
            for (int i = 0; i < NeuropixelsV1e.ChannelCount; i++)
            {
                var configIdx = i % 2;

                // References
                var refIdx = configIdx == 0 ?
                    (382 - i) / 2 * 3 :
                    (383 - i) / 2 * 3;

                BaseConfigs[configIdx][refIdx + 0] = ((byte)probeConfiguration.Reference >> 0 & 0x1) == 1;
                BaseConfigs[configIdx][refIdx + 1] = ((byte)probeConfiguration.Reference >> 1 & 0x1) == 1;
                BaseConfigs[configIdx][refIdx + 2] = ((byte)probeConfiguration.Reference >> 2 & 0x1) == 1;

                var chanOptsIdx = BaseConfigurationConfigOffset + ((i - configIdx) * 4);

                // MSB [Full, standby, LFPGain(3 downto 0), APGain(3 downto 0)] LSB

                BaseConfigs[configIdx][chanOptsIdx + 0] = ((byte)probeConfiguration.SpikeAmplifierGain >> 0 & 0x1) == 1;
                BaseConfigs[configIdx][chanOptsIdx + 1] = ((byte)probeConfiguration.SpikeAmplifierGain >> 1 & 0x1) == 1;
                BaseConfigs[configIdx][chanOptsIdx + 2] = ((byte)probeConfiguration.SpikeAmplifierGain >> 2 & 0x1) == 1;

                BaseConfigs[configIdx][chanOptsIdx + 3] = ((byte)probeConfiguration.LfpAmplifierGain >> 0 & 0x1) == 1;
                BaseConfigs[configIdx][chanOptsIdx + 4] = ((byte)probeConfiguration.LfpAmplifierGain >> 1 & 0x1) == 1;
                BaseConfigs[configIdx][chanOptsIdx + 5] = ((byte)probeConfiguration.LfpAmplifierGain >> 2 & 0x1) == 1;

                BaseConfigs[configIdx][chanOptsIdx + 6] = false;
                BaseConfigs[configIdx][chanOptsIdx + 7] = !!probeConfiguration.SpikeFilter;; // Full bandwidth = 1, filter on = 0

            }

            int k = 0;
            foreach (var adc in Adcs)
            {
                if (adc.CompP < 0 || adc.CompP > 0x1F)
                {
                    throw new ArgumentOutOfRangeException($"ADC calibration parameter CompP value of {adc.CompP} is invalid.");
                }

                if (adc.CompN < 0 || adc.CompN > 0x1F)
                {
                    throw new ArgumentOutOfRangeException($"ADC calibration parameter CompN value of {adc.CompN} is invalid.");
                }

                if (adc.Cfix < 0 || adc.Cfix > 0xF)
                {
                    throw new ArgumentOutOfRangeException($"ADC calibration parameter Cfix value of {adc.Cfix} is invalid.");
                }

                if (adc.Slope < 0 || adc.Slope > 0x7)
                {
                    throw new ArgumentOutOfRangeException($"ADC calibration parameter Slope value of {adc.Slope} is invalid.");
                }

                if (adc.Coarse < 0 || adc.Coarse > 0x3)
                {
                    throw new ArgumentOutOfRangeException($"ADC calibration parameter Coarse value of {adc.Coarse} is invalid.");
                }

                if (adc.Fine < 0 || adc.Fine > 0x3)
                {
                    throw new ArgumentOutOfRangeException($"ADC calibration parameter Fine value of {adc.Fine} is invalid.");
                }

                var configIdx = k % 2;
                int d = k++ / 2;

                int compOffset = 2406 - 42 * (d / 2) + (d % 2) * 10;
                int slopeOffset = compOffset + 20 + (d % 2);

                var compP = new BitArray(new byte[] { (byte)adc.CompP });
                var compN = new BitArray(new byte[] { (byte)adc.CompN });
                var cfix = new BitArray(new byte[] { (byte)adc.Cfix });
                var slope = new BitArray(new byte[] { (byte)adc.Slope });
                var coarse = (new BitArray(new byte[] { (byte)adc.Coarse }));
                var fine = new BitArray(new byte[] { (byte)adc.Fine });

                BaseConfigs[configIdx][compOffset + 0] = compP[0];
                BaseConfigs[configIdx][compOffset + 1] = compP[1];
                BaseConfigs[configIdx][compOffset + 2] = compP[2];
                BaseConfigs[configIdx][compOffset + 3] = compP[3];
                BaseConfigs[configIdx][compOffset + 4] = compP[4];

                BaseConfigs[configIdx][compOffset + 5] = compN[0];
                BaseConfigs[configIdx][compOffset + 6] = compN[1];
                BaseConfigs[configIdx][compOffset + 7] = compN[2];
                BaseConfigs[configIdx][compOffset + 8] = compN[3];
                BaseConfigs[configIdx][compOffset + 9] = compN[4];

                BaseConfigs[configIdx][slopeOffset + 0] = slope[0];
                BaseConfigs[configIdx][slopeOffset + 1] = slope[1];
                BaseConfigs[configIdx][slopeOffset + 2] = slope[2];

                BaseConfigs[configIdx][slopeOffset + 3] = fine[0];
                BaseConfigs[configIdx][slopeOffset + 4] = fine[1];

                BaseConfigs[configIdx][slopeOffset + 5] = coarse[0];
                BaseConfigs[configIdx][slopeOffset + 6] = coarse[1];

                BaseConfigs[configIdx][slopeOffset + 7] = cfix[0];
                BaseConfigs[configIdx][slopeOffset + 8] = cfix[1];
                BaseConfigs[configIdx][slopeOffset + 9] = cfix[2];
                BaseConfigs[configIdx][slopeOffset + 10] = cfix[3];

            }
        }

        public void InitializeProbe()
        {
            // get probe set up to receive configuration
            WriteByte(NeuropixelsV1e.CAL_MOD, (uint)NeuropixelsV1CalibrationRegisterValues.CAL_OFF);
            WriteByte(NeuropixelsV1e.TEST_CONFIG1, 0);
            WriteByte(NeuropixelsV1e.TEST_CONFIG2, 0);
            WriteByte(NeuropixelsV1e.TEST_CONFIG3, 0);
            WriteByte(NeuropixelsV1e.TEST_CONFIG4, 0);
            WriteByte(NeuropixelsV1e.TEST_CONFIG5, 0);
            WriteByte(NeuropixelsV1e.SYNC, 0);
            WriteByte(NeuropixelsV1e.REC_MOD, (uint)NeuropixelsV1RecordRegisterValues.ACTIVE);
            WriteByte(NeuropixelsV1e.OP_MODE, (uint)NeuropixelsV1OperationRegisterValues.RECORD);
        }

        public void WriteConfiguration()
        {
            // shank configuration
            // NB: no read check because of ASIC bug that is documented in IMEC-API comments
            var shankBytes = BitHelper.ToBitReversedBytes(ShankConfig);

            WriteByte(NeuropixelsV1e.SR_LENGTH1, (uint)shankBytes.Length % 0x100);
            WriteByte(NeuropixelsV1e.SR_LENGTH2, (uint)shankBytes.Length / 0x100);

            foreach (var b in shankBytes)
            {
               WriteByte(NeuropixelsV1e.SR_CHAIN1, b);
            }

            // base configuration
            for (int i = 0; i < BaseConfigs.Length; i++)
            {
                var srAddress = i == 0 ? NeuropixelsV1e.SR_CHAIN2 : NeuropixelsV1e.SR_CHAIN3;

                for (int j = 0; j < 2; j++)
                {
                    // WONTFIX: Without this reset, the ShiftRegisterSuccess check below will always fail
                    // on whatever the second shift register write sequence regardless of order or
                    // contents. Could be increased current draw during internal process causes MCLK
                    // to droop and mess up internal state. Or that MCLK is just not good enough to
                    // prevent metastability in some logic in the ASIC that is only entered in between
                    // SR accesses.
                    WriteByte(NeuropixelsV1e.SOFT_RESET, 0xFF);
                    WriteByte(NeuropixelsV1e.SOFT_RESET, 0x00);

                    var baseBytes = BitHelper.ToBitReversedBytes(BaseConfigs[i]);

                    WriteByte(NeuropixelsV1e.SR_LENGTH1, (uint)baseBytes.Length % 0x100);
                    WriteByte(NeuropixelsV1e.SR_LENGTH2, (uint)baseBytes.Length / 0x100);

                    foreach (var b in baseBytes)
                    {
                        WriteByte(srAddress, b);
                    }
                }

                if (ReadByte(NeuropixelsV1e.STATUS) != ShiftRegisterSuccess)
                {
                    throw new InvalidOperationException($"Shift register {srAddress} status check failed.");
                }
            }
        }

        public void StartAcquisition()
        {
            // WONTFIX: Soft reset inside settings.WriteShiftRegisters() above puts probe in reset set that
            // needs to be undone here
            WriteByte(NeuropixelsV1e.OP_MODE, (uint)NeuropixelsV1OperationRegisterValues.RECORD);
            WriteByte(NeuropixelsV1e.REC_MOD, (uint)NeuropixelsV1RecordRegisterValues.ACTIVE);
        }

        public static BitArray MakeShankBits(NeuropixelsV1eProbeConfiguration probeConfiguration)
        {
            const int ShankConfigurationBitCount = 968;
            const int ShankBitExt1 = 965;
            const int ShankBitExt2 = 2;
            const int ShankBitTip1 = 484;
            const int ShankBitTip2 = 483;
            const int InternalReferenceChannel = 191;

            BitArray shankBits = new(ShankConfigurationBitCount);

            foreach (var e in probeConfiguration.ChannelMap)
            {
                if (e.Index == InternalReferenceChannel) continue;

                int bitIndex = e.Index % 2 == 0 ?
                        485 + (e.Index / 2) : // even electrode
                        482 - (e.Index / 2);  // odd electrode

                shankBits[bitIndex] = true;
            }

            switch (probeConfiguration.Reference)
            {
                case NeuropixelsV1ReferenceSource.External:
                    {
                        shankBits[ShankBitExt1] = true;
                        shankBits[ShankBitExt2] = true;
                        break;
                    }
                case NeuropixelsV1ReferenceSource.Tip:
                    {
                        shankBits[ShankBitTip1] = true;
                        shankBits[ShankBitTip2] = true;
                        break;
                    }
            }

            return shankBits;
        }

    }
}
