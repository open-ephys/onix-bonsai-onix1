﻿using System;
using System.Drawing;
using System.Xml.Serialization;

namespace OpenEphys.Onix1
{
    /// <summary>
    /// Class defining a <see cref="NeuropixelsV2QuadShankElectrode"/>.
    /// </summary>
    public class NeuropixelsV2QuadShankElectrode : Electrode
    {
        /// <summary>
        /// Gets the bank, or logical block of channels, this electrode belongs to.
        /// </summary>
        [XmlIgnore]
        public NeuropixelsV2QuadShankBank Bank { get; private set; }

        /// <summary>
        /// Gets the block this electrode belongs to.
        /// </summary>
        [XmlIgnore]
        public int Block { get; private set; }

        /// <summary>
        /// Gets the index within the block this electrode belongs to.
        /// </summary>
        [XmlIgnore]
        public int BlockIndex { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NeuropixelsV2QuadShankElectrode"/> class.
        /// </summary>
        /// <param name="index">Integer defining the index of the contact.</param>
        public NeuropixelsV2QuadShankElectrode(int index)
        {
            Index = index;
            Shank = index / NeuropixelsV2.ElectrodePerShank;
            IntraShankElectrodeIndex = index % NeuropixelsV2.ElectrodePerShank;
            Bank = (NeuropixelsV2QuadShankBank)(IntraShankElectrodeIndex / NeuropixelsV2.ChannelCount);
            Block = IntraShankElectrodeIndex % NeuropixelsV2.ChannelCount / NeuropixelsV2.ElectrodePerBlock;
            BlockIndex = IntraShankElectrodeIndex % NeuropixelsV2.ElectrodePerBlock;
            Channel = GetChannelNumber(Shank, Block, BlockIndex);
            Position = GetPosition(index);
        }

        private PointF GetPosition(int electrodeNumber)
        {
            var position = NeuropixelsV2eProbeGroup.DefaultContactPosition(electrodeNumber);
            return new PointF(x: position[0], y: position[1]);
        }

        /// <summary>
        /// Static method returning the channel number of a given electrode.
        /// </summary>
        /// <param name="electrodeIndex">Integer defining the index of the electrode in the probe.</param>
        /// <returns>An integer between 0 and 383 defining the channel number.</returns>
        public static int GetChannelNumber(int electrodeIndex)
        {
            var shank = electrodeIndex / NeuropixelsV2.ElectrodePerShank;
            var shankIndex = electrodeIndex % NeuropixelsV2.ElectrodePerShank;
            var block = shankIndex % NeuropixelsV2.ChannelCount / NeuropixelsV2.ElectrodePerBlock;
            var blockIndex = shankIndex % NeuropixelsV2.ElectrodePerBlock;

            return GetChannelNumber(shank, block, blockIndex);
        }

        internal static int GetChannelNumber(int shank, int block, int blockIndex) => (shank, block) switch
        {
            (0, 0) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 0,
            (0, 1) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 2,
            (0, 2) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 4,
            (0, 3) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 6,
            (0, 4) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 5,
            (0, 5) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 7,
            (0, 6) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 1,
            (0, 7) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 3,

            (1, 0) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 1,
            (1, 1) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 3,
            (1, 2) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 5,
            (1, 3) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 7,
            (1, 4) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 4,
            (1, 5) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 6,
            (1, 6) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 0,
            (1, 7) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 2,

            (2, 0) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 4,
            (2, 1) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 6,
            (2, 2) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 0,
            (2, 3) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 2,
            (2, 4) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 1,
            (2, 5) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 3,
            (2, 6) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 5,
            (2, 7) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 7,

            (3, 0) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 5,
            (3, 1) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 7,
            (3, 2) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 1,
            (3, 3) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 3,
            (3, 4) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 0,
            (3, 5) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 2,
            (3, 6) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 4,
            (3, 7) => blockIndex + NeuropixelsV2.ElectrodePerBlock * 6,

            _ => throw new ArgumentOutOfRangeException($"Invalid shank and/or electrode value: {(shank, block)}"),
        };
    }
}
