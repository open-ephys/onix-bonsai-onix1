﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using Bonsai;

namespace OpenEphys.Onix1
{
    /// <summary>
    /// Produces a sequence of heartbeat data frames.
    /// </summary>
    /// <remarks>
    /// This data IO operator must be linked to an appropriate configuration, such as a <see
    /// cref="ConfigureHeartbeat"/>, using a shared <c>DeviceName</c>.
    /// </remarks>
    [Description("Produces a sequence of heartbeat data frames.")]
    public class HeartbeatData : Source<HeartbeatDataFrame>
    {
        /// <inheritdoc cref = "SingleDeviceFactory.DeviceName"/>
        [TypeConverter(typeof(Heartbeat.NameConverter))]
        [Description(SingleDeviceFactory.DeviceNameDescription)]
        [Category(DeviceFactory.ConfigurationCategory)]
        public string DeviceName { get; set; }

        /// <summary>
        /// Generates a sequence of <see cref="HeartbeatDataFrame"/> objects, each of which contains period signal from the
        /// acquisition system indicating that it is active.
        /// </summary>
        /// <returns>A sequence of <see cref="HeartbeatDataFrame"/> objects.</returns>
        public override IObservable<HeartbeatDataFrame> Generate()
        {
            return DeviceManager.GetDevice(DeviceName).SelectMany(deviceInfo =>
            {
                var device = deviceInfo.GetDeviceContext(typeof(Heartbeat));
                return deviceInfo.Context
                    .GetDeviceFrames(device.Address)
                    .Select(frame => new HeartbeatDataFrame(frame));
            });
        }
    }
}
