﻿using System;
using System.ComponentModel;
using System.Linq;

namespace OpenEphys.Onix
{
    /// <summary>
    /// A class for configuring the ONIX breakout board's analog inputs and outputs.
    /// </summary>
    [TypeConverter(typeof(SortedPropertyConverter))]
    [Description("Configures the analog input and output device in the ONIX breakout board.")]
    public class ConfigureBreakoutAnalogIO : SingleDeviceFactory
    {
        /// <summary>
        /// Initialize a new instance of ConfigureAnalogIO.
        /// </summary>
        public ConfigureBreakoutAnalogIO()
            : base(typeof(BreakoutAnalogIO))
        {
            DeviceAddress = 6;
        }

        /// <summary>
        /// Gets or sets the device enable state.
        /// </summary>
        /// <remarks>
        /// If set to true, <see cref="BreakoutAnalogInput"/> will produce data. If set to false, <see cref="BreakoutAnalogInput"/> will not produce data.
        /// </remarks>
        [Category(ConfigurationCategory)]
        [Description("Specifies whether the analog IO device is enabled.")]
        public bool Enable { get; set; } = true;

        /// <summary>
        /// Gets or sets the input voltage range of channel 0.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 0.")]
        public BreakoutAnalogIOVoltageRange InputRange0 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 1.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 1.")]
        public BreakoutAnalogIOVoltageRange InputRange1 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 2.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 2.")]
        public BreakoutAnalogIOVoltageRange InputRange2 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 3.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 3.")]
        public BreakoutAnalogIOVoltageRange InputRange3 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 4.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 4.")]
        public BreakoutAnalogIOVoltageRange InputRange4 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 5.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 5.")]
        public BreakoutAnalogIOVoltageRange InputRange5 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 6.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 6.")]
        public BreakoutAnalogIOVoltageRange InputRange6 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 7.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 7.")]
        public BreakoutAnalogIOVoltageRange InputRange7 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 8.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 8.")]
        public BreakoutAnalogIOVoltageRange InputRange8 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 9.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 9.")]
        public BreakoutAnalogIOVoltageRange InputRange9 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 10.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 10.")]
        public BreakoutAnalogIOVoltageRange InputRange10 { get; set; }

        /// <summary>
        /// Gets or sets the input voltage range of channel 11.
        /// </summary>
        [Category(ConfigurationCategory)]
        [Description("The input voltage range of channel 11.")]
        public BreakoutAnalogIOVoltageRange InputRange11 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 0.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 0.")]
        public BreakoutAnalogIODirection Direction0 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 1.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 1.")]
        public BreakoutAnalogIODirection Direction1 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 2.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 2.")]
        public BreakoutAnalogIODirection Direction2 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 3.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 3.")]
        public BreakoutAnalogIODirection Direction3 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 4.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 4.")]
        public BreakoutAnalogIODirection Direction4 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 5.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 5.")]
        public BreakoutAnalogIODirection Direction5 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 6.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 6.")]
        public BreakoutAnalogIODirection Direction6 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 7.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 7.")]
        public BreakoutAnalogIODirection Direction7 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 8.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 8.")]
        public BreakoutAnalogIODirection Direction8 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 9.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 9.")]
        public BreakoutAnalogIODirection Direction9 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 10.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 10.")]
        public BreakoutAnalogIODirection Direction10 { get; set; }

        /// <summary>
        /// Gets or sets the direction of channel 11.
        /// </summary>
        [Category(AcquisitionCategory)]
        [Description("The direction of channel 11.")]
        public BreakoutAnalogIODirection Direction11 { get; set; }

        /// <summary>
        /// Configures the analog input and output device in the ONIX breakout board.
        /// </summary>
        /// <remarks>
        /// This will schedule analog IO hardware configuration actions that can be applied by a
        /// <see cref="StartAcquisition"/> object prior to data collection.
        /// </remarks>
        /// <param name="source">
        /// The sequence of <see cref="ContextTask"/> objects on which to apply the analog IO configuration.
        /// </param>
        /// <returns>
        /// A sequence of <see cref="ContextTask"/> objects that is identical to <paramref name="source"/>
        /// in which each <see cref="ContextTask"/> has been instructed to apply the analog IO configuration.
        /// </returns>
        public override IObservable<ContextTask> Process(IObservable<ContextTask> source)
        {
            var deviceName = DeviceName;
            var deviceAddress = DeviceAddress;
            return source.ConfigureDevice(context =>
            {
                var device = context.GetDeviceContext(deviceAddress, DeviceType);
                device.WriteRegister(BreakoutAnalogIO.ENABLE, Enable ? 1u : 0u);
                device.WriteRegister(BreakoutAnalogIO.CH00INRANGE, (uint)InputRange0);
                device.WriteRegister(BreakoutAnalogIO.CH01INRANGE, (uint)InputRange1);
                device.WriteRegister(BreakoutAnalogIO.CH02INRANGE, (uint)InputRange2);
                device.WriteRegister(BreakoutAnalogIO.CH03INRANGE, (uint)InputRange3);
                device.WriteRegister(BreakoutAnalogIO.CH04INRANGE, (uint)InputRange4);
                device.WriteRegister(BreakoutAnalogIO.CH05INRANGE, (uint)InputRange5);
                device.WriteRegister(BreakoutAnalogIO.CH06INRANGE, (uint)InputRange6);
                device.WriteRegister(BreakoutAnalogIO.CH07INRANGE, (uint)InputRange7);
                device.WriteRegister(BreakoutAnalogIO.CH08INRANGE, (uint)InputRange8);
                device.WriteRegister(BreakoutAnalogIO.CH09INRANGE, (uint)InputRange9);
                device.WriteRegister(BreakoutAnalogIO.CH10INRANGE, (uint)InputRange10);
                device.WriteRegister(BreakoutAnalogIO.CH11INRANGE, (uint)InputRange11);

                // Build the whole value for CHDIR and write it once
                static uint SetIO(uint io_reg, int channel, BreakoutAnalogIODirection direction) =>
                    (io_reg & ~((uint)1 << channel)) | ((uint)(direction) << channel);

                var io_reg = 0u;
                io_reg = SetIO(io_reg, 0, Direction0);
                io_reg = SetIO(io_reg, 1, Direction1);
                io_reg = SetIO(io_reg, 2, Direction2);
                io_reg = SetIO(io_reg, 3, Direction3);
                io_reg = SetIO(io_reg, 4, Direction4);
                io_reg = SetIO(io_reg, 5, Direction5);
                io_reg = SetIO(io_reg, 6, Direction6);
                io_reg = SetIO(io_reg, 7, Direction7);
                io_reg = SetIO(io_reg, 8, Direction8);
                io_reg = SetIO(io_reg, 9, Direction9);
                io_reg = SetIO(io_reg, 10, Direction10);
                io_reg = SetIO(io_reg, 11, Direction11);
                device.WriteRegister(BreakoutAnalogIO.CHDIR, io_reg);

                var deviceInfo = new BreakoutAnalogIODeviceInfo(device, this);
                return DeviceManager.RegisterDevice(deviceName, deviceInfo);
            });
        }

        class SortedPropertyConverter : ExpandableObjectConverter
        {
            public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
            {
                var properties = base.GetProperties(context, value, attributes);
                var sortedOrder = properties.Cast<PropertyDescriptor>()
                                            .Where(p => p.PropertyType == typeof(BreakoutAnalogIOVoltageRange)
                                                     || p.PropertyType == typeof(BreakoutAnalogIODirection))
                                            .OrderBy(p => p.PropertyType.MetadataToken)
                                            .Select(p => p.Name)
                                            .Prepend(nameof(Enable))
                                            .ToArray();
                return properties.Sort(sortedOrder);
            }
        }
    }

    static class BreakoutAnalogIO
    {
        public const int ID = 22;

        // constants
        public const int ChannelCount = 12;
        public const int NumberOfDivisions = 1 << 16;

        // managed registers
        public const uint ENABLE = 0;
        public const uint CHDIR = 1;
        public const uint CH00INRANGE = 2;
        public const uint CH01INRANGE = 3;
        public const uint CH02INRANGE = 4;
        public const uint CH03INRANGE = 5;
        public const uint CH04INRANGE = 6;
        public const uint CH05INRANGE = 7;
        public const uint CH06INRANGE = 8;
        public const uint CH07INRANGE = 9;
        public const uint CH08INRANGE = 10;
        public const uint CH09INRANGE = 11;
        public const uint CH10INRANGE = 12;
        public const uint CH11INRANGE = 13;

        internal class NameConverter : DeviceNameConverter
        {
            public NameConverter()
                : base(typeof(BreakoutAnalogIO))
            {
            }
        }
    }

    class BreakoutAnalogIODeviceInfo : DeviceInfo
    {
        public BreakoutAnalogIODeviceInfo(DeviceContext device, ConfigureBreakoutAnalogIO deviceFactory)
            : base(device, deviceFactory.DeviceType)
        {
            VoltsPerDivision = new[]
            {
                GetVoltsPerDivision(deviceFactory.InputRange0),
                GetVoltsPerDivision(deviceFactory.InputRange1),
                GetVoltsPerDivision(deviceFactory.InputRange2),
                GetVoltsPerDivision(deviceFactory.InputRange3),
                GetVoltsPerDivision(deviceFactory.InputRange4),
                GetVoltsPerDivision(deviceFactory.InputRange5),
                GetVoltsPerDivision(deviceFactory.InputRange6),
                GetVoltsPerDivision(deviceFactory.InputRange7),
                GetVoltsPerDivision(deviceFactory.InputRange8),
                GetVoltsPerDivision(deviceFactory.InputRange9),
                GetVoltsPerDivision(deviceFactory.InputRange10),
                GetVoltsPerDivision(deviceFactory.InputRange11)
            };
        }

        public static float GetVoltsPerDivision(BreakoutAnalogIOVoltageRange voltageRange)
        {
            return voltageRange switch
            {
                BreakoutAnalogIOVoltageRange.TenVolts => 20.0f / BreakoutAnalogIO.NumberOfDivisions,
                BreakoutAnalogIOVoltageRange.TwoPointFiveVolts => 5.0f / BreakoutAnalogIO.NumberOfDivisions,
                BreakoutAnalogIOVoltageRange.FiveVolts => 10.0f / BreakoutAnalogIO.NumberOfDivisions,
                _ => throw new ArgumentOutOfRangeException(nameof(voltageRange)),
            };
        }

        public float[] VoltsPerDivision { get; }
    }

    /// <summary>
    /// Specifies the analog input ADC voltage range.
    /// </summary>
    public enum BreakoutAnalogIOVoltageRange
    {
        /// <summary>
        /// ±10.0 volts.
        /// </summary>
        [Description("+/-10.0 volts")]
        TenVolts = 0,
        /// <summary>
        /// ±2.5 volts.
        /// </summary>
        [Description("+/-2.5 volts")]
        TwoPointFiveVolts = 1,
        /// <summary>
        /// ±5.0 volts.
        /// </summary>
        [Description("+/-5.0 volts")]
        FiveVolts,
    }

    /// <summary>
    /// Specifies analog channel direction.
    /// </summary>
    public enum BreakoutAnalogIODirection
    {
        /// <summary>
        /// Input to breakout board.
        /// </summary>
        Input = 0,
        /// <summary>
        /// Output from breakout board with loopback.
        /// </summary>
        Output = 1
    }

    /// <summary>
    /// Specifies the analog sample representation.
    /// </summary>
    public enum BreakoutAnalogIODataType
    {
        /// <summary>
        /// Twos-complement encoded signed 16-bit integer
        /// </summary>
        S16,
        /// <summary>
        /// 32-bit floating point voltage.
        /// </summary>
        Volts
    }
}
